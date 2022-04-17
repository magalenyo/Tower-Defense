using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    public static GridBuildingSystem Instance { get; private set; }

    [SerializeField] private List<ScriptableObjectTower> placedObjectTypeSOList;
    private ScriptableObjectTower placedObjectTypeSO;

    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = 10f;

    private GridXZ<GridObject> grid;
    private ScriptableObjectTower.Dir dir = ScriptableObjectTower.Dir.DOWN;

    public event EventHandler OnSelectedChanged;

    private void Awake()
    {
        Instance = this;

        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        grid.SetDebug(true);
        placedObjectTypeSO = placedObjectTypeSOList[0];
    }

    class GridObject
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;
        private PlacedObject placedObject;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }
        
        public void SetPlacedObject(PlacedObject placedObject)
        {
            this.placedObject = placedObject;
            grid.TriggerObjectChanged(x, z);
        }

        public PlacedObject GetPlacedObject()
        {
            return placedObject;
        }

        public void ClearPlacedObject()
        {
            placedObject = null;
            grid.TriggerObjectChanged(x, z);
        }

        public bool CanBuild()
        {
            return placedObject == null;
        }

        public override string ToString()
        {
            return x + ", " + z + "\n" + placedObject;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);
            
            List<Vector2Int> gridPositionList = placedObjectTypeSO.GetGridPositionlist(new Vector2Int(x, z), dir);

            bool canBuild = true;
            foreach(Vector2Int gridPosition in gridPositionList)
            {
                if(!grid.GetGridObject(gridPosition.x, gridPosition.y).CanBuild())
                {
                    canBuild = false;
                    break;
                }
            }

            if (canBuild)
            {
                Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
                Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();

                PlacedObject placedObject = PlacedObject.Create(placedObjectWorldPosition, new Vector2Int(x, z), dir, placedObjectTypeSO);

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(placedObject);
                }
            }
            else
            {
                Debug.Log("Can't build");
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            GridObject gridObject = grid.GetGridObject(Mouse3D.GetMouseWorldPosition());
            PlacedObject placedObject = gridObject.GetPlacedObject();
            if (placedObject != null)
            {
                placedObject.DestroySelf();

                List<Vector2Int> gridPositionList = placedObject.GetGridPositionList();

                foreach (Vector2Int gridPosition in gridPositionList)
                {
                    grid.GetGridObject(gridPosition.x, gridPosition.y).ClearPlacedObject();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            dir = ScriptableObjectTower.GetNextDir(dir);
            Debug.Log(dir);
        }
    }

    public ScriptableObjectTower GetScriptableObjectTower()
    {
        return placedObjectTypeSO;
    }

    public Vector3 GetMouseWorldSnappedPosition()
    {
        Vector3 mousePosition = Mouse3D.GetMouseWorldPosition();
        grid.GetXZ(mousePosition, out int x, out int z);

        if (placedObjectTypeSO != null)
        {
            Vector2Int rotationOffset = placedObjectTypeSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, 0, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        }
        else
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation()
    {
        if (placedObjectTypeSO != null)
        {
            return Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0);
        }
        else
        {
            return Quaternion.identity;
        }
    }
}
