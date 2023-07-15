using System;
using UnityEngine;
using Rewired;

public class PlayerSystemReference : MonoBehaviour
{
    #region PUBLIC FIELDS

    public PlayerControl playerControl;

    #endregion PUBLIC FIELDS



    #region UNITY FUNCTIONS

    private void Start()
    {
        // Start is called before the first frame update
    }


    private void Update()
    {
        // Update is called once per frame

        /// Note: player movement should always account the adjusted movement from actions so it should be always be called afterwards.

        //player actions
        ActionTimer();
        Dash();
        Jump();

        //player movement
        MovePlayer();

    }


    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        /// Functionality: OnControllerColliderHit() is a Unity function that is called whenever the character controller component collides with an object.


        /// Note: This may cause problems because the controller "could collide with anything" and this will be called.

        // when character controller collides with an object, no longer jumping
        playerControl.isJumping = false;
    }

    #endregion UNITY FUNCTIONS



    #region PLAYER FUNCTIONS

    private void MovePlayer()
    {
        /// Functionality: This function accounts for the player movement, which moves the character controller component attached to the player object.

        // player is moving if either input is being used.
        playerControl.isMoving = (m_xMove != 0 || m_zMove != 0);

        // reduce the playerControl.moveDirection Y by time and gravity to [Always] force player downwards
        playerControl.moveDirection.y -= m_time * playerControl.gravity;

        // moving character controller [left/right] on [Horizontal] input
        playerControl.moveDirection.x = m_xMove;


        // check if the player [IS NOT] dashing
        if (playerControl.isDashing == false)
        {
            // moving character controller [forward/back] on [Vertical] input
            playerControl.moveDirection.z = m_zMove;
        }

        // check if the player [IS] dashing
        else if(playerControl.isDashing == true)
        {

            // check if the direction the player is moving [forward] ---> POSITIVE
            if (playerControl.moveDirection.z > 0)
            {
                // since the playerControl.moveDirection Z is [forward], lets [DECREASE] the move over time
                playerControl.moveDirection.z -= m_time * playerControl.dashTimeReduceSpeed;

                /// Note: we want the force amount to be no faster than the force applied, but we want to clamp the minimum value to 0 to "check it".
                // lets clamp the playerControl.moveDirection Z between [MIN == 0] and [MAX == positive dashForce]
                playerControl.moveDirection.z = Mathf.Clamp(playerControl.moveDirection.z, 0, playerControl.dashForce);
            }


            // check if the direction the player is moving [back] ---> NEGATIVE 
            else if (playerControl.moveDirection.z < 0)
            {
                // since the playerControl.moveDirection Z is [back], lets [INCREASE] the move over time
                playerControl.moveDirection.z += m_time * playerControl.dashTimeReduceSpeed;

                /// Note: we want the force amount to be no less than the force applied, but we want to clamp the maximum value to 0 to "check it".
                // lets clamp the playerControl.moveDirection Z between [MIN == negative dashforce] and [MAX == 0]
                playerControl.moveDirection.z = Mathf.Clamp(playerControl.moveDirection.z, -playerControl.dashForce, 0);
            }


            // check if the playerControl.moveDirection Z is [not moving]
            if (playerControl.moveDirection.z == 0)
            {
                // shut off the  player dashing
                playerControl.isDashing = false;
            }

        }


        // set the [isRunning] toggle to the button press - if [Run] is pressed, isRunning will be [True]
        playerControl.isRunning = m_inputPlayer.GetButton("Run");

        // adjust the players move speed based on if the player [isRunning]
        m_moveSpeed = playerControl.isRunning ? playerControl.runSpeed : playerControl.walkSpeed;

        /// Note: This is simplized code, it is the same as this ---->

        ///if (playerControl.isRunning)
        ///{
        ///    moveSpeed = playerControl.runSpeed;
        ///}
        ///else
        ///{
        ///    moveSpeed = playerControl.walkSpeed;
        ///}

        // code that's responsible for [Both] moving the player by moveSpeed and checking if it's grounded
        playerControl.isGrounded = (m_characterController.Move(playerControl.moveDirection * m_time * m_moveSpeed) & CollisionFlags.Below) != 0;

        ///Note: This is simplized code, it is the same as this ---->

        // moves the player based on moveDirection times speed over time
        ///m_characterController.Move(playerControl.moveDirection * time * moveSpeed;

        // checks if theres a collision below the character controller collider
        ///if ((m_characterController.collisionFlags & CollisionFlags.Below) != 0)
        ///{
        ///    playerControl.isGrounded = true;
        ///}
        ///else
        ///{
        ///    playerControl.isGrounded = false;
        ///}
    }


    private void ActionTimer()
    {
        /// Note: Decided to put both [jumpTimer] and [dashTimer] together as an [actionTimer].

        /// Functionality: Allows the action value to increase when Jump or Dash buttons are not pressed. 
        ///                When the action value is [Greater] than 0 the action can be pressed again.


        // check to see if both dash or jump are not being pressed, increase action value if not.
        if (!m_inputPlayer.GetButton("Jump") && !m_inputPlayer.GetButton("Dash"))
        {
            // increase action timer, Plus Plus means ----> [m_actionTimer += 1] or [m_actionTimer = m_actionTimer + 1]
            m_actionTimer++;
        }
    }


    private void Jump()
    {
        /// Functionality: This function accounts for the player [Jump] action, which applies [Positive] force to playerControl.moveDirection Y by pressing [Jump] Button.


        // if the player [isJumping] dont run any code below
        if (playerControl.isJumping == true)
        {
            return;
        }


        // while the jump button [is] pressed [AND] the action value is [Greater] than 0, make the player jump.
        if (m_inputPlayer.GetButton("Jump") && m_actionTimer > 0)
        {
            // set the playerControl.moveDirection Y to the jump force
            playerControl.moveDirection.y = playerControl.jumpForce;

            // player is now jumping
            playerControl.isJumping = true;

            // reset the jump value back to 0 so this [If Statement] stops running while the jump button is pressed.
            m_actionTimer = 0;
        }


    }


    private void Dash()
    {
        /// Functionality: This function accounts for the player [Dash] action, which applies [Positive or Negative] force to playerControl.moveDirection Z by pressing [Dash] Button.

        // if the player [isDashing] dont run any code below
        if (playerControl.isDashing)
        {
            return;
        }

        // while the dash button [is] pressed [AND] the action value is [Greater] than 0, make the player dash.
        if (m_inputPlayer.GetButton("Dash") && m_actionTimer > 0)
        {
            /// Note: Since we want the player to dash [forward] or [backward] based on Input, we need to check to see if the player input is moving [forward] or [Backward] 

            // check to see if player is moving [forward]
            if (m_zMove > 0)
            {
                // set the playerControl.moveDirection Z to [POSITIVE] dash force
                playerControl.moveDirection.z = playerControl.dashForce;
            }

            // check to see if player is moving [backward]
            else if (m_zMove < 0)
            {
                // set the playerControl.moveDirection Z to [NEGATIVE] dash force
                playerControl.moveDirection.z = -playerControl.dashForce;
            }

            // player is now dashing
            playerControl.isDashing = true;

            // reset the dash value back to 0 so this [If Statement] stops running while the dash button is pressed.
            m_actionTimer = 0;
        }
    }

    #endregion PLAYER FUNCTIONS



    #region PRIVATE FIELDS

    /// <summary>
    /// 
    /// INFORMATION ABOUT MEMBER VARIABLES - HUNGARIAN NOTATION: The reason for using m_ for private fields names --------->
    /// 
    /// This is typical programming practice for defining variables that are member variables. 
    /// So when you're using them later, you don't need to see where they're defined to know their scope. 
    /// This is also great if you already know the scope and you're using something like intelliSense, 
    /// you can start with m_ and a list of all your member variables are shown. 
    /// Part of Hungarian notation.
    /// 
    /// </summary>

    private CharacterController m_characterController => GetComponent<CharacterController>();
    private Rigidbody m_rb => GetComponent<Rigidbody>();
    private Player m_inputPlayer => ReInput.players.GetPlayer(0);
    private float m_time => Time.deltaTime;
    private float m_xMove => m_inputPlayer.GetAxisRaw("Horizontal");
    private float m_zMove => m_inputPlayer.GetAxisRaw("Vertical");

    private float m_actionTimer = 0;
    private float m_moveSpeed = 0;

    #endregion PRIVATE FIELDS



    #region STRUCTS
    [Serializable]
    /// we created a struct that tidies and keeps all player movement values together which can [all] be saved so a single value later
    public struct PlayerControl
    {
        [Header("Player Settings")]
        public float dashTimeReduceSpeed;
        public float gravity;
        public float jumpForce;
        public float dashForce;
        public float walkSpeed;
        public float runSpeed;

        //[Space]
        //[Header("Player Physics")]
        //public float momentum;
        //public float fallIncrease;
        //public float acceleration;
        //public float deceleration;
        //public float friction;

        [Space]
        [Header("Player Actions")]
        public bool isJumping;
        public bool isGrounded;
        public bool isMoving;
        public bool isRunning;
        public bool isDashing;
        [Space]
        [Header("Player Movement")]
        public Vector3 moveDirection;

        //[Space]
        //[Header("Player Look")]
        //public bool lookSpeed;
        //public bool lookAngle;


    }
    #endregion STRUCTS
}
