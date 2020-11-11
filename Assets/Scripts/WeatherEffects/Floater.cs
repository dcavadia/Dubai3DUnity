using UnityEngine;
using System.Collections;

// Makes objects float up & down.
public class Floater : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float degreesPerSecond;
    public float amplitude;
    public float frequency;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Awake()
    {
        // Store the starting position
        posOffset = transform.position;
    }

    void Update()
    {
        // Float up/down with a Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}