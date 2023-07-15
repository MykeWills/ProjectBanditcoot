using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerSystem : MonoBehaviour
{


    [Serializable]
    public struct PlayerControl
    {
        public float dashTimeReduceSpeed;
        public float gravity;
        public float jumpForce;
        public float dashForce;
        public float walkSpeed;
        public float runSpeed;
        public bool isJumping;
        public bool isGrounded;
        public bool isMoving;
        public bool isRunnning;
        public bool isDashing;
        
    }

    public PlayerControl playerControl;


    // Start is called before the first frame update
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()

    {
        Dash();
        Jump();
        MovePlayer();
        



    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        playerControl.isJumping = false;
    }

    private void MovePlayer()
    {
       
        moveDirection.y -= time * playerControl.gravity;
        moveDirection.x = xMove;
        if (!playerControl.isDashing)
        {
            // moving character controller forward/back on input
            
            moveDirection.z = zMove;
        }
        else
        {
            // moving character controller forward/back on dash
            if(moveDirection.z > 0)
            {
                
                
                moveDirection.z -= time * playerControl.dashTimeReduceSpeed;
                moveDirection.z = Mathf.Clamp(moveDirection.z, 0, playerControl.dashForce);
                


            }
            
            else if(moveDirection.z < 0)
            {
                
                
                moveDirection.z += time * playerControl.dashTimeReduceSpeed;
                moveDirection.z = Mathf.Clamp(moveDirection.z, -playerControl.dashForce,0);
                


            }
            if (moveDirection.z == 0)
            {
                playerControl.isDashing = false;
            }

        }
        playerControl.isRunnning = inputPlayer.GetButton("Run");
        moveSpeed = playerControl.isRunnning ? playerControl.runSpeed : playerControl.walkSpeed;
        playerControl.isGrounded = (m_characterController.Move(moveDirection * time * moveSpeed) & CollisionFlags.Below) != 0;
    }

    private void Jump()
    {
        if (playerControl.isJumping == true)
        {
            return;
        }
        if (!inputPlayer.GetButton("Jump")) jumpTimer++;

        else if (inputPlayer.GetButton("Jump") && jumpTimer >= 1)
        {
            moveDirection.y = playerControl.jumpForce;
            playerControl.isJumping = true;
            jumpTimer = 0;
        }


    }
    private void Dash()
    {
        if (playerControl.isDashing)
        {
            return;
        }
        if (!inputPlayer.GetButton("Dash")) dashTimer++;

        else if (inputPlayer.GetButton("Dash") && dashTimer > 0)
        {
            if (zMove > 0)
            {
                moveDirection.z = playerControl.dashForce;
            }
            else if (zMove < 0)
            {
                moveDirection.z = -playerControl.dashForce;
            }
            
            playerControl.isDashing = true;
            dashTimer = 0;
        }
    }
    

    #region PRIVATE FIELDS
    private CharacterController m_characterController => GetComponent<CharacterController>();

    //private Transform transform => transform;

    //private Rigidbody rb => GetComponent<Rigidbody>();
    private float time => Time.deltaTime;

    private Vector3 moveDirection = Vector3.zero;
    private Player inputPlayer => ReInput.players.GetPlayer(0);
    private float jumpTimer = 0;
    private float dashTimer = 0;
    private float xMove => inputPlayer.GetAxisRaw("Horizontal");
    private float zMove => inputPlayer.GetAxisRaw("Vertical");
    private float moveSpeed = 0;
    #endregion PRIVATE FIELDS




}
