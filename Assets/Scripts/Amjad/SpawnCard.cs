using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCard : MonoBehaviour
{
    // Private
    [SerializeField] private MalwareData _malwareData;
    private Button _card;
    void Start()
    {
        //this.GetComponent<Image>().color = _malwareData.CardColor;
        this.GetComponentInChildren<TMP_Text>().text = _malwareData.EnergyCost.ToString();
        _card = this.GetComponent<Button>();
    }

    void Update()
    {
        if (EnergyManager.Instance._energy < _malwareData.EnergyCost)
        {
            _card.interactable = false;
        }
        else
        {
            _card.interactable = true;
        }
    }

    public void ChooseObjectToSpawn()
    {
        SpawnManager.Instance.ObjectName = _malwareData.MalwareName;
        SpawnManager.Instance.ObjectEnergyCost = _malwareData.EnergyCost;
        SpawnManager.Instance.ObjectSpawnSFX = _malwareData.SpawnSFX;
    }


}
