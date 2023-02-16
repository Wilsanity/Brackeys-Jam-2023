using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //North = 0
    //South = 1
    //East = 2
    //West = 3

    [Tooltip("Destroy Room Upon Exiting")]
    public bool AutoDecontruct = false;

    public GameObject[] MapChunkPrefabs;

    [SerializeField] private List<MapChunk> currentMapChunks;

    private void Start()
    {
        StartMap();
    }

    public void ResetMap()
    {
        foreach(var mapChunk in currentMapChunks)
        {
            Destroy(mapChunk);
        }

        currentMapChunks.Clear();
    }

    public void StartMap()
    {
        ResetMap();

        MapChunk mapChunk = Instantiate(MapChunkPrefabs[0]).GetComponent<MapChunk>();

        mapChunk.transform.position = Vector3.zero;
        currentMapChunks.Add(mapChunk);
    }
    
    public void AddTilePiece(MapChunk from, MapConnection.Direction direction)
    {
        int random = Random.Range(0, MapChunkPrefabs.Length);

        MapChunk mapChunk = Instantiate(MapChunkPrefabs[random]).GetComponent<MapChunk>();

        //mapChunk.transform.position = from.GetEntrancePosition(direction).position;
        currentMapChunks.Add(mapChunk);

        MapConnection.Direction desiredDirection = 0;

        switch(direction)
        {
            case MapConnection.Direction.TOP:
                desiredDirection = MapConnection.Direction.BOTTOM;
                break;
            case MapConnection.Direction.RIGHT:
                desiredDirection = MapConnection.Direction.LEFT;
                break;
            case MapConnection.Direction.BOTTOM:
                desiredDirection = MapConnection.Direction.TOP;
                break;
            case MapConnection.Direction.LEFT:
                desiredDirection = MapConnection.Direction.RIGHT;
                break;
        }

        int temp = 0;
        while (!mapChunk.HasDirection(desiredDirection) && temp < 6)
        {
            mapChunk.Rotate();
            temp++;
        }

        Vector3 offset = mapChunk.transform.position - mapChunk.GetEntrancePosition(desiredDirection).position;
        mapChunk.transform.position = from.GetEntrancePosition(direction).position + offset;

        mapChunk.GetMapConnection(desiredDirection).SetHit();

        if(AutoDecontruct)
            StartCoroutine(DestroyMapChunk(from, mapChunk.GetMapConnection(desiredDirection)));
    }

    public IEnumerator DestroyMapChunk(MapChunk mapChunk, MapConnection connectionToReset)
    {
        yield return new WaitForEndOfFrame();

        currentMapChunks.Remove(mapChunk);
        mapChunk.OnDestroy();

        yield return new WaitForSeconds(1f);

        connectionToReset.SetHit(false);
    }

}
