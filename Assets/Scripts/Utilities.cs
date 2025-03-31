using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Utilities : MonoBehaviour
{
    public static void Shuffle<T> (List<T> list) {
        for (int i = 0; i < list.Count; i++) {
            int r = Random.Range(i, list.Count);
            T tmp = list[i];
            list [i] = list[r];
            list[r] = tmp;
        }
    }

    public static int[] GetRandomIndexes<T> (List<T> list) {
        List<int> indexes = Enumerable.Range(0, list.Count).ToList();
        Shuffle<int>(indexes);
        return indexes.ToArray();
    }
}
