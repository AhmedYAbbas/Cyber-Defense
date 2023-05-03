using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    // private variables
    [SerializeField] private TMP_InputField if_playerNickName;
    [SerializeField] private Button btn_LetsGo;
    [SerializeField] private GameObject welcomePanel;

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

    public void StartGame()
    {
        NetworkingManager.Instance.JoinOrCreateRoom();
    }

    public void Options()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }
}