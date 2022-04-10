using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuildingSystem : MonoBehaviour
{
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

        private const int MIN = 0;
        private const int MAX = 100;
        public int value;

        public GridObject(GridXZ<GridObject> grid, int x, int z)
        {
            this.grid = grid;
            this.x = x;
            this.z = z;
        }

        public void AddValue(int addValue)
        {

            value += addValue;
            value = Mathf.Clamp(value, MIN, MAX);

            grid.TriggerObjectChanged(x, z);
        }

        public float GetValueNormalized()
        {
            return (float)value / MAX;
        }

        public override string ToString()
        {
            return value.ToString();
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
            GridObject go = grid.GetGridObject(GridUtils.GetMouseWorldPosition());
            if (go != null)
            {
                go.AddValue(5);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log(grid.GetGridObject(GridUtils.GetMouseWorldPosition()));
        }
    }
}
