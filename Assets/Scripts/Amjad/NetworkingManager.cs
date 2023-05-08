using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    // private variables
    private static NetworkingManager _instance;
    [SerializeField] private TMP_InputField if_playerNickname;
    [SerializeField] private GameObject waitingForPlayersPanel;

    // public variables
    public static NetworkingManager Instance { get => _instance; }


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v0.1";
        PhotonNetwork.ConnectUsingSettings();

        if (!_instance)
        {
            _instance = this;
        }

        if (PlayerPrefs.GetString("NickName") != String.Empty)
        {
            PhotonNetwork.NickName = PlayerPrefs.GetString("NickName");
        }
    }

    public void JoinOrCreateRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        waitingForPlayersPanel.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has joined the room");
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void SetNickName()
    {
        PhotonNetwork.NickName = if_playerNickname.text;
    }
}