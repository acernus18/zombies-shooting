using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMainController : Photon.MonoBehaviour
{
	public GameObject currentPlayer;

	void OnJoinedRoom()
	{
		PhotonNetwork.player.NickName = "Player" + PhotonNetwork.player.ID;
		Vector3 pos = GameSettings.In.positions[PhotonNetwork.player.ID - 1];
		currentPlayer = PhotonNetwork.Instantiate("Player", pos, Quaternion.identity, 0);
	}
}
