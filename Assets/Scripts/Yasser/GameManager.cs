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
        UpdateUI();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= ShowAds;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
        {
            MatchManager.Instance.BaseDestroyedRaiseEvent();
            foreach (Player player in PhotonNetwork.PlayerList)
            {
                Debug.Log(player.NickName);
            }
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
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (i == 0)
                UILayer.Instance.p1WinsText.text = $"P{i + 1} Wins: " + (int)PhotonNetwork.PlayerList[i].CustomProperties[CustomKeys.WINS];
            else
                UILayer.Instance.p2WinsText.text = $"P{i + 1} Wins: " + (int)PhotonNetwork.PlayerList[i].CustomProperties[CustomKeys.WINS];
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