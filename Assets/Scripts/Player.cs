using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] Camera playerCamera;
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
   
    // Start is called before the first frame update
    void Start()
    {
        // start of level setup
        curPlayerRotation = transform.rotation;
        newPlayerPosition = GetComponent<Transform>().position;
        curPlayerPos = newPlayerPosition;
        UpdateCamAndPlayer();
        targetPlayerRotation = curPlayerRotation;

        GetComponent<MeshRenderer>().enabled = false; // make player invisibale to the camera

    }


    // Update is called once per frame
    void Update()
    {
        PlayerInput();
        UpdateCamAndPlayer();
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
        playerCamera.transform.rotation = curPlayerRotation; // update the camera to players rotation local z is forward
        
        // when all the moving is done give control back to the player
        if (curPlayerPos == newPlayerPosition && curPlayerRotation == targetPlayerRotation)
        {
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
            newPlayerPosition = lastPlayerPos; // if you hit that wall change the target position to the plaers last position
        }
    }
}
