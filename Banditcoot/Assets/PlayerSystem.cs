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

        Debug.Log(moveDirection.y);

        moveDirection.y -= time * playerControl.gravity;
        moveDirection.x = xMove;
        moveDirection.z = zMove;


        playerControl.isGrounded = (m_characterController.Move(moveDirection * time * playerControl.speed) & CollisionFlags.Below) != 0;
    }
    private void Dash()
    {

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

    #endregion PRIVATE FIELDS


  

}
