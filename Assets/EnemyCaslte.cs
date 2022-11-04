using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCaslte : MonoBehaviour
{
    public float CurrentHealth;

    public GameObject HealthBar;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = 1;
    }

    
    public void UpdateHealthValue(float updateHealthValue)
    {
        CurrentHealth += updateHealthValue;
        HealthBar.GetComponent<Image>().fillAmount = CurrentHealth;
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
        
    }
}
