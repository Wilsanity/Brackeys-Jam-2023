using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapChunk : MonoBehaviour
{

    public MapConnection[] entrances;

    private void Awake()
    {
        foreach (var entrance in entrances)
            entrance.AssignParent(this);
    }

    public void Rotate()
    {
        transform.Rotate(0, 90, 0);

        foreach(var entrance in entrances)
        {
            entrance.Rotate(1);
        }
    }

    public bool HasDirection(MapConnection.Direction direction)
    {
        foreach (var entrance in entrances)
        {
            if (entrance.direction == direction)  
                return true;
        }

        return false;
    }

    public Transform GetEntrancePosition(MapConnection.Direction direction)
    {
        foreach (var entrance in entrances)
        {
            if (entrance.direction == direction)
                return entrance.transform;
        }

        return null;
    }

    public MapConnection GetMapConnection(MapConnection.Direction direction)
    {
        foreach (var entrance in entrances)
        {
            if (entrance.direction == direction)
                return entrance;
        }

        return null;
    }

    public void OnDestroy()
    {
        Destroy(gameObject);
    }

}
