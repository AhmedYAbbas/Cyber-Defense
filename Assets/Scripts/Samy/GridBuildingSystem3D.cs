using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using CodeMonkey.Utils;
using Photon.Pun;
using UnityEngine.EventSystems;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class GridBuildingSystem3D : MonoBehaviour
{
    public static GridBuildingSystem3D Instance { get; private set; }

    public event EventHandler OnSelectedChanged;
    public event EventHandler OnObjectPlaced;

    public static Camera MyCamera;
    private GridXZ<GridObject> grid;
    [SerializeField] private List<PlacedObjectTypeSO> placedObjectTypeSOList = null;
    private PlacedObjectTypeSO placedObjectTypeSO;
    private PlacedObjectTypeSO.Dir dir;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;
    [SerializeField] private float cellSize;
    [SerializeField] TowerModifications towerBase;

    int counter =0;
    private void Awake() {
        Instance = this;
        MyCamera = Camera.main;
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, new Vector3(transform.position.x, transform.position.y, transform.position.z), (GridXZ<GridObject> g, int x, int y) => new GridObject(g, x, y));

        placedObjectTypeSO = null;
    }

    public class GridObject {
        
        private GridXZ<GridObject> grid;
        private int x;
        private int y;
        public PlacedObject_Done placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int y) {
            this.grid = grid;
            this.x = x;
            this.y = y;
            placedObject = null;
        }

        public override string ToString() {
            return x + ", " + y + "\n" + placedObject;
        }

        public void SetPlacedObject(PlacedObject_Done placedObject) {
            this.placedObject = placedObject;
            grid.TriggerGridObjectChanged(x, y);
        }

        public void ClearPlacedObject() {
            placedObject = null;
            grid.TriggerGridObjectChanged(x, y);
        }

        public PlacedObject_Done GetPlacedObject() {
            return placedObject;
        }

        public bool CanBuild() {

            return placedObject == null;
        }

    }

    private void Update() {

        InputHandler();
    }
    private void InputHandler()
    {

        if (placedObjectTypeSO != null && EnergyManager.Instance._energy >= towerBase.EnergyCost && !EventSystem.current.IsPointerOverGameObject())
        {
            Vector3 mousePosition;
            if (Input.GetMouseButtonUp(0)||TouchInputManager.Instance.GetTouchPhase() == TouchPhase.Ended)
            {
                if (TouchInputManager.Instance.HasTouchInput())
                {
                    mousePosition = TouchInputManager.Instance.GetTouchWorldPosition();
                    if(counter==0)
                    {
                        counter++;
                        return;
                    }
                }
                else
                {
                    mousePosition = Mouse3D.Instance.GetMouseWorldPosition();
                }

                grid.GetXZ(mousePosition, out int x, out int z);
                Vector2Int placedObjectOrigin = new Vector2Int(x, z);
                placedObjectOrigin = grid.ValidateGridPosition(placedObjectOrigin);

                // Test Can Build
                List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionList(placedObjectOrigin, dir);
                bool canBuild = true;
                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    if (!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                    {
                        canBuild = false;
                        break;
                    }
                }
                bool RayCastCheck ;
                
                if (TouchInputManager.Instance.HasTouchInput())
                {
                    RayCastCheck = TouchInputManager.Instance.CANBUILD();
                }
                else
                {
                    RayCastCheck = Mouse3D.Instance.CANBUILD();
                }
                if (canBuild && RayCastCheck)
                {
                    Vector2Int rotationOffset = Vector2Int.zero;
                    Vector3 placedObjectWorldPosition = grid.GetWorldPosition(placedObjectOrigin.x, placedObjectOrigin.y) +new Vector3(rotationOffset.x, 0, rotationOffset.y)  * grid.GetCellSize()/2;

                    PlacedObject_Done placedObject = PlacedObject_Done.Create(placedObjectWorldPosition, placedObjectOrigin, dir, placedObjectTypeSO);

                    foreach (Vector2Int gridPosition in gridPositionList)
                    {
                        grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                    }

                    OnObjectPlaced?.Invoke(this, EventArgs.Empty);
                    EnergyManager.Instance.DecreaseEnergy(placedObjectTypeSO.energyReq);
                    DeselectObjectType();
                    counter= 0;
                }
                else
                {
                    // Cannot build here
                    UtilsClass.CreateWorldTextPopup("Cannot Build Here!", new Vector3(mousePosition.x-12, mousePosition.y, mousePosition.z));
                    //DeselectObjectType();
                }
            }
        }

            RefreshSelectedObjectType();  

    }

    private void DeselectObjectType() {
        placedObjectTypeSO = null; 
        RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType() {
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXZ(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition;
        if (TouchInputManager.Instance.HasTouchInput())
        {
          mousePosition = TouchInputManager.Instance.GetTouchWorldPosition();
        }
        else
        {
            mousePosition =  Mouse3D.Instance.GetMouseWorldPosition();
        }
        grid.GetXZ(mousePosition, out int x, out int z);
        if (placedObjectTypeSO != null) {
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z)  + Vector3.zero * grid.GetCellSize()/2;
            return placedObjectWorldPosition;
        } else {
            return mousePosition;
        }
    }

    //public Quaternion GetPlacedObjectRotation() {
    //    if (placedObjectTypeSO != null) {
    //        return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
    //    } else {
    //        return Quaternion.identity;
    //    }
    //}

    public PlacedObjectTypeSO GetPlacedObjectTypeSO() {
        return placedObjectTypeSO;
    }
    public void SetPlacedObjectTypeSO(int TypeNum)
    {
        placedObjectTypeSO = placedObjectTypeSOList[TypeNum];
        RefreshSelectedObjectType();
    }
}
