using System.Collections;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviourPunCallbacks
{
    [SerializeField] private TextMeshProUGUI timerText;
    private bool _canRaiseEvent;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            StartCoroutine(SyncTimerCoroutine());
        }
    }

    private void Update()
    {
        if (MatchManager.Instance.roundTime > 0)
        {
            MatchManager.Instance.roundTime -= Time.deltaTime;
            _canRaiseEvent = true;
        }
        else
        {
            if (_canRaiseEvent)
            {
                _canRaiseEvent = false;
                if (PhotonNetwork.IsMasterClient)
                {
                    MatchManager.Instance.EndRound(false);
                }
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            DisplayTime(MatchManager.Instance.roundTime);
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay <= 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    [PunRPC]
    private void SyncTimer(float time)
    {
        DisplayTime(time);
    }

    private IEnumerator SyncTimerCoroutine()
    {
        while (true)
        {
            photonView.RPC("SyncTimer", RpcTarget.Others, MatchManager.Instance.roundTime);
            yield return new WaitForSeconds(1.0f);
        }
    }
}
