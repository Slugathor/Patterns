using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using static HelperFunctions;

public class AchievementSystem : GenericSingleton<AchievementSystem>
{
    private readonly List<Achievement> achievements = new List<Achievement>();
    // creates a public readonly version Achievements, which only has a getter that returns achievements
    public IReadOnlyList<Achievement> Achievements => achievements;

    /// <summary>
    /// String represents a prefix to the achievement name and int the amount of coins to collect.
    /// The rest of the achievement name is Coin Collector.
    /// </summary>
    private List<(string, int)> CoinAchievements = new List<(string,int)>() {
        ("Beginner ", 10),
        ("Novice ",25),
        ("Aspiring ",50),
        ("Dedicated ",100),
        ("Insane ",500),
        ("Diabolical ",666),
        ("Unbelievable ", 10000000)
    };
    /// <summary>
    /// String represents a prefix to the achievement name and float the distance required to travel.
    /// The rest of the achievement name is Traveller.
    /// </summary>
    private List<(string, float)> DistanceWalkedAchievements = new List<(string,float)>() {
        ("Newbie ", 10),
        ("Sunday ",25),
        ("Inspired ",50),
        ("Sore-foot ",100),
        ("Tireless ",1000),
        ("Demonic ",666.666f) 
    };



    [SerializeField]private int totalCoinsCollected;
    [SerializeField] internal AudioClip achievementUnlockSound;
    [SerializeField] private float distanceWalked;

    private void OnDisable()
    {
        Dispose();
    }

    private void Start()
    {
        CoinManager.instance.CoinCollected += UpdateCoinsCollected;
        PlayerController.PlayerMoved += OnPlayerMoved;

        // create all coin achievements
        foreach ((string,int) tuple in CoinAchievements)
        {
            CreateCoinCollectingAchievement(tuple.Item1, totalCoinsCollected, tuple.Item2);
        }
        
        // create all distance walked achievements
        foreach ((string,float) tuple in DistanceWalkedAchievements)
        {
            CreateDistanceWalkedAchievement(tuple.Item1, distanceWalked, tuple.Item2);
        }


    }
    void UpdateCoinsCollected(int coinValue)
    {
        totalCoinsCollected += coinValue; // don't confuse this to be an event subscription event, it's just incrementing an int
    }
    void OnPlayerMoved(float dist)
    {
        distanceWalked += dist;
    }
  

    public class Achievement
    {
        public string name;
        public bool complete { get; private set; } = false;
        public string achievementType; // "coinAchievement" is an example of a type, it means that the achievement's progress is tracked as coins collected

        public Achievement(string _name, string _achievementType)
        {
            this.name = _name;
            this.achievementType = _achievementType;
        }
    
        protected void Complete()
        {
            complete = true;
            
            string additionalLine = this.achievementType switch
            {
                "coinAchievement" => $"Collected {((CountableAchievement<int>)this).target} coins.\n",
                "distanceWalkedAchievement" => $"Travelled {((CountableAchievement<float>)this).target} metres by foot.\n",
                _ => ""
            };

            Debug.Log("You've earned the achievement: " + this.name + "\n"+additionalLine);
            AudioManager.instance.GetComponent<AudioSource>().PlayOneShot(instance.achievementUnlockSound);
        }
    }
    /// <summary>
    /// Achievements that require some numeric value to be reached in order to unlock. Such as collect 10 coins, kill 50 enemies or walk 100 000km.
    /// </summary>
    /// <typeparam name="T">has to be IComparable and a numeric.</typeparam>
    internal class CountableAchievement<T> : Achievement where T : IComparable<T>, IEquatable<T>, IConvertible
    {
        public T value; // numeric type current value
        public T target; // numeric type target to earn achievement

        /// <summary>
        /// Create a new Countable achievement.
        /// </summary>
        /// <param name="_name">Achievement name.</param>
        /// <param name="_achievementType">Achievement type as a string.</param>
        /// <param name="_value">Current progress (e.g. coins collected so far etc.) </param>
        /// <param name="_target">Target value to reach.</param>
        public CountableAchievement(string _name, string _achievementType, T _value, T _target) : base(_name, _achievementType)
        {
            this.value = _value;
            this.target = _target;
        }

        internal void IncrementBy()
        {
            // Provide a default value of `1` for numeric types
            IncrementBy((T)(object)1);
        }
        internal void IncrementBy(T amount)
        {
            // checks if value and amount are numerics
            if (IsSupportedNumeric(value) && IsSupportedNumeric(amount))
            {
                // int
                if (typeof(T) == typeof(int))
                {
                    value = (T)(object)((int)(object)value + (int)(object)amount);
                }
                #region Same code for all other numeric data types
                // uint
                else if (typeof(T) == typeof(uint))
                {
                    value = (T)(object)((uint)(object)value + (uint)(object)amount);
                }
                // long
                else if (typeof(T) == typeof(long))
                {
                    value = (T)(object)((long)(object)value + (long)(object)amount);
                }
                // ulong
                else if (typeof(T) == typeof(ulong))
                {
                    value = (T)(object)((ulong)(object)value + (ulong)(object)amount);
                }
                // float
                else if (typeof(T) == typeof(float))
                {
                    value = (T)(object)((float)(object)value + (float)(object)amount);
                }
                // double
                else if (typeof(T) == typeof(double))
                {
                    value = (T)(object)((double)(object)value + (double)(object)amount);
                }
                // BigInteger
                else if (typeof(T) == typeof(BigInteger))
                {
                    value = (T)(object)((BigInteger)(object)value + (BigInteger)(object)amount);
                }
                #endregion


                if (value.CompareTo(target) >= 0 && !complete)
                {
                    Complete();
                }
            }
            //Debug.Log(this.name+" is at value: "+value.ToString());
        }
    }

    private void CreateCoinCollectingAchievement(string _namePrefix, int _currentCoins, int _targetCoins)
    {
        string achType = "coinAchievement";
        
        var coinAch = new CountableAchievement<int>(_namePrefix + "Coin Collector", achType, _currentCoins, _targetCoins);
        achievements.Add(coinAch);
        CoinManager.instance.CoinCollected += coinAch.IncrementBy;
    }
    private void CreateDistanceWalkedAchievement(string _namePrefix, float _currentDist, float _targetDist)
    {
        string achType = "distanceWalkedAchievement";
        
        var distAch = new CountableAchievement<float>(_namePrefix + "Traveller", achType, _currentDist, _targetDist);
        achievements.Add(distAch);
        PlayerController.PlayerMoved += distAch.IncrementBy;
    }

    // Had to add this to unsub from the above ^^
    public void Dispose() // Unsubscribe from collect events to prevent multiple achievements being tracked
    {
        foreach (var ach in achievements)
        {
            switch (ach)
            {
                case CountableAchievement<int> countableInt:
                    CoinManager.instance.CoinCollected -= countableInt.IncrementBy;
                    break;
                case CountableAchievement<float> countableFloat:
                    PlayerController.PlayerMoved -= countableFloat.IncrementBy;
                    break;

                case CountableAchievement<uint> countableUint:
                    
                    break;
                case CountableAchievement<double> countableDouble:
                    
                    break;
                    // Add other types here as needed
            }
        }
    }
}
