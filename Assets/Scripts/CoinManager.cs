using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CoinManager : GenericSingleton<CoinManager>
{
    public event Action<int> CoinCollected;

    public void CollectCoin(int coinValue)
    {
        CoinCollected?.Invoke(coinValue);
    }
}
