using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformSystem : MonoBehaviour
{

    [SerializeField] private float moveXSpeed = 1;
    [SerializeField] private float moveYSpeed = 1;
    [SerializeField] private float moveZSpeed = 1;
    [Space]
    [SerializeField] private bool switchDirections;
    private Transform gameManager => GameManager.gameManager.transform;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            

            other.transform.parent = transform;
        }


    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.parent = gameManager;
        }

    }
    void FixedUpdate()

    {
        
        
        float storedXSpeed = moveXSpeed;
        float storedYSpeed = moveYSpeed;
        float storedZSpeed = moveZSpeed;

        if(switchDirections == true)
        {
            storedXSpeed = -storedXSpeed;
            storedYSpeed = -storedYSpeed;
            storedZSpeed = -storedZSpeed;
        }

        transform.position += new Vector3(Time.deltaTime * storedXSpeed, Time.deltaTime * storedYSpeed, Time.deltaTime * storedZSpeed);

        // Clamp on X

        if (transform.position.x > 100)
        {
            switchDirections = true;
        }
        else if (transform.position.x < 0)
        {
            switchDirections = false;
        }

        // Clamp on Y
        if (transform.position.y > 100)
        {
            switchDirections = true;
        }
        else if (transform.position.y < 0)
        {
            switchDirections = false;
        }

        // Clamp on Z
        if (transform.position.z > 100)
        {
            switchDirections = true;
        }
        else if(transform.position.z < 0)
        {
            switchDirections = false;
        }
   

    }

    
   
}
