using SciFiArsenal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float CurrentHealth;
    public GameObject HealthBar;

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

        float movX = Input.GetAxis("Horizontal") * -2f;
        float movZ = Input.GetAxis("Vertical") * -5f;
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

        //   transform.Translate(new Vector3(movX, 0, movZ));
        gameObject.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * movZ * 100);
        gameObject.GetComponent<Rigidbody>().AddForce(transform.right * movX * 100);
        transform.Rotate(new Vector3(0, movRotation, 0));
    }

    public void ResetHealth()
    {


        CurrentHealth = 1;
        HealthBar.GetComponent<Image>().fillAmount = CurrentHealth;

    }

    public void UpdateHealthValue(float updateHealthValue)
    {
        CurrentHealth += updateHealthValue;
        HealthBar.GetComponent<Image>().fillAmount = CurrentHealth;

        if (CurrentHealth <= 0)
        {
            GameController.Instance.PlayerDied();
        }
    }

    void OnCollisionEnter(Collision hit)
    {
        if (hit.collider.gameObject.CompareTag("AmmoCrate"))
        {
            byte ranAmmoCode = (byte)UnityEngine.Random.Range(0, 3);
            byte ranAmmoCant = (byte)UnityEngine.Random.Range(1, 4);

            gameObject.transform.Find("SciFiFireProjectile").gameObject.GetComponent<SciFiFireProjectile>().PickUpAmmo(ranAmmoCode, ranAmmoCant);

            Destroy(hit.collider.gameObject.transform.parent.gameObject);
        }
        else if (hit.collider.gameObject.CompareTag("HealthCrate"))
        {
            PickUpHealth();

            Destroy(hit.collider.gameObject.transform.parent.gameObject);
        }
    }

    private void PickUpHealth()
    {
        UpdateHealthValue(0.2f);
    }
}
