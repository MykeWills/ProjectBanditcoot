using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    
    // Start is called before the first frame update
    private void Start()
    {
         rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    private void Update()

    {
        


        float xMove = Input.GetAxisRaw("Horizontal");
        float zMove = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(xMove, rb.velocity.y, zMove) * speed;



    }

    

    #region PRIVATE FIELDS
    private CharacterController m_characterController => GetComponent<CharacterController>();

    private Transform transform => transform;

    private Rigidbody rb;

    private float speed = 10f;
    








    #endregion PRIVATE FIELDS




}
