using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour {

    public int columns = 12;
    public int rows = 8;

    public GameObject floorTile;
    public GameObject wallTile;

    private List<Vector3> gridPositions = new List<Vector3>();
    private Transform roomHolder;

    void InitializeList()
    {
        gridPositions.Clear();

        for ( int i = 1; i < columns - 1; i++)
        {
            for ( int j = 1; j < rows - 1; j++)
            {
                gridPositions.Add(new Vector3(i, j, 0f));
            }
        }
    }

    void roomSetup() {
        roomHolder = new GameObject("Room").transform;

        for (int i = -1; i < columns + 1; i++)
        {
            for (int j = -1; j < rows + 1; j++)
            {
                GameObject toInstantiate = floorTile;
                if (i == -1 || i == columns || j == -1 || j == rows)
                    toInstantiate = wallTile;

                GameObject instance = Instantiate(toInstantiate, new Vector3(i, j, 0f), Quaternion.identity) as GameObject;

                instance.transform.SetParent(roomHolder);
            }
        }
    }

    public void SetupScene()
    {
        roomSetup();
        InitializeList();

    }
}
