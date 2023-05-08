using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour
{

    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    private void Awake()
    {
        Instance = this;
        mouseColliderLayerMask = LayerMask.GetMask("Buildable");
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool found = false;
        /*RaycastHit[] hits = Physics.RaycastAll(ray, 999f, mouseColliderLayerMask);
        RaycastHit hit = new RaycastHit();
        for (int i = 0; i < hits.Length; i++)
        {
            Debug.Log("Hit object: " + hits[i].collider.gameObject.name);
            if (!hits[i].transform.CompareTag("Buildable"))
            {
                continue;
            }
            //if (hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Default"))
            //{
            //    print("HIT DEFAULT LAYER");
            //    continue;
            //}
            hit = hits[i];
            found = true;
            break;
        }*/

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.name);
            if (hit.transform.CompareTag("Buildable"))
            {
                found = true;
            }
        }


        if (found)
        {
            transform.position = hit.point;
        }
    }

    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit[] hits = Physics.RaycastAll(ray, 999f, mouseColliderLayerMask);
        //RaycastHit hit = new RaycastHit();
        bool found = false;
        /*for (int i = 0; i < hits.Length; i++)
        {
            print(hits[i].collider.gameObject.name);
            if (!(hits[i].collider.gameObject.layer == LayerMask.NameToLayer("Buildable")))
            {
                continue;
            }
            hit = hits[i];
            found = true;
            break;
        }*/

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.tag);

            if (hit.transform.CompareTag("Buildable"))
            {
                found = true;
            }
        }


        if (found)
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }



    public static bool CANBUILD()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool found = false;
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.tag);

            if (hit.transform.CompareTag("Buildable"))
            {
                found = true;
            }
        }
        return found;
    }
}
