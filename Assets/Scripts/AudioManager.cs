using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : GenericSingleton<AudioManager>
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
