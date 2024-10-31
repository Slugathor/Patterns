using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public event Action CoinCollected;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else Destroy(this);
    }

    public void CollectCoin()
    {
        CoinCollected?.Invoke();
    }
}