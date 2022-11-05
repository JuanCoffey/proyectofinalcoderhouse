using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCaslte : MonoBehaviour
{
    public float CurrentHealth;
    public float Cooldown;

    public GameObject projectile;

    public GameObject HealthBar;
    public GameObject CooldownBar;

    public GameObject Cannon0;
    public GameObject Cannon1;

    public byte CurrentCannonFire;

    const float MaxCooldown = 9f;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = 1;
        Cooldown = MaxCooldown;
        CurrentCannonFire = 0;
    }


    public void UpdateHealthValue(float updateHealthValue)
    {
        CurrentHealth += updateHealthValue;
        HealthBar.GetComponent<Image>().fillAmount = CurrentHealth;

        Cooldown += 1;

        if (CurrentHealth <= 0)
        {
            this.CastleDied();
        }
    }

    private void CastleDied()
    {
        GameController.Instance.EnemyDied();

        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        Cooldown -= Time.deltaTime;
        CooldownBar.GetComponent<Image>().fillAmount = 1 - (Cooldown / MaxCooldown);

        if (Cooldown <= 0)
        {
            FireWeapon();
        }

        rotateCannonTowards(GameController.Instance.Player.transform, Cannon0.transform);
        rotateCannonTowards(GameController.Instance.Player.transform, Cannon1.transform);
    }

    private void FireWeapon()
    {
        Cooldown = MaxCooldown;
        Transform spawnPosition = null;

        if (CurrentCannonFire == 0)
        {
            CurrentCannonFire = 1;
            spawnPosition = Cannon0.transform.Find("barrel").transform.Find("SpawnPoint");
        }
        else
        {
            CurrentCannonFire = 0;
            spawnPosition = Cannon1.transform.Find("barrel").transform.Find("SpawnPoint");
        }

        GameObject newProjectile = Instantiate(projectile, spawnPosition.position, Quaternion.identity) as GameObject; //Spawns the selected projectile
        newProjectile.transform.LookAt(GameController.Instance.Player.transform); //Sets the projectiles rotation to look at the point clicked
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * 8000); //Set the speed of the projectile by applying force to the rigidbody
        newProjectile.transform.localScale = Vector3.one * 9;

    }

    void rotateCannonTowards(Transform target, Transform cannon)
    {
        Vector3 targetDirection = target.position - cannon.position;

        float singleStep = 20 * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(cannon.forward, targetDirection, singleStep, 0.0f);

        Debug.DrawRay(cannon.position, newDirection, Color.red);

        cannon.rotation = Quaternion.LookRotation(newDirection);
    }
}
