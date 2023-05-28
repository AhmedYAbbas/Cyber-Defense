using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Variables

    public static GameManager Instance { get; private set; }
    public GameObject TowerManager;
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
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += ShowAds;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= ShowAds;
    }

    private void Start()
    {
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
        {
            MatchManager.Instance.EndRound(true);
        }
    }

    #endregion

    public void UpdateUI()
    {
        // Start timer
        // TODO: Consider syncing the timer instead of just enabling it
        UILayer.Instance.timerGameObject.SetActive(true);

        // Set rounds text
        UILayer.Instance.roundText.gameObject.SetActive(true);
        UILayer.Instance.roundText.text = "Round: " + MatchManager.Instance.currentRound;

        // Just for Debugging purposes and most likely gonna change the workflow entirely
        UILayer.Instance.p1WinsText.gameObject.SetActive(true);
        UILayer.Instance.p2WinsText.gameObject.SetActive(true);

        for (int i = 0; i < PhotonNetwork.PlayerList.Length;)
        {
            UILayer.Instance.p1WinsText.text = $"P1 Wins: " + (int)PhotonNetwork.PlayerList[0].CustomProperties[CustomKeys.WINS];
            UILayer.Instance.p2WinsText.text = $"P2 Wins: " + (int)PhotonNetwork.PlayerList[1].CustomProperties[CustomKeys.WINS];
            break;
        }

        // Load players UI
        UILayer.Instance.LoadPlayerUI((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE]);
    }

    private void ShowAds(EventData obj)
    {
        if (obj.Code == MatchManager.AdwareAbilityEventCode)
        {
            UILayer.Instance.ShowAds();
        }
    }

}
