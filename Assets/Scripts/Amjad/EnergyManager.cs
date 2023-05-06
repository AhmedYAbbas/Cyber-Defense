using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
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
    [HideInInspector] public int _energy;


    void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }

        _energySlider.maxValue = _maxEnergy;
        InitializeRoundVariables();
        InvokeRepeating("IncreaseEnergyPerTime", 0, _timePeriodToIncreaseEnergy);
        //_energySlider.value = 0;
    }

    void Update()
    {
        // needs to be fixed :(
        //_roundTime += (int)Time.time;
        //print(_roundTime + " " + Time.deltaTime);
        //IncreaseEnergyPerTime(_roundTime, _timePeriodToIncreaseEnergy, _energyIncreaseValue);
    }


    public void InitializeRoundVariables()
    {
        _roundTime = 0;
        _energy = _maxEnergy;
        _energySlider.value = _energy;
    }

    public void DecreaseEnergy(int Amount)
    {
        _energy -= Amount;
        _energySlider.value = _energy;
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


}
