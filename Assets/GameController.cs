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
    public GameObject Crates;
    public GameObject Player;
    public GameObject SciFiProjectile;
    public GameObject MainCanvas;


    public GameObject[] EnemyCastlesPositions;
    public GameObject[] CratesPositions;
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
        Invoke("CreateCrate", 5);
        EnemyCastlesPositionsSelected = new List<byte>();
        
        SelectWeapon("0");
    }

    private void CreateCrate()
    {
      if (Crates.transform.childCount == 5)
        {
            //max crates reached, try again in 3 seconds
            Invoke("CreateCrate", 3);
            return;
        }

        //get a random spawn position for new crate
        int index = UnityEngine.Random.Range(0, CratesPositions.Length);
        GameObject spawnPoint = CratesPositions[index];


        string prefabName = "AmmoCrate";

        var rand = new System.Random();

        if (rand.NextDouble() >= 0.5)
        {
            prefabName = "HealthCrate";
        }

        GameObject crateInstance = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/"+ prefabName, typeof(GameObject)), Crates.transform) as GameObject;
        crateInstance.transform.position = spawnPoint.transform.position;
        crateInstance.transform.position = new Vector3(crateInstance.transform.position.x, 50, crateInstance.transform.position.z);
        crateInstance.name = "crate_" + Crates.transform.childCount;


        for (int i = 0; i < Crates.transform.childCount; i++)
        {
            //check if there is already a create at the desired position
            //if there is one, recursively call current method and try again
            if (crateInstance.transform.position == Crates.transform.GetChild(i).transform.position && !Crates.transform.GetChild(i).name.Equals(crateInstance.name)) 
            {
                Destroy(crateInstance);
                CreateCrate();
                return;
            }
        }
        

        Invoke("CreateCrate", 5);
    }

    private void CreateEnemyCastle()
    {
        if (EnemyCastlesPositionsSelected.Count == 3)
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
        rotateCastleTowards(enemyCastle);


        Invoke("CreateEnemyCastle", 5);
    }

    internal void PlayerDied()
    {
        MainCanvas.transform.Find("MainMenu").gameObject.SetActive(true);
    }

    void rotateCastleTowards(GameObject castle)
    {
        Quaternion r1 = Quaternion.LookRotation(Player.transform.position - castle.transform.position, Vector3.up);
        Vector3 euler2 = transform.eulerAngles;
        castle.transform.rotation = Quaternion.Euler(euler2.x, r1.eulerAngles.y, euler2.z);
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
