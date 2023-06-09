using System;
using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // private variables
    [SerializeField] private TMP_InputField _ifPlayerNickName;
    [SerializeField] private Button _btnLetsGo;
    [SerializeField] private GameObject _welcomePanel;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _chooseModePanel;
    [SerializeField] private GameObject _waitingForPlayerPanel;
    [SerializeField] private GameObject _rejoinPanel;

    void Start()
    {
        if (PlayerPrefs.GetString("NickName") != String.Empty)
        {
            _welcomePanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
        }
    }

    public void CheckIfPlayerEnteredHisName()
    {
        if (_ifPlayerNickName.text.Length > 0)
        {
            _btnLetsGo.interactable = true;
        }
        else
        {
            _btnLetsGo.interactable = false;
        }
    }

    public void SubmitNickName()
    {
        _welcomePanel.SetActive(false);
        PlayerPrefs.SetString("NickName", _ifPlayerNickName.text);
    }

    public void CreateOrJoinRoom(TMP_InputField RoomName)
    {
        if (!string.IsNullOrEmpty(RoomName.text) && PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            NetworkingManager.Instance.CreateOrJoinRoom(RoomName.text);
            _waitingForPlayerPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(RejoinCoroutine());
        }
    }

    public void JoinRandomRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            NetworkingManager.Instance.QuickMatch();
            _waitingForPlayerPanel.SetActive(true);
        }
        else
        {
            StartCoroutine(RejoinCoroutine());
        }
    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator RejoinCoroutine()
    {
        _rejoinPanel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        _rejoinPanel.SetActive(false);
    }
}