using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShowCardData : MonoBehaviour
{
    // Private Variables
    [SerializeField] private MalwareData _malwareData;
    [SerializeField] private GameObject _dataCard;
    [SerializeField] private List<Button> _cards;
    [SerializeField] private TMP_Text _malwareName;
    [SerializeField] private TMP_Text _attackDamage;
    [SerializeField] private TMP_Text _movementSpeed;
    [SerializeField] private TMP_Text _health;
    [SerializeField] private TMP_Text _fireRate;
    [SerializeField] private TMP_Text _energyCost;

    public void ShowData()
    {
        for (int i = 0; i < _cards.Count; i++)
        {
            _cards[i].transform.GetChild(1).gameObject.SetActive(false);
        }

        _malwareName.text = $"Name: {_malwareData.MalwareName}";
        _attackDamage.text = $"Damage: {_malwareData.AttackDamage}";
        _movementSpeed.text = $"Mov Speed: {_malwareData.MovementSpeed}";
        _health.text = $"Health: {_malwareData.Health}";
        _fireRate.text = $"Fire Rate: {_malwareData.FireRate}";
        _energyCost.text = $"Cost: {_malwareData.EnergyCost}";
        _dataCard.SetActive(true);
    }
}
