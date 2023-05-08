using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class NetworkPlayer : MonoBehaviourPunCallbacks
{


    #region Public Variables

    public static NetworkPlayer Instance { get; private set; }

    #endregion

    #region Private Variables



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
            //DontDestroyOnLoad(this.gameObject);
        }
    }

    #endregion

    #region Pun Callbacks

    public override void OnJoinedRoom()
    {
       
    }

    #endregion

    #region Private Methods

    

    #endregion
}
