using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GenericAbility : MonoBehaviour
{
    public Button _card;
    public int cost = 5;
    public float _nextUseTime;
    public const float COOLDOWN_TIME = 10.0f;
    public TextMeshProUGUI cooldownText;
    public float _timer = COOLDOWN_TIME;
    public bool _isOnCooldown;
    public string nameOfMalwareNeededToUse;
    public int numberOfMalwareNeededToUse;



    private PhotonView[] photonViews;

    public virtual void Update()
    {
        if (_isOnCooldown)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = COOLDOWN_TIME;
        }

        DisplayTime(_timer);
        if (EnergyManager.Instance._energy < cost || _nextUseTime > Time.time )
        {
            _card.interactable = false;
            cooldownText.gameObject.SetActive(true);
            _isOnCooldown = true;
        }
        else
        {
            _card.interactable = true;
            cooldownText.gameObject.SetActive(false);
            _isOnCooldown = false;
        }

        //if (CountActiveGameObjects() >= numberOfMalwareNeededToUse)
        //{
        //    if (EnergyManager.Instance._energy < cost || _nextUseTime > Time.time)
        //    {
        //        _card.interactable = false;
        //        cooldownText.gameObject.SetActive(true);
        //        _isOnCooldown = true;
        //    }
        //    else
        //    {
        //        _card.interactable = true;
        //        cooldownText.gameObject.SetActive(false);
        //        _isOnCooldown = false;
        //    }
        //}
        //else
        //{
        //    _card.interactable = false;
        //}



    }

    private void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        cooldownText.text = $"{seconds}";
    }




    //private int CountActiveGameObjects()
    //{
    //    int count = 0;

    //    // Get all active PhotonViews in the scene
    //    photonViews = FindObjectsOfType<PhotonView>();

    //    foreach (PhotonView view in photonViews)
    //    {
    //        // Check if the PhotonView is active and has a valid GameObject
    //        if (view != null && view.gameObject != null && view.gameObject.activeInHierarchy)
    //        {
    //            print(view.gameObject.name);
    //            // Check if the GameObject has the name "Virus"
    //            if (view.gameObject.name == nameOfMalwareNeededToUse)
    //            {
    //                count++;
    //            }
    //        }
    //    }
    //    print("the count of virus" + count);
    //    return count;
    //}
}
