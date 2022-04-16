using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class ScriptableObjectTower : ScriptableObject
{
    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.DOWN: return Dir.LEFT;
            case Dir.LEFT: return Dir.UP;
            case Dir.UP: return Dir.RIGHT;
            case Dir.RIGHT: return Dir.DOWN;
        }
    }

    public enum Dir
    {
        DOWN,
        LEFT,
        UP,
        RIGHT
    }

    public string nameString;
    public Transform prefab;
    public Transform visual;
    public int width;
    public int height;
    
    public int GetRotationAngle(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.DOWN: return 0;
            case Dir.LEFT: return 90;
            case Dir.UP: return 180;
            case Dir.RIGHT: return 270;
        }
    }

    public Vector2Int GetRotationOffset(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.DOWN: return new Vector2Int(0, 0);
            case Dir.LEFT: return new Vector2Int(0, width);
            case Dir.UP: return new Vector2Int(width, height);
            case Dir.RIGHT: return new Vector2Int(height, 0);
        }
    }

    public List<Vector2Int> GetGridPositionlist(Vector2Int offset, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        switch (dir)
        {
            case Dir.DOWN:
            case Dir.UP:
                for(int x = 0; x < width; ++x)
                {
                    for (int y = 0; y <height; ++y)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
            case Dir.LEFT:
            case Dir.RIGHT:
                for (int x = 0; x < height; ++x)
                {
                    for (int y = 0; y < width; ++y)
                    {
                        gridPositionList.Add(offset + new Vector2Int(x, y));
                    }
                }
                break;
        }

        return gridPositionList;
    }
}
