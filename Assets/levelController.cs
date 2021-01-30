﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    Vector2Int playerIndex;
    GameObject[] goals;
    GameObject[] pits;
    GameObject[] walls;

    GameObject[,] stage = new GameObject[grid.gridWidth,grid.gridHeight];

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goals = GameObject.FindGameObjectsWithTag("Goal");
        pits = GameObject.FindGameObjectsWithTag("Pit");
        walls = GameObject.FindGameObjectsWithTag("Wall");

        //set player
        playerIndex = calculateLevelIndexes(player.transform.position);
        stage[playerIndex.x, playerIndex.y] = player;
        player.transform.position = new Vector3(playerIndex.x * grid.cellSizeX + grid.cellSizeX/2, playerIndex.y * grid.cellSizeY + grid.cellSizeY/2, player.transform.position.z);
        Debug.Log("Player Position: " + (int)(player.transform.position.x / grid.cellSizeX) + ", " + (int)(player.transform.position.y / grid.cellSizeY));
        
        foreach ( GameObject go in goals) {
            Vector2Int goIndex = calculateLevelIndexes(go.transform.position);
            stage[goIndex.x, goIndex.y] = go;
            go.transform.position = new Vector3(goIndex.x * grid.cellSizeX + grid.cellSizeX/2, goIndex.y * grid.cellSizeY + grid.cellSizeY/2, go.transform.position.z);
        }

        foreach ( GameObject go in pits) {
            Vector2Int goIndex = calculateLevelIndexes(go.transform.position);
            stage[goIndex.x, goIndex.y] = go;
            go.transform.position = new Vector3(goIndex.x * grid.cellSizeX + grid.cellSizeX/2, goIndex.y * grid.cellSizeY + grid.cellSizeY/2, go.transform.position.z);
        }

        foreach ( GameObject go in walls) {
            Vector2Int goIndex = calculateLevelIndexes(go.transform.position);
            stage[goIndex.x, goIndex.y] = go;
            go.transform.position = new Vector3(goIndex.x * grid.cellSizeX + grid.cellSizeX/2, goIndex.y * grid.cellSizeY + grid.cellSizeY/2, go.transform.position.z);
            Debug.Log("Wall Location: " + goIndex);
        }

        // for (int i = 0; i < 9; i++) {
        //     for (int j = 0; j < 9; j++) {
        //         Debug.Log("[" + i + "," + j + "] " + stage[i,j]);
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public Vector2Int calculateLevelIndexes(Vector3 v3){
        return new Vector2Int((int)(v3.x/grid.cellSizeX), (int)(v3.y/grid.cellSizeY));
    }

    public GameObject getTile(int x, int y){
        return stage?[x,y];
    }

    public void updatePlayerLocation(Vector3 v3){
        Vector2Int newIndex = calculateLevelIndexes(v3);
        stage[playerIndex.x, playerIndex.y] = null;
        stage[newIndex.x, newIndex.y] = player;
        playerIndex = newIndex;
    }
}
