using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCaslte : MonoBehaviour
{
    public float CurrentHealth;
    public float Cooldown;

    public GameObject HealthBar;
    public GameObject CooldownBar;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = 1;
        Cooldown = 4;
    }

    
    public void UpdateHealthValue(float updateHealthValue)
    {
        CurrentHealth += updateHealthValue;
        HealthBar.GetComponent<Image>().fillAmount = CurrentHealth;
        
        Cooldown += 1;
        
        if (CurrentHealth<=0)
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
        CooldownBar.GetComponent<Image>().fillAmount =  1 - (Cooldown / 4f);

        if (Cooldown<=0)
        {
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        Cooldown = 4;


    }
}
