using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class CoinManager : GenericSingleton<CoinManager>
{
    public event Action<int> CoinCollected;
    [SerializeField] AudioClip coinCollectSound;

    public void CollectCoin(int coinValue)
    {
        CoinCollected?.Invoke(coinValue);
        AudioManager.instance.GetComponent<AudioSource>().PlayOneShot(coinCollectSound);
    }
}
