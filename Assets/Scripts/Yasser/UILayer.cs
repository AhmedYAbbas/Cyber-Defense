using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class UILayer : MonoBehaviour
{
    #region Serialized Fields

    [SerializeField] private GameObject attackerUI;
    [SerializeField] private GameObject defenderUI;
    [SerializeField] private GameObject switchingSidesPanel;
    [SerializeField] private TMP_Text roundNumText;
    [SerializeField] private TMP_Text attackerDefenderTurnText;
    [SerializeField] private GameObject ads;

    #endregion

    #region Public Variables

    public GameObject timerGameObject;
    public TextMeshProUGUI roundText;
    public TextMeshProUGUI p1WinsText;
    public TextMeshProUGUI p2WinsText;
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
                attackerUI.SetActive(true);
                break;
            case MatchManager.Side.Defender:
                defenderUI.SetActive(true);
                break;
        }
    }

    public IEnumerator EnableSwitchingSidesPanel()
    {
        roundNumText.text = $"Round: {MatchManager.Instance.currentRound+1}";
        if (defenderUI.activeInHierarchy)
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
        ads.SetActive(true);
    }

    #endregion
}