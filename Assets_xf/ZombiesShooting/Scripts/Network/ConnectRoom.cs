using System.Collections;
using System.Collections;
using UnityEngine;

public class ConnectRoom : Photon.MonoBehaviour
{
	private Transform border;
	private bool initialization;
	private int amountAlone;

	public GameObject PrefabPlayer;

	public GameObject Player;

	public string Version;

	public bool InConnectUpdate;

	public bool Viewer;

	public Transform Field;

	public GameObject Bomb;

	// Use this for initialization
	void Start()
	{
		
		PhotonNetwork.autoJoinLobby = true;
//		var bomb = Instantiate (Bomb);
//		bomb.transform.position = new Vector2(5, 5);
//		bomb.transform.localScale = new Vector2(5,5);
	}

	// Update is called once per frame
	void Update()
	{
		if (!InConnectUpdate && !PhotonNetwork.connected)
		{
			InConnectUpdate = true;
			PhotonNetwork.ConnectUsingSettings(Version);
		}
	}

	void OnJoinedLobby()
	{
		RoomOptions roomOptions = new RoomOptions() { MaxPlayers = 20 };
		PhotonNetwork.JoinOrCreateRoom("ZombiesShooting", roomOptions, null);
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log("OnFailedToConnectToPhoton caused by" + cause);
	}

	void OnJoinedRoom()
	{
		Debug.Log("On Joined Room");
		PhotonNetwork.player.NickName = "Player" + PhotonNetwork.player.ID;
		Player = PhotonNetwork.Instantiate("Player", new Vector3(0, 0, -1), Quaternion.identity, 0);
		Debug.Log("Load done");
	}
}
