using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModificationCard : MonoBehaviour
{
    // Private
    [SerializeField] private TowerModifications _modificationCard;
    private Button _card;

    void Start()
    {
        // this.GetComponent<Image>().color = _malwareData.CardColor;
        this.GetComponentInChildren<TMP_Text>().text = _modificationCard.EnergyCost.ToString();
        _card = this.GetComponent<Button>();
    }

    void Update()
    {
        if (EnergyManager.Instance._energy < _modificationCard.EnergyCost)
        {
            _card.interactable = false;
        }
        else
        {
            _card.interactable = true;
        }
    }

    public void ChooseObjectToModify()
    {
        TowerModificationManager.Instance.TowerModifications = _modificationCard;
        TowerModificationManager.Instance.ObjectEnergyCost = _modificationCard.EnergyCost;
        print(_modificationCard.ModificationName);
    }
}
