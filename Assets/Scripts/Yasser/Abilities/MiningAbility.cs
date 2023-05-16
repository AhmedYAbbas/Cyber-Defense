using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiningAbility : MonoBehaviour, IAbility
{
    private Button _card;
    private const int COST = 5;
    private float _timeStamp;
    private float _nextUseTime;
    private const float COOLDOWN_TIME = 10.0f;
    [SerializeField] private TextMeshProUGUI cooldownText;
    private float _timer = COOLDOWN_TIME;
    private bool _isOnCooldown;

    private void Awake()
    {
        _card = GetComponent<Button>();
    }

    private void Update()
    {
        Debug.Log(_timer);

        if (_isOnCooldown)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _timer = COOLDOWN_TIME;
        }

        DisplayTime(_timer);

        if (EnergyManager.Instance._energy < COST || _nextUseTime > Time.time)
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

        
    }

    public void Use()
    {
        // TODO: Make the ability usable after a time period (30 secs) and also make it usable only one time per (Game/Round)?!
        if (Time.time > _nextUseTime)
        {
            EnergyManager.Instance.DecreaseEnergyEvent(COST);
            int defenderEnergy = (int)PhotonNetwork.PlayerListOthers[0].CustomProperties[CustomKeys.ENERGY];
            int energyToAdd = (int)(defenderEnergy * 0.2);
            EnergyManager.Instance.DecreaseEnergyEvent(-energyToAdd);


            _nextUseTime = Time.time + COOLDOWN_TIME;
        }
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
}
