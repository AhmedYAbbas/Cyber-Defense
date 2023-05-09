using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    #region Public Variables

    public GameManager Instance { get; private set; }
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

    private void Start()
    {
        UILayer.Instance.timerGameObject.SetActive(true);
        UILayer.Instance.LoadPlayerUI((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE]);
        //print(((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE]).ToString());
        if((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Defender)
        {
            TowerManager.gameObject.SetActive(true);
            //print("I AM A DEFENDER");
        }
        else
        {
            //print("I AM AN ATTACKER");
            TowerManager.gameObject.SetActive(false);
        }

    }
    #endregion
}
