using System;
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
    [HideInInspector] public AudioClip ObjectSpawnSFX;
    public Camera camera;

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
        Ray ray = camera.ScreenPointToRay(position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if ((MatchManager.Side)PhotonNetwork.LocalPlayer.CustomProperties[CustomKeys.P_SIDE] == MatchManager.Side.Attacker)
            {
                GameManager.Instance.TowerManager.SetActive(false);
                if (hit.transform.CompareTag("Road") && !String.IsNullOrEmpty(ObjectName))
                {
                    Vector3 spawnPosition = hit.point;
                    spawnPosition.y += .3f;
                    //Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
                    if (ObjectEnergyCost <= EnergyManager.Instance._energy)
                    {
                        MalwaresManager.Instance.SpawnMalware(ObjectName, spawnPosition);
                        EnergyManager.Instance.DecreaseEnergy(ObjectEnergyCost);
                        if (ObjectSpawnSFX != null)
                        {
                            SoundManager.Instance.PlaySoundEffect(ObjectSpawnSFX , 0.1f);
                        }
                    }
                }
            }
            else
            {
                GameManager.Instance.TowerManager.SetActive(true);
            }
        }
    }
}