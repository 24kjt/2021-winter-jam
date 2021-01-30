using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelController : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    GameObject[] goals;
    GameObject[] pits;
    GameObject[] walls;

    GameObject[,] stage = new GameObject[9,9];

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        goals = GameObject.FindGameObjectsWithTag("Goal");
        pits = GameObject.FindGameObjectsWithTag("Pit");
        walls = GameObject.FindGameObjectsWithTag("Wall");

        //set player
        Vector2Int playerIndex = calculateLevelIndexes(player.transform.position);
        stage[playerIndex.x, playerIndex.y] = player;
        player.transform.position = new Vector3(playerIndex.x * (float)1.2 + 0.6f, playerIndex.y + 0.5f, player.transform.position.z);
        Debug.Log("Player Position: " + (int)(player.transform.position.x / 1.2) + ", " + (int)(player.transform.position.y));
        foreach ( GameObject go in goals) {
            Vector2Int goIndex = calculateLevelIndexes(go.transform.position);
            stage[goIndex.x, goIndex.y] = go;
            go.transform.position = new Vector3(goIndex.x * (float)1.2 + 0.6f, goIndex.y + 0.5f, go.transform.position.z);
        }

        foreach ( GameObject go in pits) {
            Vector2Int goIndex = calculateLevelIndexes(go.transform.position);
            stage[goIndex.x, goIndex.y] = go;
            go.transform.position = new Vector3(goIndex.x * (float)1.2 + 0.6f, goIndex.y + 0.5f, go.transform.position.z);
        }

        foreach ( GameObject go in walls) {
            Vector2Int goIndex = calculateLevelIndexes(go.transform.position);
            stage[goIndex.x, goIndex.y] = go;
            go.transform.position = new Vector3(goIndex.x * (float)1.2 + 0.6f, goIndex.y + 0.5f, go.transform.position.z);
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
        return new Vector2Int((int)(v3.x/1.2), (int)(v3.y));
    }

    public GameObject getTile(int x, int y){
        return stage?[x,y];
    }
}
