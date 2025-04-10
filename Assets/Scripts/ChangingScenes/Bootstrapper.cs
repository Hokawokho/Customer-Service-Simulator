// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.SceneManagement;
// using UnityEngine;
// using UnityUtils;

// public class Bootstrapper : PersistentSingleton<Bootstrapper>
// {
//     [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]

//     static async void Init(){

//         Debug.Log("Bootstrapper...");
//         await SceneManager.LoadSceneAsync("Bootstrapper", LoadSceneMode.Single).AsTask();
//     }
// }
