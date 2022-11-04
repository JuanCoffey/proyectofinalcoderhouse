using SciFiArsenal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        checkInput();
    }
    private void checkInput()
    {

        float movX = Input.GetAxis("Horizontal") * -0.2f;
        float movZ = Input.GetAxis("Vertical") * -0.2f;
        //   float movRotation = Input.GetAxis("Rotate") * .9f;
        float movRotation = 0;

        if (Input.GetKey(KeyCode.Q))
        {
            movRotation = -0.5f;
        }
        else if (Input.GetKey(KeyCode.E))
        {
            movRotation = 0.5f;
        }

        transform.Translate(new Vector3(movX, 0, movZ));

        transform.Rotate(new Vector3(0, movRotation, 0));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("AmmoCrate"))
        {
            gameObject.GetComponent<SciFiFireProjectile>().PickUpAmmo(1,2);

            Destroy(collision.gameObject);
        }
    }

}
