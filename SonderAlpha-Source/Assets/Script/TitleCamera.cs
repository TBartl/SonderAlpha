using UnityEngine;
using System.Collections;

public class TitleCamera : MonoBehaviour {
	public float cameraSmoothing,maxRotation;

	// Use this for initializations
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 temp = new Vector2(Input.mousePosition.x/Screen.width-.5f,Input.mousePosition.y/Screen.height-.5f);
		transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(new Vector3(-temp.y*maxRotation,temp.x*maxRotation,0)), cameraSmoothing*Time.deltaTime);
	}
}
