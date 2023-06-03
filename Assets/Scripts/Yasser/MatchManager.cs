using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviourPunCallbacks
{
    #region Photon Event Codes

    private const byte RoundEndedEventCode = 1;
    private const byte MatchStartedEventCode = 2;
    private const byte DisconnectPlayersEventCode = 3;


    public const byte MiningUsedEventCode = 4;
    public const byte AdwareAbilityEventCode = 5;

    #endregion

    #region Public Variables

    public enum Side
    {
        Attacker,
        Defender
    }
    public float roundTime;
    public int currentRound = 1;
    public static MatchManager Instance { get; private set; }
    public string MAIN_MENU_SCENE_NAME;
    public string GAMEPLAY_SCENE_NAME;

    #endregion

    #region Private Variables

    private Side _pSide;

    private const int WINS_REQUIRED = 2;
    public static float ROUND_START_TIME = 150f;
    private int _p1Energy;
    private int _p2Energy;
    private bool _disconnected = true;

    #endregion

    #region Unity Callbacks

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += StartMatchDataSet;
        PhotonNetwork.NetworkingClient.EventReceived += SwitchSides;
        PhotonNetwork.NetworkingClient.EventReceived += DisconnectPlayers;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= StartMatchDataSet;
        PhotonNetwork.NetworkingClient.EventReceived -= SwitchSides;
        PhotonNetwork.NetworkingClient.EventReceived -= DisconnectPlayers;
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

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == MatchManager.Instance.GAMEPLAY_SCENE_NAME)
        {
            // TODO: Profiler: cache this list
            if (PhotonNetwork.PlayerList.Length < 2)
            {
                DisconnectPlayersRaiseEvent();
            }
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
                [CustomKeys.WINS] = 0,
                [CustomKeys.ENERGY] = _p1Energy,
                [CustomKeys.User_Name] = PhotonNetwork.LocalPlayer.NickName
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(masterClientProps);

            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] = _pSide;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.WINS] = 0;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY] = _p1Energy;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.User_Name] = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Hashtable newPlayerProps = new Hashtable()
        {
            [CustomKeys.P_SIDE] = 1 - _pSide,
            [CustomKeys.WINS] = 0,
            [CustomKeys.ENERGY] = _p2Energy,
            [CustomKeys.User_Name] = newPlayer.NickName
        };

        newPlayer.SetCustomProperties(newPlayerProps);

        newPlayer.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)PhotonNetwork.MasterClient.CustomProperties[CustomKeys.P_SIDE];
        newPlayer.CustomProperties[CustomKeys.WINS] = 0;
        newPlayer.CustomProperties[CustomKeys.ENERGY] = _p2Energy;
        newPlayer.CustomProperties[CustomKeys.User_Name] = newPlayer.NickName;

        MatchStartedRaiseEvent();
        StartMatch();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        if (_disconnected && SceneManager.GetActiveScene().name == GAMEPLAY_SCENE_NAME)
        {
            DisconnectPlayersRaiseEvent();
            UILayer.Instance.EnableDisconnectionPanel();
        }

        ResetMatch();
    }

    #endregion

    #region Public Methods

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

    public void RoundEndedRaiseEvent()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
            PhotonNetwork.RaiseEvent(RoundEndedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
        }
    }

    public void EndRound(bool isBaseDestroyed)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CheckWinner(isBaseDestroyed);
            RoundEndedRaiseEvent();
        }
    }

    public void SetPlayerDisconnected(bool disconnected)
    {
        _disconnected = disconnected;
    }

    #endregion

    #region Private Methods


    private void ResetMatch()
    {
        currentRound = 1;
        ResetTime();
    }

    private void ResetRound()
    {
        ResetEnergy();
        ResetTime();
        IncreaseRound();
    }

    private void ResetEnergy()
    {
        EnergyManager.Instance._energy = EnergyManager.Instance._maxEnergy;
        EnergyManager.Instance._energySlider.value = EnergyManager.Instance._energy;
    }

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

    private void CheckWinner(bool isBaseDestroyed)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if ((Side)player.CustomProperties[CustomKeys.P_SIDE] == Side.Attacker && isBaseDestroyed)
            {
                int wins = (int)player.CustomProperties[CustomKeys.WINS];
                wins++;

                Hashtable winsProp = new Hashtable { [CustomKeys.WINS] = wins };
                player.SetCustomProperties(winsProp);
                player.CustomProperties[CustomKeys.WINS] = wins;
            }
            else if ((Side)player.CustomProperties[CustomKeys.P_SIDE] == Side.Defender && roundTime <= 0)
            {
                int wins = (int)player.CustomProperties[CustomKeys.WINS];
                wins++;

                Hashtable winsProp = new Hashtable { [CustomKeys.WINS] = wins };
                player.SetCustomProperties(winsProp);
                player.CustomProperties[CustomKeys.WINS] = wins;
            }
        }

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            Debug.Log($"P{i + 1} wins: " + (int)PhotonNetwork.PlayerList[i].CustomProperties[CustomKeys.WINS]);
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

    private void StateWinner()
    {
        for (int i = 0; i < PhotonNetwork.PlayerList.Length;)
        {
            if ((int)PhotonNetwork.PlayerList[0].CustomProperties[CustomKeys.WINS] >
                (int)PhotonNetwork.PlayerList[1].CustomProperties[CustomKeys.WINS])
            {
                UILayer.Instance.GameEndedPanel.SetActive(true);
                UILayer.Instance.winnerText.text = $"<color=yellow>{(string)PhotonNetwork.PlayerList[0].CustomProperties[CustomKeys.User_Name]}</color> Won The Game";
                SetPlayerDisconnected(false);
                PhotonNetwork.Disconnect();
            }
            else if ((int)PhotonNetwork.PlayerList[0].CustomProperties[CustomKeys.WINS] <
                     (int)PhotonNetwork.PlayerList[1].CustomProperties[CustomKeys.WINS])
            {
                UILayer.Instance.GameEndedPanel.SetActive(true);
                UILayer.Instance.winnerText.text = $"<color=yellow>{(string)PhotonNetwork.PlayerList[1].CustomProperties[CustomKeys.User_Name]}</color> Won The Game";
                SetPlayerDisconnected(false);
                PhotonNetwork.Disconnect();
            }

            break;
        }
    }

    // All
    private void SwitchSides(EventData obj)
    {
        if (obj.Code == RoundEndedEventCode)
        {
            ChangeSides();

            if (currentRound >= WINS_REQUIRED)
            {
                StateWinner();
            }

            StartCoroutine(UILayer.Instance.EnableSwitchingSidesPanel(1));

            ResetRound();
            StartMatch();
        }
    }

    private void IncreaseRound()
    {
        currentRound++;
    }

    private void ChangeSides()
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            player.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)player.CustomProperties[CustomKeys.P_SIDE];
        }
    }

    private void DisconnectPlayersRaiseEvent()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(DisconnectPlayersEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    private void DisconnectPlayers(EventData obj)
    {
        if (obj.Code == DisconnectPlayersEventCode)
        {
            PhotonNetwork.Disconnect();
        }
    }

    #endregion

}