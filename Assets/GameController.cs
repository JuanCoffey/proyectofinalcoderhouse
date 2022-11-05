using SciFiArsenal;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public GameObject EnemyCastles;
    public GameObject Player;
    public GameObject SciFiProjectile;


    public GameObject[] EnemyCastlesPositions;
    public List<byte> EnemyCastlesPositionsSelected;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    void Start()
    {
        Invoke("CreateEnemyCastle", 5);
        EnemyCastlesPositionsSelected = new List<byte>();
        SelectWeapon("0");
    }

    private void CreateEnemyCastle()
    {
        if (EnemyCastlesPositionsSelected.Count == 4)
        {
            return;
        }


        byte randomNumber = (byte)UnityEngine.Random.Range(0, EnemyCastlesPositions.Length);

        for (int i = 0; i < EnemyCastlesPositionsSelected.Count; i++)
        {
            if (randomNumber == EnemyCastlesPositionsSelected[i])
            {
                CreateEnemyCastle();
                return;
            }
        }

        EnemyCastlesPositionsSelected.Add(randomNumber);
        GameObject enemyCastle = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/castle1", typeof(GameObject)), EnemyCastles.transform) as GameObject;
        enemyCastle.transform.position = EnemyCastlesPositions[randomNumber].transform.position;
        enemyCastle.transform.position = new Vector3(enemyCastle.transform.position.x, 15, enemyCastle.transform.position.z);

        Invoke("CreateEnemyCastle", 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("" + 1))
        {
            SelectWeapon("0");
        }
        else if (Input.GetKeyDown("" + 2))
        {
            SelectWeapon("1");
        }
        else
        if (Input.GetKeyDown("" + 3))
        {
            SelectWeapon("2");
        }

    }

    public void FireWeapon(string weaponIndex)
    {



    }

    public void SelectWeapon(string weaponIndex)
    {
        SciFiFireProjectile sciFiFireProjectile = SciFiProjectile.GetComponent<SciFiFireProjectile>();
        sciFiFireProjectile.CurrentWeaponSelected = byte.Parse(weaponIndex);
        sciFiFireProjectile.WeaponSelected = sciFiFireProjectile.Weapons[sciFiFireProjectile.CurrentWeaponSelected];
        sciFiFireProjectile.imgWeapon0Cooldown.transform.parent.Find("imgSelectedBg").gameObject.SetActive(false);
        sciFiFireProjectile.imgWeapon1Cooldown.transform.parent.Find("imgSelectedBg").gameObject.SetActive(false);
        sciFiFireProjectile.imgWeapon2Cooldown.transform.parent.Find("imgSelectedBg").gameObject.SetActive(false);

        sciFiFireProjectile.imgWeapon0Cooldown.transform.parent.Find("Border").gameObject.SetActive(false);
        sciFiFireProjectile.imgWeapon1Cooldown.transform.parent.Find("Border").gameObject.SetActive(false);
        sciFiFireProjectile.imgWeapon2Cooldown.transform.parent.Find("Border").gameObject.SetActive(false);

        sciFiFireProjectile.WeaponIdleParticle0.SetActive(false);
        sciFiFireProjectile.WeaponIdleParticle1.SetActive(false);
        sciFiFireProjectile.WeaponIdleParticle2.SetActive(false);

        switch (weaponIndex)
        {
            case "0":
                sciFiFireProjectile.imgWeapon0Cooldown.transform.parent.Find("imgSelectedBg").gameObject.SetActive(true);
                sciFiFireProjectile.imgWeapon0Cooldown.transform.parent.Find("Border").gameObject.SetActive(true);
                sciFiFireProjectile.WeaponIdleParticle0.SetActive(true);
                break;
            case "1":
                sciFiFireProjectile.imgWeapon1Cooldown.transform.parent.Find("imgSelectedBg").gameObject.SetActive(true);
                sciFiFireProjectile.imgWeapon1Cooldown.transform.parent.Find("Border").gameObject.SetActive(true);
                sciFiFireProjectile.WeaponIdleParticle1.SetActive(true);
                break;
            case "2":
                sciFiFireProjectile.imgWeapon2Cooldown.transform.parent.Find("imgSelectedBg").gameObject.SetActive(true);
                sciFiFireProjectile.imgWeapon2Cooldown.transform.parent.Find("Border").gameObject.SetActive(true);
                sciFiFireProjectile.WeaponIdleParticle2.SetActive(true);
                break;
            default:
                break;
        }

    }

    internal void EnemyDied()
    {



    }
}
