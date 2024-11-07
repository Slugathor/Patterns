using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public static class HelperFunctions
{
    /// <summary>
    /// Checks if given <paramref name="value"/> is a supported numeric.
    /// </summary>
    /// <param name="value"></param>
    /// <returns>True if <paramref name="value"/> is <see cref="int"/>, <see cref="uint"/>, <see cref="long"/>, <see cref="ulong"/>, <see cref="float"/>, <see cref="double"/> or <see cref="BigInteger"/>.</returns>
    public static bool IsSupportedNumeric(object value)
    {
        return
            value is int
            || value is uint
            || value is long
            || value is ulong
            || value is float
            || value is double
            || value is BigInteger;
    }


    public static void PrintAchievements()
    {
        foreach (var ach in AchievementSystem.instance.Achievements)
        {
            string no = ach.complete ? "" : "Not ";
            Debug.Log(ach.name + " : " + no + "Completed.\n");
        }
    }
}
