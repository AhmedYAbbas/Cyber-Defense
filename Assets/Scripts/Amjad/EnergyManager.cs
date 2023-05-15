using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviourPunCallbacks 
{
    // Private
    private static EnergyManager _instance;
    [SerializeField] private int _maxEnergy;
    [SerializeField] private Slider _energySlider;
    [SerializeField] private int _timePeriodToIncreaseEnergy;
    [SerializeField] private int _energyIncreaseValue;
    private int _roundTime;

    // Public 
    public static EnergyManager Instance => _instance;
    public int _energy;


    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += ModifyEnergy;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= ModifyEnergy;
    }

    private void Awake()
    {
        InitializeRoundVariables();
    }

    void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }

        _energySlider.maxValue = _maxEnergy;
        InvokeRepeating("IncreaseEnergyPerTime", 0, _timePeriodToIncreaseEnergy);
        //_energySlider.value = 0;
    }

    public void InitializeRoundVariables()
    {
        _roundTime = 0;
        _energy = _maxEnergy;
        _energySlider.value = _energy;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            player.CustomProperties[CustomKeys.ENERGY] = _energy;
        }
    }

    public void DecreaseEnergy(int amount)
    {
        _energy -= amount;

        if (_energy < 0)
            _energy = 0;
        else if(_energy > _maxEnergy)
            _energy = _maxEnergy;

        _energySlider.value = _energy;

    }

    public void DecreaseDefenderEnergy(int amount)
    {
        _energy -= amount;

        if (_energy < 0)
            _energy = 0;
        else if (_energy > _maxEnergy)
            _energy = _maxEnergy;

        _energySlider.value = _energy;

        Hashtable energyProp = new Hashtable { [CustomKeys.ENERGY] = _energy };
        PhotonNetwork.LocalPlayer.SetCustomProperties(energyProp);
        PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY] = _energy;
    }

    public void DecreaseEnergyEvent(int amount)
    {
        _energy -= amount;

        if (_energy < 0)
            _energy = 0;
        else if (_energy > _maxEnergy)
            _energy = _maxEnergy;

        _energySlider.value = _energy;
        PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY] = _energy;

        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.Others };
        PhotonNetwork.RaiseEvent(MatchManager.MiningUsedEventCode, null, raiseEventOptions, SendOptions.SendReliable);
    }

    void IncreaseEnergyPerTime()
    {
        if (_energy < _maxEnergy)
        {
            _energy += _energyIncreaseValue;
        }
        else
        {
            _energy = _maxEnergy;
        }

        _energySlider.value = _energy;
    }

    public void ModifyEnergy(EventData obj)
    {
        if (obj.Code == MatchManager.MiningUsedEventCode)
        {
            int defenderEnergy = (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY];
            int energyToDecrease = (int)(defenderEnergy * 0.2);
            DecreaseDefenderEnergy(energyToDecrease);
            Debug.Log("Removed from defender");
        }
    }

}
