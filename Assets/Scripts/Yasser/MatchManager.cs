using System.Collections;
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

    #endregion

    #region Public Variables

    public enum Side
    {
        Attacker,
        Defender
    }

    public float roundTime = 12f;
    public int currentRound = 1;

    // TODO: Define a setter maybe and change it to be private
    public bool _destroyedDefenderBase;

    public static MatchManager Instance { get; private set; }

    #endregion

    #region Private Variables

    [SerializeField] string GAMEPLAY_SCENE_NAME;

    private Side _pSide = Side.Defender;

    private const int WINS_REQUIRED = 2;
    private const float ROUND_START_TIME = 13f;
    private int _p1Wins;
    private int _p2Wins;


    #endregion

    #region Unity Callbacks

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += SwitchSide;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= SwitchSide;

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
                [CustomKeys.WINS] = _p1Wins
            };

            PhotonNetwork.LocalPlayer.SetCustomProperties(masterClientProps);

            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] = _pSide;
            PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.WINS] = _p1Wins;
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Hashtable newPlayerProps = new Hashtable()
        {
            [CustomKeys.P_SIDE] = 1 - _pSide,
            [CustomKeys.WINS] = _p2Wins
        };

        newPlayer.SetCustomProperties(newPlayerProps);

        newPlayer.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)PhotonNetwork.MasterClient.CustomProperties[CustomKeys.P_SIDE];
        newPlayer.CustomProperties[CustomKeys.WINS] = _p2Wins;

        StartMatch();
    }

    #endregion

    #region Public Methods

    public void ResetMatch()
    {
        _p1Wins = _p2Wins = 0;
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

    #endregion

    #region Private Methods

    public void StartMatch()
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
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                // Not sure if that's the right thing and I need more testing with advanced cases
                // Maybe when we add the main logic??
                //if (PhotonNetwork.IsMasterClient)
                CheckWinner(player);

                player.CustomProperties[CustomKeys.P_SIDE] = 1 - (Side)player.CustomProperties[CustomKeys.P_SIDE];
            }


            if (currentRound >= 2)
            {
                if (_p1Wins > _p2Wins)
                {
                    Debug.Log("P1 Wins");
                    //UILayer.Instance.GameEndedPanel.SetActive(true);
                    //Time.timeScale = 0;
                }
                else if (_p2Wins > _p1Wins)
                {
                    Debug.Log("P2 Wins");
                    //UILayer.Instance.GameEndedPanel.SetActive(true);
                    //Time.timeScale = 0;
                }
            }

            print(currentRound + " " + _p1Wins + " " + _p2Wins);

            StartCoroutine(UILayer.Instance.EnableSwitchingSidesPanel());

            ResetTime();
            StartMatch();
            currentRound++;

            if ((Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == Side.Attacker)
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
        }
        else if ((Side)player.CustomProperties[CustomKeys.P_SIDE] == Side.Defender && roundTime <= 0)
        {
            RecordRoundWinner(player);
        }
    }

    #endregion
}
