using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedObject : MonoBehaviour
{
    private ScriptableObjectTower placedObjectTypeSO;
    private Vector2Int origin;
    private ScriptableObjectTower.Dir dir;

    public static PlacedObject Create(Vector3 worldPosition, Vector2Int origin, ScriptableObjectTower.Dir dir, ScriptableObjectTower placedObjectTypeSO)
    {
        Transform placedObjectTransform = Instantiate(placedObjectTypeSO.prefab, worldPosition, Quaternion.Euler(0, placedObjectTypeSO.GetRotationAngle(dir), 0));

        PlacedObject placedObject = placedObjectTransform.GetComponent<PlacedObject>();

        placedObject.placedObjectTypeSO = placedObjectTypeSO;
        placedObject.origin = origin;
        placedObject.dir = dir;

        return placedObject;
    }

    public List<Vector2Int> GetGridPositionList()
    {
        return placedObjectTypeSO.GetGridPositionlist(origin, dir);
    }

    public void DestroySelf()
    {
        Destroy(gameObject);
    }
}
