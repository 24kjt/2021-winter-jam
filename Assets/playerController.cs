using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public GameObject player;
    public GameObject levelController;
    private levelController levelScripts;

    public int tilesToMove = 1;
    public float moveSpeed = 5f;
    public bool hasHelment = false;

    bool isMoving = false;
    Vector2 input;

    Vector3 startPos; //Starting Position
    Vector3 endPos; //Movement destination
    float progress; //Progress to finishing movement
    MovementData res; //results of movement calc 

    // Start is called before the first frame update
    void Start()
    {
        levelScripts = levelController.gameObject.GetComponent<levelController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isMoving){
            input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            //Disable diagonal movement
            if (input.x != 0)
                input.y = 0;
            
            //check for valid input
            if (input != Vector2.zero) {
                startPos = player.transform.position;
                endPos = new Vector3(startPos.x + input.x * grid.cellSizeX * tilesToMove, 
                                     startPos.y + input.y * grid.cellSizeY * tilesToMove, 
                                     startPos.z);
                isMoving = true;
                res = calculateMovement(startPos, endPos);
                endPos = res.endPos;
                Debug.Log("EFFECT: " + res.playerEffect);
                progress = 0f;
            }
        } else {
            if (progress < 1f) {
                progress += Time.deltaTime * moveSpeed;

                player.transform.position = Vector3.Lerp(startPos, endPos, progress);
            } else {
                isMoving = false;
                Debug.Log("Moving done!");
                tilesToMove++;
                player.transform.position = endPos;
                levelScripts.updatePlayerLocation(endPos);
                performConsequences();
            }
        }
    }

    MovementData calculateMovement(Vector3 startPos, Vector3 goalPos) {
        Direction movementDirection = Direction.Right;
        int numIterations = 0;
        bool attemptedEscape = false;
        bool isHitWall = false;
        Vector2Int startIndex = levelScripts.calculateLevelIndexes(startPos);
        Vector2Int endIndex = levelScripts.calculateLevelIndexes(goalPos);

        MovementData ans;
        ans.endPos = goalPos;
        ans.playerEffect = Effect.None;

        //Check if attempting to exit stage
        Debug.Log("End Index: " + endIndex);
        if (endIndex.x > grid.gridWidth - 1 || endIndex.x < 0 || endIndex.y > grid.gridHeight - 1 || endIndex.y < 0) {
            attemptedEscape = true;
            //Adjust x
            endIndex.x = endIndex.x > grid.gridWidth - 1 ? grid.gridWidth - 1 : endIndex.x;
            endIndex.x = endIndex.x < 0 ? 0 : endIndex.x;
            //Adjust y
            endIndex.y = endIndex.y > grid.gridHeight - 1 ? grid.gridHeight - 1 : endIndex.y;
            endIndex.y = endIndex.y < 0 ? 0 : endIndex.y;
            Debug.Log("ATTEMPTED ESCAPE");
        }

        //Determine direction of movement
        if (startIndex.x > endIndex.x) {
            movementDirection = Direction.Left;
            numIterations = startIndex.x - endIndex.x;
        } else if ( startIndex.x < endIndex.x) {
            movementDirection = Direction.Right;
            numIterations = endIndex.x - startIndex.x;
        } else if (startIndex.y < endIndex.y){
            movementDirection = Direction.Up;
            numIterations = endIndex.y - startIndex.y;
        } else if (startIndex.y > endIndex.y) {
            movementDirection = Direction.Down;
            numIterations = startIndex.y - endIndex.y;
        }

        for (int i = 1; i <= numIterations && !isHitWall; i++) {
            GameObject curr;
            //Change curr based on which direction you are moving
            switch (movementDirection){
                case Direction.Left: 
                    curr = levelScripts.getTile(startIndex.x - i, startIndex.y);
                    break;
                case Direction.Right: 
                    curr = levelScripts.getTile(startIndex.x + i,startIndex.y);
                    break;
                case Direction.Up: 
                    curr = levelScripts.getTile(startIndex.x,startIndex.y + i);
                    break;
                case Direction.Down: 
                    curr = levelScripts.getTile(startIndex.x,startIndex.y - i);
                    break;
                default:
                    curr = null;
                    break;
            }

            //Only care about 'special' tiles
            Debug.Log("Tile " + i + ": " + curr?.tag);
            if (curr != null){
                switch(curr.tag){
                    case "Goal":
                        if (i == numIterations)
                            ans.playerEffect = Effect.Win;
                        break;
                    case "Pit":
                        if (i  == numIterations)
                            ans.playerEffect = Effect.Pit;
                        break;
                    case "Wall":
                        ans.playerEffect = Effect.Wall;
                        //TODO: Wall logic
                        switch (movementDirection){
                            case Direction.Left: 
                                ans.endPos.x = curr.transform.position.x + grid.cellSizeX;
                                break;
                            case Direction.Right: 
                                ans.endPos.x = curr.transform.position.x - grid.cellSizeX;
                                break;
                            case Direction.Up: 
                                ans.endPos.y = curr.transform.position.y - grid.cellSizeY;
                                break;
                            case Direction.Down: 
                                ans.endPos.y = curr.transform.position.y + grid.cellSizeY;
                                break;
                            default:
                                curr = null;
                                break;
                        }
                        isHitWall = true;
                        break;
                    default:
                        ans.endPos = goalPos;
                        ans.playerEffect = Effect.None;
                        break;
                }
            }
        }
        Debug.Log("End Index3: " + endIndex);

        if (attemptedEscape && ans.playerEffect == Effect.None){
            ans.endPos.x = endIndex.x * grid.cellSizeX + grid.cellSizeX/2;
            ans.endPos.y = endIndex.y * grid.cellSizeY + grid.cellSizeY/2;
            ans.playerEffect = Effect.Wall;
        }

        return ans;
    }

    void performConsequences(){
        switch (res.playerEffect){
            case Effect.Pit:
                levelScripts.restartLevel();
                break;
        }
    }
}