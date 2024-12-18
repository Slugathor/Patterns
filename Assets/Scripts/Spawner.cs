using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject coinPrefab;
    [SerializeField] GameObject platCoinPrefab;
    [SerializeField] int goldCoinAmount = 100;
    [SerializeField] int platCoinAmount = 30;
    // Start is called before the first frame update
    void Start()
    {
        Spawn(coinPrefab, goldCoinAmount);
        Spawn(platCoinPrefab, platCoinAmount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Comma)) 
        {
            Spawn(coinPrefab, goldCoinAmount);
            Spawn(platCoinPrefab, platCoinAmount);
        }
    }

    void Spawn(GameObject prefab, int amountToSpawn)
    {
        for (int i = 0; i < amountToSpawn; i++)
        {
            Vector3 coords = new Vector3(UnityEngine.Random.Range(-47, 43), -12.66f, UnityEngine.Random.Range(-49, 21));
            Instantiate(prefab, coords, prefab.transform.rotation);
        }
    }
}
