using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using static MatchManager;

public class EnergyManager : MonoBehaviourPunCallbacks
{
    // Private
    private static EnergyManager _instance;
    [SerializeField] public int _AttackerMaxEnergy;
    [SerializeField] public int _DefenderMaxEnergy;
    [SerializeField] public Slider _energySlider;
    [SerializeField] private int _timePeriodToIncreaseEnergy;
    [SerializeField] private int _AttackerEnergyIncreaseValue;
    [SerializeField] private int _DefenderEnergyIncreaseValue;
    private int _roundTime;

    // Public 
    public static EnergyManager Instance => _instance;
    [HideInInspector] public int _energy;
    [HideInInspector] public int _energyIncreaseValue;
    [HideInInspector] public int _maxEnergy;

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
        if ((Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == Side.Defender)
        {
            _maxEnergy = _DefenderMaxEnergy;
            _energyIncreaseValue = _DefenderEnergyIncreaseValue;
        }
        else
        {
            _maxEnergy = _AttackerMaxEnergy;
            _energyIncreaseValue = _AttackerEnergyIncreaseValue;
        }
        InitializeRoundVariables();
    }


    void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }

        _energySlider.maxValue = _maxEnergy;
        _energySlider.value = _maxEnergy;

        InvokeRepeating("IncreaseEnergyPerTime", 0, _timePeriodToIncreaseEnergy);
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
        else if (_energy > _maxEnergy)
            _energy = _maxEnergy;

        _energySlider.value = _energy;
        PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY] = _energy;
    }

    public void DecreaseDefenderEnergy(int amount)
    {
        _energy -= amount;

        if (_energy < 0)
            _energy = 0;
        else if (_energy > _maxEnergy)
            _energy = _maxEnergy;

        _roundTime = 0;
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

    public void ModifyEnergy(EventData obj)
    {
        if (obj.Code == MatchManager.MiningUsedEventCode)
        {
            int defenderEnergy = (int)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.ENERGY];
            int energyToDecrease = (int)(defenderEnergy * 0.2);
            DecreaseDefenderEnergy(energyToDecrease);
        }
    }

    void IncreaseEnergyPerTime()
    {
        if (_energy < _maxEnergy)
        {
            _energy += _energyIncreaseValue;
            _energySlider.value = _energy;
        }
    }
}
