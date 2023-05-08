using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviourPunCallbacks
{
    public float roundTime = 10f;

    private const byte TimeEndedEventCode = 1;

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += ResetTime;
        PhotonNetwork.NetworkingClient.EventReceived += SwitchSide;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= ResetTime;
        PhotonNetwork.NetworkingClient.EventReceived -= SwitchSide;
    }

    #region Public Variables

    public enum Side
    {
        Attacker, 
        Defender
    }

    public static MatchManager Instance { get; private set; }

    #endregion

    #region Private Variables

    private const string GAMEPLAY_SCENE_NAME = "CD_Gameplay Scene";

    private Side _pSide = Side.Defender;

    private const int WINS_REQUIRED = 2;
    private int _attackerWins;
    private int _defenderWins;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion  

    #region Photon Callbacks

    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _pSide = (Side)Random.Range(0, 2);

            Hashtable masterClientProps = new Hashtable() { [CustomKeys.P_SIDE] = _pSide };
            PhotonNetwork.LocalPlayer.SetCustomProperties(masterClientProps);

            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] = _pSide;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Hashtable newPlayerProps = new Hashtable() { [CustomKeys.P_SIDE] = 1 - _pSide };
        newPlayer.SetCustomProperties(newPlayerProps);

        newPlayer.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)PhotonNetwork.MasterClient.CustomProperties[CustomKeys.P_SIDE];

        StartMatch();
    }

    #endregion

    #region Public Methods

    public void StartMatch()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(GAMEPLAY_SCENE_NAME);
        }
    }

    public void ResetMatch()
    {
        _attackerWins = _defenderWins = 0;
    }

    public void RecordRoundWinner(Side winner)
    {
        switch (winner)
        {
            case Side.Attacker:
                _attackerWins++;
                break;
            case Side.Defender:
                _defenderWins++;
                break;
        }
    }

    #endregion

    public void SwitchSideRaiseEvent()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(TimeEndedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }


    private void ResetTime(EventData obj)
    {
        if (obj.Code == TimeEndedEventCode)
            roundTime = 5f;
    }

    private void SwitchSide(EventData obj)
    {
        if (obj.Code == TimeEndedEventCode)
        {
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                player.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)player.CustomProperties[CustomKeys.P_SIDE];
            }

            UILayer.Instance.LoadPlayerUI((Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE]);
        }

    }

}
