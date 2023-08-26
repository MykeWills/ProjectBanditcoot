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


    void Update()
    {
        float storedXSpeed = moveXSpeed;
        float storedYSpeed = moveYSpeed;
        float storedZSpeed = moveXSpeed;

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
