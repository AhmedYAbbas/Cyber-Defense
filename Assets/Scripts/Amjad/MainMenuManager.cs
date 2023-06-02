using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // private variables
    [SerializeField] private TMP_InputField if_playerNickName;
    [SerializeField] private Button btn_LetsGo;
    [SerializeField] private GameObject welcomePanel;
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject chooseModePanel;
    [SerializeField] private GameObject waitingForPlayerPanel;
    [SerializeField] private GameObject searchingForMatchPanel;

    void Start()
    {
        if (PlayerPrefs.GetString("NickName") != String.Empty)
        {
            welcomePanel.SetActive(false);
            mainMenuPanel.SetActive(true);
        }
    }

    public void ChechIfPlayerEnteredHisName()
    {
        if (if_playerNickName.text.Length > 0)
        {
            btn_LetsGo.interactable = true;
        }
        else
        {
            btn_LetsGo.interactable = false;
        }
    }

    public void SubmitNickName()
    {
        welcomePanel.SetActive(false);
        PlayerPrefs.SetString("NickName", if_playerNickName.text);
    }

    public void CreateOrJoinRoom(TMP_InputField RoomName)
    {
        if (string.IsNullOrEmpty(RoomName.text))
        {
            return;
        }
        else
        {
            NetworkingManager.Instance.CreateOrJoinRoom(RoomName.text);
            waitingForPlayerPanel.SetActive(true);
            chooseModePanel.GetComponent<Canvas>().enabled = false;
        }
    }

    public void JoinRandomRoom()
    {
        NetworkingManager.Instance.QuickMatch();
        waitingForPlayerPanel.SetActive(true);
        chooseModePanel.GetComponent<Canvas>().enabled = false;

    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}