using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera playerCamera = null; // unity dose not complain 
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float rotSpeed = 10f;
    [SerializeField] float playerCameraHight = 10f;
    Vector3 curPlayerPos;
    Vector3 newPlayerPosition;
    Vector3 lastPlayerPos;
    Quaternion playerRotationCW = Quaternion.Euler(0, 90, 0);
    Quaternion playerRotationCCW = Quaternion.Euler(0, -90, 0);
    Quaternion curPlayerRotation;
    Quaternion targetPlayerRotation;
    [SerializeField] bool isMoving = false;
    [SerializeField] bool isPlayerLooking = false;
   
    // for camera look
    [SerializeField] float camLookSpeed = 2.0f;   
    float cameraYaw = 0f;
    float cameraPitch = 0f;
    Vector2 curMousePos;

    enum direction {north, south, east, west };
    direction playerFacing;
   
    // Start is called before the first frame update
    void Start()
    {
        // console instructions
        print("W,A,S,D to move and turn. L to mouse look");

        // start of level setup
        curPlayerRotation = transform.rotation;
        newPlayerPosition = GetComponent<Transform>().position;
        curPlayerPos = newPlayerPosition;
        UpdateCamAndPlayer();
        targetPlayerRotation = curPlayerRotation;

        GetComponent<MeshRenderer>().enabled = false; // make player invisible to the camera
       
        GetPlayerDirection(); // set the players direction 
        
    }


    // Update is called once per frame
    void Update()
    {
        print(playerFacing);
        PlayerInput();
        UpdateCamAndPlayer();
    }

    private void GetPlayerDirection()
    {
        if (Mathf.Floor(GetComponent<Transform>().eulerAngles.y) == 0)
        {
            playerFacing = direction.north;
            cameraYaw = 0f; // reset yaw for next mouse look
            
        }
        if (Mathf.Floor(GetComponent<Transform>().eulerAngles.y) == 270)
        {
            playerFacing = direction.west;
            cameraYaw = -90f; // reset yaw for next mouse look
        }
        if (Mathf.Floor(GetComponent<Transform>().eulerAngles.y) == 180)
        {
            playerFacing = direction.south;
            cameraYaw = 180f; // reset yaw for next mouse look
        }
        if (Mathf.Floor(GetComponent<Transform>().eulerAngles.y) == 90)
        {
            playerFacing = direction.east;
            cameraYaw = 90f; // reset yaw for next mouse look
        }
        cameraPitch = 0f; // reset pitch for next mouse look
    }

    private void UpdateCamAndPlayer()
    {
        // to get a better movement speed/feel
        float mSpeed =+ Time.deltaTime * moveSpeed;
        mSpeed = Mathf.Clamp01(mSpeed);
        float rSpeed = +Time.deltaTime * rotSpeed;
        rSpeed = Mathf.Clamp01(rSpeed);
        
        // get positions and rotations
        curPlayerPos = transform.position; // get the players current position
        curPlayerRotation = transform.rotation; // get the players current rotation Z is forward
        
        // move and rotate play
        transform.position = Vector3.Lerp(curPlayerPos, newPlayerPosition, mSpeed); // move the player from current position to the new position
        transform.rotation = Quaternion.Slerp(curPlayerRotation, targetPlayerRotation, rSpeed); // rotate the player form current rotation to the new rotation
        
        // make the camera match the player
        playerCamera.transform.position = new Vector3(curPlayerPos.x, curPlayerPos.y + playerCameraHight, curPlayerPos.z); // update the camera to players position with hight offset
        if (isPlayerLooking == false)
        {
            playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, curPlayerRotation , rSpeed); //rotate camera to player rotation... only executes if player is not in mouse look
        }
        // when all the moving is done give control back to the player
        if (curPlayerPos == newPlayerPosition && curPlayerRotation == targetPlayerRotation && isPlayerLooking == false)
        {
            GetPlayerDirection();
            isMoving = false;
        }
    }


    private void PlayerInput()
    {
        if (isMoving == false)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                MoveForward();
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                RotatePlayerCCW();
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                RotatePlayerCW();
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                MoveBackwards();
            }

        }
        // mouse look
        if(Input.GetKey(KeyCode.L))
        {
            if (isPlayerLooking == false)
            {
                isMoving = true;
            }
            MouseLook();
        }
        else if (!Input.GetKey(KeyCode.L) && isPlayerLooking == true)
        {
            isPlayerLooking = false;   
            GetPlayerDirection();
        }

    }

    private void MouseLook()
    {
        isPlayerLooking = true;

        cameraYaw += Input.GetAxis("Mouse X") * camLookSpeed;
        cameraPitch -= Input.GetAxis("Mouse Y") * camLookSpeed;
        playerCamera.transform.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
        
    }

    private void RotatePlayerCW()
    {
      
        targetPlayerRotation = curPlayerRotation * playerRotationCW;
        isMoving = true;
    }

    private void RotatePlayerCCW()
    {
        
        targetPlayerRotation = curPlayerRotation * playerRotationCCW;
        isMoving = true;
        
    }

    private void MoveForward()
    {
        lastPlayerPos = curPlayerPos;
        newPlayerPosition = curPlayerPos += transform.forward * 1f;
        isMoving = true;

    }

    private void MoveBackwards()
    {
        lastPlayerPos = curPlayerPos;
        newPlayerPosition = curPlayerPos -= transform.forward * 1f;
        isMoving = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            newPlayerPosition = lastPlayerPos; // if you hit that wall change the target position to the players last position
        }
    }
}
