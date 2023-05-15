using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpawnCard : MonoBehaviour
{
    [SerializeField] private MalwareData _malwareData;

    void Start()
    {
        this.GetComponent<Image>().color = _malwareData.CardColor;
        this.GetComponentInChildren<TMP_Text>().text = _malwareData.EnergyCost.ToString();
    }

    public void ChooseObjectToSpawn()
    {
        SpawnManager.Instance.ObjectName = _malwareData.MalwareName;
        SpawnManager.Instance.ObjectEnergyCost = _malwareData.EnergyCost;
        print(SpawnManager.Instance.ObjectName);
    }


}
