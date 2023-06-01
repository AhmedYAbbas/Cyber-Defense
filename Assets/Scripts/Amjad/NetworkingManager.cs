using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkingManager : MonoBehaviourPunCallbacks
{
    // private variables
    private static NetworkingManager _instance;
    [SerializeField] private TMP_InputField if_playerNickname;

    // public variables
    public static NetworkingManager Instance { get => _instance; }


    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.GameVersion = "v0.2";
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

    public void QuickMatch()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public void CreateOrJoinRoom(string RoomName)
    {
        PhotonNetwork.JoinOrCreateRoom(RoomName, new RoomOptions { MaxPlayers = 2 }, null, null);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
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