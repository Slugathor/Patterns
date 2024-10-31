using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementSystem : MonoBehaviour
{
    int totalCoinsCollected = 0;

    private void Start()
    {
        CoinManager.instance.CoinCollected += UpdateCoinsCollected;
    }
    void UpdateCoinsCollected()
    {
        totalCoinsCollected++;

        // if achi not completed, complete it
    }

}
