using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UILayer : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject attackerUI;
    [SerializeField] private GameObject defenderUI;
    [SerializeField] private GameObject switchingSidesPanel;
    [SerializeField] private TMP_Text roundNumText;
    [SerializeField] private TMP_Text attackerDefenderTurnText;
    [SerializeField] private GameObject ads;
    [SerializeField] private GameObject matchDisconnetedPanel;

    #endregion

    #region Public Variables

    public TextMeshProUGUI roundText;
    public TextMeshProUGUI p1WinsText;
    public TextMeshProUGUI p1NameText;
    public TextMeshProUGUI p2WinsText;
    public TextMeshProUGUI p2NameText;
    public TMP_Text winnerText;
    public GameObject GameEndedPanel;

    public static UILayer Instance { get; private set; }

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
            DontDestroyOnLoad(gameObject);
        }

        StartCoroutine(EnableSwitchingSidesPanel(0));
    }

    #endregion

    #region Public Methods

    public void LoadPlayerUI(MatchManager.Side side)
    {
        if (attackerUI.activeInHierarchy || defenderUI.activeInHierarchy)
        {
            attackerUI.SetActive(false);
            defenderUI.SetActive(false);
        }

        switch (side)
        {
            case MatchManager.Side.Attacker:
                defenderUI.SetActive(false);
                attackerUI.SetActive(true);
                break;
            case MatchManager.Side.Defender:
                attackerUI.SetActive(false);
                defenderUI.SetActive(true);
                break;
        }
    }

    public IEnumerator EnableSwitchingSidesPanel(int increment)
    {
        roundNumText.text = $"Round: {MatchManager.Instance.currentRound + increment}";
        if ((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
        {
            attackerDefenderTurnText.text = "Now You Are An <color=red>Attacker</color>";
        }
        else
        {
            attackerDefenderTurnText.text = "Now You Are A <color=blue>Defender</color>";
        }


        switchingSidesPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        switchingSidesPanel.gameObject.SetActive(false);
    }

    public void ReturnToMainMenu()
    {
        PhotonNetwork.LoadLevel(0);
        Destroy(gameObject, 2.0f);
    }

    public void ShowAds()
    {
        foreach (Transform AD in ads.transform)
        {
            AD.gameObject.SetActive(true);
        }
        ads.SetActive(true);
    }

    public void EnableDisconnectionPanel()
    {
        if (SceneManager.GetActiveScene().name == MatchManager.Instance.GAMEPLAY_SCENE_NAME)
        {
            StartCoroutine(EnableDisconnectedPanel());
        }
    }

    #endregion

    #region Private Methods

    private IEnumerator EnableDisconnectedPanel()
    {
        matchDisconnetedPanel.SetActive(true);
        SceneManager.LoadScene(MatchManager.Instance.MAIN_MENU_SCENE_NAME);
        yield return new WaitForSeconds(3.0f);
        Destroy(gameObject, 1.0f);
    }

    #endregion
}
