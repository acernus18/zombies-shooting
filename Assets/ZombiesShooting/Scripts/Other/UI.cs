﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;
using ExitGames.Client.Photon;

public class UI : MonoBehaviour
{
	public GUISkin Skin;
	public Vector2 WidthAndHeight = new Vector2(600, 400);
	private string roomName = "myRoom";

	private Vector2 scrollPos = Vector2.zero;

	private bool connectFailed = false;

	public static readonly string SceneNameMenu = "DemoWorker-Scene";

	public static readonly string SceneNameGame = "Main";

	private string errorDialog;
	private double timeToClearDialog;

	public string ErrorDialog
	{
		get { return this.errorDialog; }
		private set
		{
			this.errorDialog = value;
			if (!string.IsNullOrEmpty(value))
			{
				this.timeToClearDialog = Time.time + 4.0f;
			}
		}
	}

	public void Awake()
	{
		PhotonNetwork.automaticallySyncScene = true;

		if (PhotonNetwork.connectionStateDetailed == ClientState.PeerCreated)
		{
			PhotonNetwork.ConnectUsingSettings("0.9");
		}

		if (String.IsNullOrEmpty(PhotonNetwork.playerName))
		{
			PhotonNetwork.playerName = "Guest" + Random.Range(1, 9999);
		}
	}

	public void OnGUI()
	{
		if (this.Skin != null)
		{
			GUI.skin = this.Skin;
		}

		if (!PhotonNetwork.connected)
		{
			if (PhotonNetwork.connecting)
			{
				GUILayout.Label("Connecting to: " + PhotonNetwork.ServerAddress);
			}
			else
			{
				GUILayout.Label("Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress);
			}

			if (this.connectFailed)
			{
				GUILayout.Label("Connection failed. Check setup and use Setup Wizard to fix configuration.");
				GUILayout.Label(String.Format("Server: {0}", new object[] { PhotonNetwork.ServerAddress }));
				GUILayout.Label("AppId: " + PhotonNetwork.PhotonServerSettings.AppID.Substring(0, 8) + "****"); // only show/log first 8 characters. never log the full AppId.

				if (GUILayout.Button("Try Again", GUILayout.Width(100)))
				{
					this.connectFailed = false;
					PhotonNetwork.ConnectUsingSettings("0.9");
				}
			}

			return;
		}

		Rect content = new Rect((Screen.width - this.WidthAndHeight.x) / 2, (Screen.height - this.WidthAndHeight.y) / 2, this.WidthAndHeight.x, this.WidthAndHeight.y);
		GUI.Box(content, "Join or Create Room");
		GUILayout.BeginArea(content);

		GUILayout.Space(40);

		// Player name
		GUILayout.BeginHorizontal();
		GUILayout.Label("Player name:", GUILayout.Width(150));
		PhotonNetwork.playerName = GUILayout.TextField(PhotonNetwork.playerName);
		GUILayout.Space(158);
		if (GUI.changed)
		{
			// Save name
			PlayerPrefs.SetString("playerName", PhotonNetwork.playerName);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(15);

		// Join room by title
		GUILayout.BeginHorizontal();
		GUILayout.Label("Roomname:", GUILayout.Width(150));
		this.roomName = GUILayout.TextField(this.roomName);

		if (GUILayout.Button("Create Room", GUILayout.Width(150)))
		{
			PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { MaxPlayers = 10 }, null);
		}
		GUILayout.EndHorizontal();

		if (!string.IsNullOrEmpty(ErrorDialog))
		{
			GUILayout.Label(ErrorDialog);

			if (this.timeToClearDialog < Time.time)
			{
				this.timeToClearDialog = 0;
				ErrorDialog = "";
			}
		}
		GUILayout.Space(15);

		// Join random room
		GUILayout.BeginHorizontal();

		GUILayout.Label(PhotonNetwork.countOfPlayers + " users are online in " + PhotonNetwork.countOfRooms + " rooms.");
		GUILayout.FlexibleSpace();

		GUILayout.EndHorizontal();

		GUILayout.Space(15);
		if (PhotonNetwork.GetRoomList().Length == 0)
		{
			GUILayout.Label("Currently no games are available.");
			GUILayout.Label("Rooms will be listed here, when they become available.");
		}
		else
		{
			GUILayout.Label(PhotonNetwork.GetRoomList().Length + " rooms available:");

			// Room listing: simply call GetRoomList: no need to fetch/poll whatever!
			this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
			foreach (RoomInfo roomInfo in PhotonNetwork.GetRoomList())
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(roomInfo.Name + " " + roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers);
				if (GUILayout.Button("Join", GUILayout.Width(150)))
				{
					PhotonNetwork.JoinRoom(roomInfo.Name);
				}

				GUILayout.EndHorizontal();
			}

			GUILayout.EndScrollView();
		}

		GUILayout.EndArea();
	}

	// We have two options here: we either joined(by title, list or random) or created a room.
	public void OnJoinedRoom()
	{
		Debug.Log("OnJoinedRoom");
	}

	public void OnCreatedRoom()
	{
		Debug.Log("OnCreatedRoom");
		PhotonNetwork.LoadLevel(SceneNameGame);
	}

	public void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon.");
	}

	public void OnFailedToConnectToPhoton(object parameters)
	{
		this.connectFailed = true;
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.ServerAddress);
	}

	public void OnConnectedToMaster()
	{
		Debug.Log("As OnConnectedToMaster() got called, the PhotonServerSetting.AutoJoinLobby must be off. Joining lobby by calling PhotonNetwork.JoinLobby().");
		PhotonNetwork.JoinLobby();
	}
}
