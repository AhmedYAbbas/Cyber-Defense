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
    [SerializeField] private GameObject createOrJoinRandomPanel;
    [SerializeField] private GameObject waitingForPlayersPanel;

    void Start()
    {
        if (PlayerPrefs.GetString("NickName") != String.Empty)
        {
            welcomePanel.SetActive(false);
            print(PlayerPrefs.GetString("NickName"));
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

    public void CreateOrJoinRoom(TMP_Text RoomName)
    {
        if (string.IsNullOrEmpty(RoomName.text))
        {
            return;
        }
        else
        {
            NetworkingManager.Instance.CreateOrJoinRoom(RoomName.text);
            createOrJoinRandomPanel.SetActive(false);
            waitingForPlayersPanel.SetActive(true);
        }
    }

    public void JoinRandomRoom()
    {
        NetworkingManager.Instance.QuickMatch();
        createOrJoinRandomPanel.SetActive(false);
        waitingForPlayersPanel.SetActive(true);
    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}