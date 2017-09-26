using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {

	private Vector3 angles = Vector3.zero;
	// Update is called once per frame
	void Update () {
		angles = new Vector3 ((angles.x + Mathf.PI*Time.deltaTime/4f)%(2*Mathf.PI),(angles.y + Mathf.PI*Time.deltaTime/4f)%(2*Mathf.PI),(angles.z + Mathf.PI*Time.deltaTime/4f)%(2*Mathf.PI));
		transform.position = 5*Vector3.down +new Vector3 (Mathf.Sin(angles.x), .3f*Mathf.Sin(angles.y+Mathf.PI/2f), Mathf.Cos(angles.x));
	}
}
