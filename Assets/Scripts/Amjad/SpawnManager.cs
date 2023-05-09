using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnManager : MonoBehaviour
{
    // Private
    private static SpawnManager _instance;


    // Public
    public static SpawnManager Instance => _instance;
    [HideInInspector] public string ObjectName;
    [HideInInspector] public int ObjectEnergyCost;

    void Start()
    {
        if (!_instance)
        {
            _instance = this;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnObject(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && !EventSystem.current.IsPointerOverGameObject())
        {
            SpawnObject(Input.GetTouch(0).position);
        }
    }

    void SpawnObject(Vector3 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.CompareTag("Road")&&(MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker )
            {
                Vector3 spawnPosition = hit.point;
                spawnPosition.y += .3f;
                //Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
                if (ObjectEnergyCost <= EnergyManager.Instance._energy)
                {
                    MalwaresManager.Instance.SpawnMalware(ObjectName, spawnPosition);
                    EnergyManager.Instance.DecreaseEnergy(ObjectEnergyCost);
                }
            }

        }
    }
}