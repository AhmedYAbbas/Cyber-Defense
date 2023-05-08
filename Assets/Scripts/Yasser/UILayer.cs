using UnityEngine;

public class UILayer : MonoBehaviour
{
    public GameObject timerGameObject;

    #region Serialized Fields

    [SerializeField] private GameObject attackerUI;
    [SerializeField] private GameObject defenderUI;

    #endregion

    #region Public Variables

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
            DontDestroyOnLoad(this);
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

    public void SwitchPlayerUI(MatchManager.Side side)
    {
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

    public void DisablePlayerUI(MatchManager.Side side)
    {
        switch (side)
        {
            case MatchManager.Side.Attacker:
                attackerUI.SetActive(false);
                break;
            case MatchManager.Side.Defender:
                defenderUI.SetActive(false);
                break;
        }
    }

    #endregion
}
