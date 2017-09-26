using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	public GameObject mainPlayerPrefab;
	public string gameTypeName;
	public string gameName;
	private HostData[] hostList;
	private Player modelCharacterPlayer;
	private int skin,eye,color;
	public string[] skinsNames;
	public Texture[] skins;
	public Texture[] tats;
	public string[] eyesNames;
	public Texture[] eyes;
	public string[] colorNames;
	public Color[] colors;

	void Start()
	{
		modelCharacterPlayer = GameObject.Find("ModelCharacter").transform.GetComponent<Player>();
		skin = 0;
		eye = 0;
	}

	void OnGUI()
	{
		if (!Network.isClient && !Network.isServer)
		{

		}
	}

	public void CycleSkin()
	{
		skin = (skin+1)%skins.Length;
		modelCharacterPlayer.body.renderer.material.SetTexture(0, skins[skin]);
		modelCharacterPlayer.body.renderer.material.SetTexture("_SecondTex",tats[skin]);
		Button b = GameObject.Find("Frames/CustomizeCharacterFrame/Button1").GetComponent<Button>();
		b.CycleSkin(skinsNames[skin]);
	}
	public void CycleEyes()
	{
		eye = (eye+1)%eyes.Length;
		modelCharacterPlayer.eyes[0].renderer.material.SetTexture(0, eyes[eye]);
		modelCharacterPlayer.eyes[1].renderer.material.SetTexture(0, eyes[eye]);
		Button b = GameObject.Find("Frames/CustomizeCharacterFrame/Button2").GetComponent<Button>();
		b.CycleEyes(eyesNames[eye]);
	}
	public void CycleColor()
	{
		color = (color+1)%colors.Length;
		modelCharacterPlayer.SetColor(colors[color]);
		Button b = GameObject.Find("Frames/CustomizeCharacterFrame/Button3").GetComponent<Button>();
		b.CycleColor(colorNames[color]);

	}

	public void StartServer()
	{
		Network.InitializeServer(8, 7777, !Network.HavePublicAddress());
		MasterServer.RegisterHost(gameTypeName, gameName);
	}
	void JoinServer(HostData hostData)
	{
		Network.Connect(hostData);
	}
	void RefreshHostList()
	{
		MasterServer.RequestHostList(gameTypeName);
	}
	void SpawnPlayer()
	{
		//Application.LoadLevel("mainGame");
		Application.LoadLevel("testGame");
	}

	void OnServerInitialized()
	{
		SpawnPlayer(); 

	}
	void OnConnectedToServer()
	{
		SpawnPlayer();
	}
	void OnMasterServerEvent(MasterServerEvent msEvent)
	{
		if (msEvent == MasterServerEvent.HostListReceived)
			hostList = MasterServer.PollHostList();
	}

	void OnLevelWasLoaded(int x)
	{
		if (x ==1 || x == 2)
		{
			GameObject temp = (GameObject)Network.Instantiate(mainPlayerPrefab, Vector3.up, Quaternion.identity, 0);
			Player tempPlayer = temp.GetComponent<Player>();
			tempPlayer.body.renderer.material.SetTexture(0,skins[skin]);
			tempPlayer.body.renderer.material.SetTexture("_SecondTex",tats[skin]);
			tempPlayer.eyes[0].renderer.material.SetTexture(0,eyes[eye]);
			tempPlayer.eyes[1].renderer.material.SetTexture(0,eyes[eye]);
			tempPlayer.SetColor(colors[color]);
			Screen.lockCursor = false;
			Screen.showCursor = false;
		}
	}


}
