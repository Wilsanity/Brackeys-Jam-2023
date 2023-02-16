using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapConnection : MonoBehaviour
{
    public enum Direction : int
    {
        TOP = 0,
        RIGHT = 1,
        BOTTOM = 2,
        LEFT = 3,
    };

    public Direction direction;
    private MapChunk parent;

    [SerializeField] bool hit = false;

    public void Rotate(int rotations)
    {
        int newDirection = (int)(direction + rotations) % 4;

        switch (newDirection)
        {
            case 0:
                direction = Direction.TOP;
                break;
            case 1:
                direction = Direction.RIGHT;
                break;
            case 2:
                direction = Direction.BOTTOM;
                break;
            case 3:
                direction = Direction.LEFT;
                break;
        }
    }

    public void AssignParent(MapChunk _parent)
    {
        parent = _parent;
    }

    public void SetHit()
    {
        hit = true;
    }

    public void SetHit(bool _hit)
    {
        hit = _hit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hit)   
            return;

        if (other.tag.Equals("Player"))
        {
            FindObjectOfType<MapGenerator>().AddTilePiece(parent, direction);
            hit = true;
        }
    }
}
