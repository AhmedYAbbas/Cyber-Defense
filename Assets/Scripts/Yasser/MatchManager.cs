using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

public class MatchManager : MonoBehaviourPunCallbacks
{
    #region Photon Event Codes

    private const byte TimeEndedEventCode = 1;
    private const byte BaseDestroyedEventCode = 2;
    private const byte MatchEndedEventCode = 3;
    private const byte MatchStartedEventCode = 4;

    public const byte MiningUsedEventCode = 5;
    public const byte AdwareAbilityEventCode = 6;

    #endregion

    #region Public Variables

    public enum Side
    {
        Attacker,
        Defender
    }

    public float roundTime;
    public int currentRound = 1;

    // TODO: Define a setter maybe and change it to be private
    public bool _destroyedDefenderBase;

    public static MatchManager Instance { get; private set; }

    #endregion

    #region Private Variables

    [SerializeField] string GAMEPLAY_SCENE_NAME;

    private Side _pSide = Side.Defender;

    private const int WINS_REQUIRED = 2;
    public static float ROUND_START_TIME = 500.0f;
    private int _p1Wins;
    private int _p2Wins;
    private int _p1Energy;
    private int _p2Energy;
    private bool _isWinner;
    private string WinnerUserName;

    #endregion

    #region Unity Callbacks

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += SwitchSide;
        PhotonNetwork.NetworkingClient.EventReceived += DisconnectPlayers;
        PhotonNetwork.NetworkingClient.EventReceived += StartMatchDataSet;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= SwitchSide;
        PhotonNetwork.NetworkingClient.EventReceived -= DisconnectPlayers;
        PhotonNetwork.NetworkingClient.EventReceived -= StartMatchDataSet;
    }

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

            Hashtable masterClientProps = new Hashtable()
            {
                [CustomKeys.P_SIDE] = _pSide,
                [CustomKeys.WINS] = _p1Wins,
                [CustomKeys.ENERGY] = _p1Energy,
                [CustomKeys.User_Name] = PlayerPrefs.GetString("NickName"),
                [CustomKeys.Winner] = _isWinner
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(masterClientProps);

            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] = _pSide;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.WINS] = _p1Wins;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY] = _p1Energy;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.User_Name] = PlayerPrefs.GetString("NickName");
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.Winner] = _isWinner;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Hashtable newPlayerProps = new Hashtable()
        {
            [CustomKeys.P_SIDE] = 1 - _pSide,
            [CustomKeys.WINS] = _p2Wins,
            [CustomKeys.ENERGY] = _p2Energy,
            [CustomKeys.User_Name] = PlayerPrefs.GetString("NickName"),
            [CustomKeys.Winner] = _isWinner
        };

        newPlayer.SetCustomProperties(newPlayerProps);

        newPlayer.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)PhotonNetwork.MasterClient.CustomProperties[CustomKeys.P_SIDE];
        newPlayer.CustomProperties[CustomKeys.WINS] = _p2Wins;
        PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY] = _p2Energy;

        MatchStartedRaiseEvent();

        StartMatch();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        ResetMatch();
    }

    #endregion

    #region Public Methods

    // TODO: we need an event that is called once the game is ready to start so that it resets the data before entering the gameplay scene.
    public void ResetMatch()
    {
        _p1Wins = 0;
        _p2Wins = 0;
        currentRound = 1;
        _destroyedDefenderBase = false;
        ResetTime();
    }

    public void SwitchSideRaiseEvent()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(TimeEndedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void BaseDestroyedRaiseEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(BaseDestroyedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void MatchEndedRaiseEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(MatchEndedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void MatchStartedRaiseEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(MatchStartedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    public void AdwareAbilityRaiseEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(AdwareAbilityEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    #endregion

    #region Private Methods

    private void StartMatch()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            PhotonNetwork.LoadLevel(GAMEPLAY_SCENE_NAME);
        }
    }

    private void ResetTime()
    {
        roundTime = ROUND_START_TIME;
    }

    // Photon Event Method
    private void SwitchSide(EventData obj)
    {
        if (obj.Code == TimeEndedEventCode || obj.Code == BaseDestroyedEventCode)
        {
            _destroyedDefenderBase = true;
            CheckWinner(PhotonNetwork.LocalPlayer);

            foreach (Player player in PhotonNetwork.PlayerList)
            {
                player.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)player.CustomProperties[CustomKeys.P_SIDE];
                if ((bool)player.CustomProperties[CustomKeys.Winner])
                {
                    print((string)player.CustomProperties[CustomKeys.User_Name]);
                    WinnerUserName = (string)player.CustomProperties[CustomKeys.User_Name];
                }
            }

            if (currentRound >= WINS_REQUIRED)
            {
                if (_p1Wins > _p2Wins || _p2Wins > _p1Wins)
                {
                    UILayer.Instance.GameEndedPanel.SetActive(true);
                    UILayer.Instance.winnerText.text = $"<color=yellow>{WinnerUserName}</color> Won The Game";
                    // Disconnects the player
                    MatchEndedRaiseEvent();
                }
            }

            StartCoroutine(UILayer.Instance.EnableSwitchingSidesPanel());

            ResetTime();
            StartMatch();
            currentRound++;
            _destroyedDefenderBase = false;
        }
    }

    private void RecordRoundWinner(Player winner)
    {
        if (winner == PhotonNetwork.MasterClient)
        {
            _p1Wins++;
            winner.CustomProperties[CustomKeys.WINS] = _p1Wins;
        }
        else
        {
            _p2Wins++;
            winner.CustomProperties[CustomKeys.WINS] = _p2Wins;
        }
    }

    private void CheckWinner(Player player)
    {
        if ((Side)player.CustomProperties[CustomKeys.P_SIDE] == Side.Attacker && _destroyedDefenderBase)
        {
            RecordRoundWinner(player);
            player.CustomProperties[CustomKeys.Winner] = true;
        }
        else if ((Side)player.CustomProperties[CustomKeys.P_SIDE] == Side.Defender && roundTime <= 0)
        {
            RecordRoundWinner(player);
            player.CustomProperties[CustomKeys.Winner] = true;
        }
        else
        {
            player.CustomProperties[CustomKeys.Winner] = false;
            RecordRoundWinner(PhotonNetwork.PlayerListOthers[0]);
        }
    }

    private void DisconnectPlayers(EventData obj)
    {
        if (obj.Code == MatchEndedEventCode)
        {
            PhotonNetwork.Disconnect();
        }
    }

    // Reset Match Data
    private void StartMatchDataSet(EventData obj)
    {
        if (obj.Code == MatchStartedEventCode)
        {
            ResetMatch();
        }
    }

    #endregion
}