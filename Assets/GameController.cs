using SciFiArsenal;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public GameObject EnemyCastles;
    public GameObject Crates;
    public GameObject Player;
    public GameObject MainCanvas;
    public GameObject PresentationCamera;
    public GameObject PlayerSpawnPoint;


    public GameObject[] EnemyCastlesPositions;
    public GameObject[] CratesPositions;
    public List<byte> EnemyCastlesPositionsSelected;
    public List<byte> CratesPositionsUsed;
    public ApplicationState CurrentState;

    public GameObject InitialCastlePosition;


    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        CurrentState = ApplicationState.MainMenu;
    }

    void Start()
    {

    }

    public void StartNewGame()
    {
        CurrentState = ApplicationState.InGame;
        PresentationCamera.SetActive(false);
        Player.SetActive(true);
        Player.transform.position = PlayerSpawnPoint.transform.position;

        foreach (Transform castle in EnemyCastles.transform)
        {
            castle.gameObject.GetComponent<EnemyCastle>().HealthBar.transform.parent.transform.parent.gameObject.SetActive(true);
        }

        SciFiFireProjectile sciFiFireProjectile = Player.transform.Find("SciFiFireProjectile").gameObject.GetComponent<SciFiFireProjectile>();
        sciFiFireProjectile.AmmoWeapon0 = 2;
        sciFiFireProjectile.AmmoWeapon1 = 2;
        sciFiFireProjectile.AmmoWeapon2 = 2;

        sciFiFireProjectile.imgWeapon0Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = sciFiFireProjectile.AmmoWeapon0 + "♦";
        sciFiFireProjectile.imgWeapon1Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = sciFiFireProjectile.AmmoWeapon1 + "♦";
        sciFiFireProjectile.imgWeapon2Cooldown.transform.parent.Find("lblAmmo").gameObject.GetComponent<TextMeshProUGUI>().text = sciFiFireProjectile.AmmoWeapon2 + "♦";



        Invoke("CreateEnemyCastle", 5);
        Invoke("CreateCrate", 5);
        EnemyCastlesPositionsSelected = new List<byte>();
        CratesPositionsUsed = new List<byte>();

        SelectWeapon("0");
        MainCanvas.transform.Find("imgMainMenuBg").gameObject.SetActive(false);
        MainCanvas.transform.Find("MainMenu").gameObject.SetActive(false);
        MainCanvas.transform.Find("GameCanvas").gameObject.SetActive(true);
        Player.GetComponent<Player>().ResetHealth();
        MainCanvas.transform.Find("MainMenu").transform.Find("lblLost").gameObject.SetActive(false);
        MainCanvas.transform.Find("MainMenu").transform.Find("lblWon").gameObject.SetActive(false);
    }


    public void OpenKeyBindings()
    {
        MainCanvas.transform.Find("KeyBindingsMenu").gameObject.SetActive(true);

    }
    public void CloseKeyBindings()
    {
        MainCanvas.transform.Find("KeyBindingsMenu").gameObject.SetActive(false);

    }

    public void RemoveCrateFromPositionsUsed(string crateName)
    {
        byte index = byte.Parse(crateName.Replace("crate_", ""));

        for (int i = CratesPositionsUsed.Count - 1; i >= 0; i--)
        {
            if (CratesPositionsUsed[i] == index)
            {
                CratesPositionsUsed.RemoveAt(i);
            }
        }
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
        byte index = (byte)UnityEngine.Random.Range(0, CratesPositions.Length);

        foreach (var cratesPositionsUsedIndex in CratesPositionsUsed)
        {
            if (cratesPositionsUsedIndex == index)
            {
                CreateCrate();
                return;
            }
        }

        CratesPositionsUsed.Add(index);

        GameObject spawnPoint = CratesPositions[index];

        string prefabName = "AmmoCrate";

        var rand = new System.Random();

        if (rand.NextDouble() >= 0.65)
        {
            prefabName = "HealthCrate";
        }

        GameObject crateInstance = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/" + prefabName, typeof(GameObject)), Crates.transform) as GameObject;
        crateInstance.transform.position = spawnPoint.transform.position;
        crateInstance.transform.position = new Vector3(crateInstance.transform.position.x, 50, crateInstance.transform.position.z);
        crateInstance.transform.Find("Capsule").name = "crate_" + index;

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
        createCastle(EnemyCastlesPositions[randomNumber].transform.position, true);

        Invoke("CreateEnemyCastle", 5);
    }

    private void createCastle(Vector3 position, bool rotateTowardsPlayer)
    {
        GameObject enemyCastle = UnityEngine.Object.Instantiate(Resources.Load("Prefabs/castle1", typeof(GameObject)), EnemyCastles.transform) as GameObject;
        enemyCastle.transform.position = position;
        enemyCastle.transform.localPosition = new Vector3(enemyCastle.transform.localPosition.x, 15, enemyCastle.transform.localPosition.z);

        if (rotateTowardsPlayer)
        {
            rotateCastleTowards(enemyCastle);
        }
    }

    internal void PlayerDied()
    {
        foreach (Transform castle in EnemyCastles.transform)
        {
            Destroy(castle.gameObject);
        }

        createCastle(InitialCastlePosition.transform.position, false);
        PresentationCamera.SetActive(true);
        Player.SetActive(false);
        MainCanvas.transform.Find("GameCanvas").gameObject.SetActive(false);
        MainCanvas.transform.Find("MainMenu").gameObject.SetActive(true);
        MainCanvas.transform.Find("imgMainMenuBg").gameObject.SetActive(true);
        MainCanvas.transform.Find("MainMenu").transform.Find("lblLost").gameObject.SetActive(true);
        CurrentState = ApplicationState.MainMenu;
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
        SciFiFireProjectile sciFiFireProjectile = Player.transform.Find("SciFiFireProjectile").gameObject.GetComponent<SciFiFireProjectile>();
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
        //wait for the current castle to die and then check if there are any left
        Invoke("checkEnemiesLeft", 3);
    }

    private void checkEnemiesLeft()
    {
        if (EnemyCastles.transform.childCount == 0)
        {
            PlayerWon();
        }
    }

    private void PlayerWon()
    {
        createCastle(InitialCastlePosition.transform.position, false);
        PresentationCamera.SetActive(true);
        MainCanvas.transform.Find("GameCanvas").gameObject.SetActive(false);
        Player.SetActive(false);
        MainCanvas.transform.Find("MainMenu").gameObject.SetActive(true);
        MainCanvas.transform.Find("imgMainMenuBg").gameObject.SetActive(true);
        MainCanvas.transform.Find("MainMenu").transform.Find("lblLost").gameObject.SetActive(false);
        MainCanvas.transform.Find("MainMenu").transform.Find("lblWon").gameObject.SetActive(true);
        CurrentState = ApplicationState.MainMenu;

    }

    public enum ApplicationState
    {
        InGame, MainMenu
    }
}
