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
        public float speed;
        public float gravity;
        public float jumpForce;
        public float dashForce;
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
        MovePlayer();
        Dash();



    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        playerControl.isJumping = false;
    }

    private void MovePlayer()
    {
        float xMove = inputPlayer.GetAxisRaw("Horizontal");
        float zMove = inputPlayer.GetAxisRaw("Vertical");

        if (!inputPlayer.GetButton("Jump")) jumpTimer++;

        else if (inputPlayer.GetButton("Jump") && jumpTimer >= 1)
        {
            moveDirection.y = playerControl.jumpForce;
            playerControl.isJumping = true;
            jumpTimer = 0;
        }

        

        moveDirection.y -= time * playerControl.gravity;
        moveDirection.x = xMove;
        moveDirection.z = zMove;


        playerControl.isGrounded = (m_characterController.Move(moveDirection * time * playerControl.speed) & CollisionFlags.Below) != 0;
    }
    private void Dash()
    {
        if (!inputPlayer.GetButton("Dash")) dashTimer++;

        else if (inputPlayer.GetButton("Dash") && dashTimer >= 1)
        {
            moveDirection.z = playerControl.dashForce;
            playerControl.isDashing = true;
            dashTimer = 0;
        }
    }
    private void ReduceDashTime()
    {

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

    #endregion PRIVATE FIELDS


  

}
