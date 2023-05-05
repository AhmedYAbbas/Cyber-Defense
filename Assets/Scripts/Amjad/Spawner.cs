using Photon.Pun;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    //public GameObject characterPrefab;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SpawnObject(Input.mousePosition);
        }
        else if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
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
            if (hit.transform.CompareTag("Road"))
            {
                Vector3 spawnPosition = hit.point;
                spawnPosition.y += .3f;
                //Instantiate(characterPrefab, spawnPosition, Quaternion.identity);
                PhotonNetwork.Instantiate("Sphere", spawnPosition , Quaternion.identity);
            }
        }
    }
}