using UnityEngine;
using System.Collections;

public class GroundDetection : MonoBehaviour {

	void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "Terrain")
		{
			SendMessageUpwards("CheckGrounded", true,SendMessageOptions.DontRequireReceiver);
		}
	}
}
