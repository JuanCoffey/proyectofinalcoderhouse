using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCastle : MonoBehaviour
{
    public float CurrentHealth;
    public float Cooldown;

    public GameObject projectile0;
    public GameObject projectile1;

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

        if (GameController.Instance.CurrentState == GameController.ApplicationState.InGame)
        {
            HealthBar.transform.parent.transform.parent.gameObject.SetActive(true);
        }
    }


    public void UpdateHealthValue(float updateHealthValue)
    {
        CurrentHealth += updateHealthValue;
        HealthBar.GetComponent<Image>().fillAmount = CurrentHealth;

        Cooldown += 1;

        if (CurrentHealth <= 0 && CurrentHealth > -1)
        {
            CurrentHealth = -1;
            this.CastleDied();
        }
    }

    private void CastleDied()
    {
        GameController.Instance.EnemyDied();

        Destroy(gameObject, 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.CurrentState == GameController.ApplicationState.MainMenu)
        {
            return;
        }

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
        GameObject fireProjectile = null;

        if (CurrentCannonFire == 0)
        {
            fireProjectile = projectile0;
            CurrentCannonFire = 1;
            spawnPosition = Cannon0.transform.Find("SpawnPoint");
        }
        else
        {
            fireProjectile = projectile1;
            CurrentCannonFire = 0;
            spawnPosition = Cannon1.transform.Find("SpawnPoint");
        }

        GameObject newProjectile = Instantiate(fireProjectile, spawnPosition.position, Quaternion.identity) as GameObject; 
        newProjectile.transform.LookAt(GameController.Instance.Player.transform); 
        newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * 8000); 
        newProjectile.transform.localScale = Vector3.one * 15;

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
