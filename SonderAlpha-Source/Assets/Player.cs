using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public enum GroundState
{
	inAir, onGround, onWall
}


public class Player : MonoBehaviour {

	//public Animation aTest;
	public float health;
	public Color playerColor;
	public Transform armatureHolder;
	public GameObject body;
	public ParticleSystem[] eyes = new ParticleSystem[2];
	public Light headlight;

	public float maxSpeed = 7f;

	public float turnSmoothing;
	
	public float wallJumpModifier = 1.5f;
	public Vector3 surfaceNormal = Vector3.zero;
	public GroundState groundState = GroundState.inAir;
	public List<Vector3> contactPoints;


	public enum PlayerPoses
	{
		idle, running
	}

	// Use this for initialization
	public void Awake () {

		FindBody();
		SetColor (playerColor);
		//animator = GetComponent<Animator> ();
		//SetRagdoll (false);
		contactPoints = new List<Vector3> ();
		//aTest.Sample ();
		//Transform[] transforms = animation.gameObject.GetComponentsInChildren<Transform>();
                           		//Debug.Log (transforms);
	}
	

	// Update is called once per frame
	public void FixedUpdate()
	{
		ConfigureGroundState();
		UpdateAnimator ();


	}
	
	void LateUpdate() {
		armatureHolder.position = transform.position;
		Vector2 groundVelocity = new Vector2 (rigidbody.velocity.x, rigidbody.velocity.z);
		if (groundVelocity.magnitude > .01f) 
		{
			if (groundVelocity.x >= 0){
				armatureHolder.transform.rotation = Quaternion.Slerp(armatureHolder.transform.rotation,Quaternion.Euler(new Vector3(0,Vector2.Angle(Vector2.up, groundVelocity.normalized),0)),Time.deltaTime*turnSmoothing);
			}
			else{
				armatureHolder.transform.rotation = Quaternion.Slerp(armatureHolder.transform.rotation,Quaternion.Euler(new Vector3(0,360-Vector2.Angle(Vector2.up, groundVelocity.normalized),0)),Time.deltaTime*turnSmoothing);
			}
		}
		
	}


	public void SetColor(Color c)
	{
		playerColor = c;
		body.renderer.material.color = c;
		foreach (ParticleSystem p in eyes)
			p.startColor = c;
		headlight.color = c;
	}

	void FindBody()
	{
		body = transform.FindChild("Cylinder").gameObject;
		armatureHolder = transform.root.FindChild ("ArmatureHolder");
		eyes[0] = armatureHolder.FindChild("Armature/Hips/Spine/Chest/Neck/Head/NB_LeftEye").particleSystem;
		eyes[1] = armatureHolder.FindChild("Armature/Hips/Spine/Chest/Neck/Head/NB_RightEye").particleSystem;
		headlight = armatureHolder.FindChild("Armature/Hips/Spine/Chest/Neck/Head/NB_Headlights").light;
	}

	public void OnCollisionStay(Collision collisionInfo) {
		foreach (ContactPoint contact in collisionInfo.contacts) {			
			contactPoints.Add(contact.point);
		}
	}

	void ConfigureGroundState()
	{
		Vector3 playerCenter = transform.position + ((CapsuleCollider)collider).center;
		float bestDot = 1;
		Vector3 bestLine = Vector3.zero;
		foreach (Vector3 contactPoint in contactPoints) {
			Vector3 slope = (playerCenter - contactPoint).normalized;
			if (Vector3.Dot(slope, Vector3.down) < bestDot)
			{
				bestDot = Vector3.Dot(slope, Vector3.down);
				bestLine = slope;
			}
		}
		surfaceNormal = bestLine;
		groundState = GroundState.inAir;
		if (surfaceNormal.magnitude > .05f) 
		{
			if (Mathf.Abs(surfaceNormal.y) > .7f)
				groundState = GroundState.onGround;
			else
				groundState = GroundState.onWall;
		}
	}

	void UpdateAnimator()
	{
		/*
		animator.SetBool("running", (rigidbody.velocity.magnitude > .4f));
		animator.SetFloat ("strafeDirection", transform.InverseTransformDirection(rigidbody.velocity).x / maxSpeed);
		animator.SetFloat ("forwardDirection", transform.InverseTransformDirection(rigidbody.velocity).z / maxSpeed);
		animator.SetInteger ("groundState", (int)groundState);
		animator.SetFloat("upVelocity", rigidbody.velocity.y);
		*/
	}





}
