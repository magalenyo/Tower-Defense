using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
    [SerializeField] private Transform testTransform;

    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float cellSize = 10f;

    //private GridXZ<GridObject> grid;
    private GridXZ<GridObject> grid;

    private void Awake()
    {
        grid = new GridXZ<GridObject>(gridWidth, gridHeight, cellSize, Vector3.zero, (GridXZ<GridObject> g, int x, int z) => new GridObject(g, x, z));
        //grid = new GridXZ<bool>(gridWidth, gridHeight, cellSize, Vector3.zero);
        grid.SetDebug(true);
    }

    class GridObject
    {
        private GridXZ<GridObject> grid;
        private int x;
        private int z;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return x + ", " + z;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            grid.GetXZ(Mouse3D.GetMouseWorldPosition(), out int x, out int z);
            Instantiate(testTransform, grid.GetWorldPosition(x, z), Quaternion.identity);

            //GridObject go = grid.GetGridObject(GridUtils.GetMouseWorldPosition());
            //if (go != null)
            //{
            //    go.AddValue(5);
            //}
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetGridObject(GridUtils.GetMouseWorldPosition()));
        }
    }
}
