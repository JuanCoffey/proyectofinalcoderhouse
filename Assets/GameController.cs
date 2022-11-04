using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance;
    public GameObject EnemyCastles;

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
        enemyCastle.transform.position = new Vector3(enemyCastle.transform.position.x, 32, enemyCastle.transform.position.z) ;

        Invoke("CreateEnemyCastle", 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    internal void EnemyDied()
    {



    }
}
