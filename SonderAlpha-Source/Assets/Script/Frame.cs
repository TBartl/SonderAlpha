using UnityEngine;
using System.Collections;

public class Frame : MonoBehaviour {
	public float speed;
	public Vector3[] positions;

	public Vector3[] framePositions;
	public int position;
	public MeshCollider[] buttons;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, positions[position], Time.deltaTime*speed);
	}

	public void Move(int x)
	{
		position = x;
		if (x == 2)
		{
			for (int index = 0; index < buttons.Length; index += 1)
			{
				buttons[index].enabled = false;
			}
		}
		else if (x == 1)
		{
			for (int index = 0; index < buttons.Length; index += 1)
			{
				buttons[index].enabled = true;
			}
		}

	}

	public void OnMouseOver()
	{
		if (position == 2 && Input.GetMouseButtonDown(0))
		{
			Frame temp = GameObject.Find("MainFrame").GetComponent<Frame>();
			temp.SendMessage("Move",0,SendMessageOptions.DontRequireReceiver);
			temp = GameObject.Find("StartServerFrame").GetComponent<Frame>();
			temp.SendMessage("Move",0,SendMessageOptions.DontRequireReceiver);
			temp = GameObject.Find("JoinServerFrame").GetComponent<Frame>();
			temp.SendMessage("Move",0,SendMessageOptions.DontRequireReceiver);
			temp = GameObject.Find("CustomizeCharacterFrame").GetComponent<Frame>();
			temp.SendMessage("Move",0,SendMessageOptions.DontRequireReceiver);

			Move(1);
		}
	}
}
