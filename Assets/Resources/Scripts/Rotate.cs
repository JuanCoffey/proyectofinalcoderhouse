using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float RotationSpeed;

    void Start()
    {
    }

    void Update()
    {
        rotate();
    }

    private void rotate()
    {
        transform.Rotate(0, Time.deltaTime * RotationSpeed, 0, Space.World);
    }
}
