using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTowerButton : MonoBehaviour
{
    private GridBuildingSystem3D GridBuildingSystem;
    private Button myButton;
    [SerializeField] TowerModifications towerBase;
    int towerEnergy;
    void Start()
    {
        myButton= GetComponent<Button>();
        GameObject GridSystem = GameObject.Find("GridBuildingSystem3D");
        
        // Check if the game object was found
        if (GridSystem != null)
        {
            //GridBuildingSystem = GridSystem.GetComponent<GridBuildingSystem3D>();
            // Do something with the game object
            Debug.Log("Found the game object!");
        }
        else
        {
            // Game object not found
            Debug.LogError("Unable to find the game object!");
        }

        myButton.onClick.AddListener(OnClick);
       
        towerEnergy = towerBase.EnergyCost;
    }

    public void OnClick()
    {
        GridBuildingSystem3D.Instance.SetPlacedObjectTypeSO(0);
    }

    private void Update()
    {
        print(EnergyManager.Instance._energy);
        //print(towerBase.EnergyCost);

        if(EnergyManager.Instance._energy >= towerEnergy)
        {
            myButton.interactable= true;
        }
        else
        {
            myButton.interactable= false;
        }
    }
}
