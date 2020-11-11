using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraZoom : MonoBehaviour
{

    Vector3 nextPosition = new Vector3(78.7f, 380, 350);
    Quaternion nextRotation = Quaternion.Euler(10, 0, 0);

    float moveSpeed = 1f; //or public float moveSpeed; if in C#
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamera();
    }

    void MoveCamera()
    {
        transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, nextRotation, Time.deltaTime * moveSpeed);
    }
}
