using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlaceTowerButton : MonoBehaviour
{
    private GridBuildingSystem3D GridBuildingSystem;
    private Button myButton;

    void start()
    {
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
    }

    public void OnClick()
    {
        GridBuildingSystem3D.Instance.SetPlacedObjectTypeSO(0);
    }
}
