using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour {

    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
            transform.position = raycastHit.point;
        }
    }

    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        bool found = false;
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            print(hit.collider.tag + " v "+hit.collider.name);
            return hit.point;
            //if (hit.transform.CompareTag("Buildable"))
            //{
            //    found = true;
            //}
        }return Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //if (found)
        //{
        //    return hit.point;
        //}
        //else
        //{
        //    return Vector3.zero;
        //}
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