using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class CustomAnimator : MonoBehaviour {
	public EvaluationCurveType currentEvaluationCurveType;
	public int currentPose;
	public int targetPose;
	public float evaluate;
	public AnimationCurve evaluationCurve;

	public AnimationCurve linearCurve;
	public AnimationCurve smoothCurve;
	public AnimationCurve twoSidedSpringCurve;
	public AnimationCurve discontinuityCurve;
	public List<AnimationClip> posesToEvaluate;
	public List<Transform> body;
	public List<Pose> poses;

	[System.Serializable]
	public class Pose
	{
		public Quaternion[] individualBones;
		public Quaternion[] flippedBones;
		public Vector3 rootPosition;
		public Pose(AnimationClip clip, List<Transform> body)
		{
			AnimationClipCurveData[] data = AnimationUtility.GetAllCurves(clip);
			individualBones = new Quaternion[data.Length/10];
			flippedBones = new Quaternion[data.Length/10];
			for(int index = 0; index < data.Length/10; index++)
			{
				Quaternion q = new Quaternion(data[index*4].curve.Evaluate(0),data[index*4+1].curve.Evaluate(0),data[index*4+2].curve.Evaluate(0),data[index*4+3].curve.Evaluate(0));
				Quaternion flippedQuaternion = new Quaternion(-q.x, q.y, q.z, -q.w);
				string path = data[index*4].path;
				string flippedPath = path;
				if (flippedPath.EndsWith("L"))
					flippedPath = flippedPath.Remove(flippedPath.Length-1) + "R";
				else if (flippedPath.EndsWith("R"))
					flippedPath = flippedPath.Remove(flippedPath.Length-1) + "L";
				for (int index2 = 0; index2 < data.Length/10; index2++)
				{
					if (path.EndsWith(body[index2].name))
						individualBones[index2] = q;
					if (flippedPath.EndsWith(body[index2].name))
						flippedBones[index2] = flippedQuaternion;
				}
			}
			rootPosition = new Vector3( data[data.Length*4/10].curve.Evaluate(0), data[data.Length*4/10+1].curve.Evaluate(0), data[data.Length*4/10+2].curve.Evaluate(0));

		}


		public Quaternion Evaluate(int index, int posePosition)
		{
			if (posePosition > 0)
				return individualBones[index];
			return flippedBones[index];
		}
	}
	[System.Serializable]
	public enum EvaluationCurveType
	{
		linear, smooth, twoSidedSpring, discontinuity
	}



	public List<AnimationCurve> animationCurves;

	void Start () {
		SetRagdoll(false);
		Transform[] allChildren = GetComponentsInChildren<Transform>();
		foreach (Transform t in allChildren)
		{
			if (!t.name.StartsWith("NB"))
				body.Add(t);
		}
		foreach (AnimationClip clip in posesToEvaluate)
		{
			poses.Add(new Pose(clip,body));
		}
		ChangeAnimationCurve(EvaluationCurveType.smooth);
	}


	void Update () {
		evaluate += Time.deltaTime;
		if (evaluate > 1)
		{
			evaluate -= 1;
			int temp = targetPose;
			targetPose = currentPose;
			currentPose = temp;
		}
		for (int index = 0; index < body.Count; index++)
		{
			body[index].localRotation = SlerpNoClamps(poses[Mathf.Abs(currentPose)-1].Evaluate(index, currentPose),
			                                          poses[Mathf.Abs(targetPose)-1].Evaluate(index, targetPose),
			                							evaluationCurve.Evaluate(evaluate));
		}
		transform.localPosition = Vector3.Lerp(poses[Mathf.Abs(currentPose)-1].rootPosition,
		                                       poses[Mathf.Abs(targetPose)-1].rootPosition,
		                                       evaluationCurve.Evaluate(evaluate));
		
	}
	void SetRagdoll(bool option)
	{
		Rigidbody[] bodies = GetComponentsInChildren<Rigidbody>();
		foreach (Rigidbody rb in bodies)
		{
			rb.isKinematic=!option;
		}
	}

	Quaternion SlerpNoClamps(Quaternion q1, Quaternion q2, float t)
	{
		//float theta = Mathf.Acos(Quaternion.Dot(q1, q2));
		//return ScaleQuaternion(q1,Mathf.Sin((1-t)*theta)/Mathf.Sin(theta)) * ScaleQuaternion(q2,Mathf.Sin(t*theta)/Mathf.Sin(theta));
		/*
		Quaternion newQ = new Quaternion();
		float cosHalfTheta = q1.x*q2.x + q1.y*q2.y + q1.z*q2.z + q1.w*q2.w; 
		bool reverse = false;
		if (cosHalfTheta < 0)
		{
			reverse = true;
			cosHalfTheta = -cosHalfTheta;
		}
		float halfTheta = Mathf.Acos(cosHalfTheta);
		float sinHalfTheta = Mathf.Sqrt(1- cosHalfTheta*cosHalfTheta);
		float a = Mathf.Sin((1-t)*halfTheta)/sinHalfTheta;
		float b = Mathf.Sin(t*halfTheta)/sinHalfTheta;
		//Insert Maximum Over Rustle Here
		return newQ;
		*/
		float opposite;
		float inverse;
		float dot = Quaternion.Dot(q1, q2);
		
		if (Mathf.Abs(dot) > 1.0f - .0005f)
		{
			inverse = 1.0f - t;
			opposite = t * Mathf.Sign(dot);
		}
		else
		{
			float acos = (float)Mathf.Acos(Mathf.Abs(dot));
			float invSin = (float)(1.0 / Mathf.Sin(acos));
			
			inverse = (float)Mathf.Sin((1.0f - t) * acos) * invSin;
			opposite = (float)Mathf.Sin(t * acos) * invSin * Mathf.Sign(dot);
		}
		Quaternion result = new Quaternion();
		result.x = (inverse * q1.x) + (opposite * q2.x);
		result.y = (inverse * q1.y) + (opposite * q2.y);
		result.z = (inverse * q1.z) + (opposite * q2.z);
		result.w = (inverse * q1.w) + (opposite * q2.w);
		return result;

	}

	void ChangeAnimationCurve(EvaluationCurveType type)
	{
		currentEvaluationCurveType = type;
		if (type == EvaluationCurveType.linear)
			evaluationCurve = linearCurve;
		else if (type == EvaluationCurveType.smooth)
			evaluationCurve = smoothCurve;
		else if (type == EvaluationCurveType.twoSidedSpring)
			evaluationCurve = twoSidedSpringCurve;
		else if (type == EvaluationCurveType.discontinuity)
			evaluationCurve = discontinuityCurve;

	}

}
