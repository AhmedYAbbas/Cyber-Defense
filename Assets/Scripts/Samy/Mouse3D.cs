using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouse3D : MonoBehaviour {

    public static Mouse3D Instance { get; private set; }
    public Camera MyCamera;
    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();
    [SerializeField] private Transform RaycastStart;
    private void Awake() {
        Instance = this;
        MyCamera = Camera.main;
    }

    private void Update() {
        Ray ray = MyCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask)) {
            transform.position = raycastHit.point;
        }
    }

    public  Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance() {
        Ray ray = MyCamera.ScreenPointToRay(Input.mousePosition);
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0));


        bool found = false;
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit, 999f, mouseColliderLayerMask))
        {

            //print(hit.collider.tag + " v " + hit.collider.name);
            return hit.point;

        }
        return hit.point;
    }

    public  bool CANBUILD()
    {

        Ray ray = MyCamera.ScreenPointToRay(Input.mousePosition);
        bool found = true;
        RaycastHit[] hits = Physics.SphereCastAll(ray, 0.5f, 999f, mouseColliderLayerMask);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.CompareTag("notBuildable")|| hit.transform.CompareTag("Road"))
            {
                return false;
            }
        }
        return found;
    }
}