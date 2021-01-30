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

    // Queue<Vector2> movement = new Queue<Vector2>(); 
    bool isMoving = false;
    Vector2 input;

    Vector3 startPos; //Starting Position
    Vector3 endPos; //Movement destination
    float progress; //Progress to finishing movement

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
                endPos = new Vector3(startPos.x + input.x * (float)1.2 * tilesToMove, 
                                     startPos.y + input.y * tilesToMove, 
                                     startPos.z);
                isMoving = true;
                MovementData res = calculateMovement(startPos, endPos);
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
            }
        }
            // if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))){
            //     movement.Enqueue(new Vector2(1,1));
            // } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)){
            //     movement.Enqueue(new Vector2(1,-1));
            // }

            // if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            //     movement.Enqueue(new Vector2(0,-1));
            // } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            //     movement.Enqueue(new Vector2(0,1));
            // }
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
        if (endIndex.x > 8 || endIndex.x < 0 || endIndex.y > 8 || endIndex.y < 0) {
            attemptedEscape = true;
            //Adjust x
            endIndex.x = endIndex.x > 8 ? 8 : endIndex.x;
            endIndex.x = endIndex.x < 0 ? 0 : endIndex.x;
            //Adjust y
            endIndex.y = endIndex.y > 8 ? 8 : endIndex.y;
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
                        if (i + 1 == numIterations)
                            ans.playerEffect = Effect.Win;
                        break;
                    case "Pit":
                        if (i + 1 == numIterations)
                            ans.playerEffect = Effect.Pit;
                        break;
                    case "Wall":
                        ans.playerEffect = Effect.Wall;
                        //TODO: Wall logic
                        switch (movementDirection){
                            case Direction.Left: 
                                ans.endPos.x = curr.transform.position.x + (float)1.2;
                                break;
                            case Direction.Right: 
                                ans.endPos.x = curr.transform.position.x - (float)1.2;
                                break;
                            case Direction.Up: 
                                ans.endPos.y = curr.transform.position.y - (float)1;
                                break;
                            case Direction.Down: 
                                ans.endPos.y = curr.transform.position.y + (float)1;
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
            ans.endPos.x = endIndex.x * 1.2f + .6f;
            ans.endPos.y = endIndex.y + .5f;
            ans.playerEffect = Effect.Wall;
        }

        return ans;
    }

    // void FixedUpdate(){
    //     while (movement.Count > 0) {
    //         Vector2 currMove = movement.Dequeue();

    //         if(currMove.y == 0) continue;

    //         Vector3 positionChange = new Vector3(0,0,0);

    //         switch (currMove.x) {
    //             //X movement
    //             case 0:
    //                 positionChange.x = currMove.y * (float)1.2 * speedMultiplier; 
    //                 break;
    //             //Y movement
    //             case 1:
    //                 positionChange.y = currMove.y * speedMultiplier; 
    //                 break;
    //             default:
    //                 Debug.Log("Something went very wrong in player movement!");
    //                 break;
    //         }
    //         Debug.Log(player.transform.position);
    //         Debug.Log("PositionChange: " + positionChange);
    //         player.transform.position = Vector3.MoveTowards(player.transform.position, player.transform.position + positionChange, Time.fixedDeltaTime*(float)moveTime);
    //         // rb.MovePosition(rb.position + positionChange);
    //         speedMultiplier++;
    //     }
    // }
}