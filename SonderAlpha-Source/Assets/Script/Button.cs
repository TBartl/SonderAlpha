using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour {
	
	public enum Command {none, frameStartServer, frameJoinServer, frameCustomizeCharacter, cycleSkin, cycleEyes, cycleColor, startServer}
	private GameManager gameManager;

	private Vector3 originalPosition;
	public static float hoverPosition, originalHilightScale = .6f,speed = 6f,hilightScale = 1.2f;
	private bool mouseOver;
	private GameObject hilight;
	public Command command;
	private float black;
	// Use this for initialization
	void Start () {
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
		originalPosition = transform.localPosition;
		hilight = transform.FindChild("Hilight").gameObject;
		black = 0f;
	}

	// Update is called once per frame
	void Update () {
		black = Mathf.Max(0, black-Time.deltaTime);
		hilight.renderer.material.color = new Color(1-black,1-black,1-black);
		if (mouseOver == false)
		{

			transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime*speed);
			hilight.transform.localScale = Vector3.Lerp(hilight.transform.localScale, Vector3.one*originalHilightScale, Time.deltaTime*speed/4f);

		}
		
		mouseOver = false;
	}

	void OnMouseOver()
	{
		mouseOver = true;
		transform.localPosition = Vector3.Lerp(transform.localPosition,new Vector3(originalPosition.x, originalPosition.y, originalPosition.z+hoverPosition), Time.deltaTime*speed);
		hilight.transform.localScale = Vector3.Lerp(hilight.transform.localScale, Vector3.one*hilightScale, Time.deltaTime*speed);
		if (Input.GetMouseButtonDown(0))
		{
			black = 1f;
			if (command == Command.frameStartServer)
			{
				Frame temp = GameObject.Find("MainFrame").GetComponent<Frame>();
				temp.Move(2);
				temp = GameObject.Find("StartServerFrame").GetComponent<Frame>();
				temp.Move(1);
			}
			else if (command == Command.frameJoinServer)
			{
				Frame temp = GameObject.Find("MainFrame").GetComponent<Frame>();
				temp.Move(2);
				temp = GameObject.Find("JoinServerFrame").GetComponent<Frame>();
				temp.Move(1);
			}
			else if (command == Command.frameCustomizeCharacter)
			{
				Frame temp = GameObject.Find("MainFrame").GetComponent<Frame>();
				temp.Move(2);
				temp = GameObject.Find("CustomizeCharacterFrame").GetComponent<Frame>();
				temp.Move(1);
			} 
			else if (command == Command.cycleSkin)
			{
				gameManager.SendMessage("CycleSkin",SendMessageOptions.DontRequireReceiver);
			}
			else if (command == Command.cycleEyes)
			{
				gameManager.CycleEyes();
			}
			else if (command == Command.cycleColor)
			{
				gameManager.CycleColor();
			}
			else if (command == Command.startServer || command == Command.startServer|| command == Command.startServer)
			{
				gameManager.StartServer();
			}
		}
	}
	public void CycleSkin(string s)
	{
		TextMesh t = transform.GetComponentInChildren<TextMesh>();
		t.text = "Skin:\n\"" + s + "\"";
	}
	public void CycleEyes(string s)
	{
		TextMesh t = transform.GetComponentInChildren<TextMesh>();
		t.text = "Eyes:\n\"" + s + "\"";
	}
	public void CycleColor(string s)
	{
		TextMesh t = transform.GetComponentInChildren<TextMesh>();
		t.text = "Color:\n\"" + s + "\"";
	}
}
