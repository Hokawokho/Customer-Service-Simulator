// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.SceneManagement;
// using System.Linq;
// using UnityEngine;
// using System.Threading.Tasks;

// namespace Systems.SceneManagement{

//     public class SceneGroupManager{
        
//         public event Action<string> OnSceneLodaded = delegate{ };
//         public event Action<string> OnSceneUnLodaded = delegate{ };
//         public event Action OnSceneGroupLodaded = delegate{ };
    
//         SceneGroup ActiveSceneGroup;

//         public async Task LoadScenes(SceneGroup group, IProgress<float> progress, bool reloadDupScenes= false){

//             ActiveSceneGroup = group;
//             var loadedScenes = new List<string>();

//             int sceneCount = SceneManager.sceneCount;

//             for (var i = 0; i < sceneCount; i++){

//                 loadedScenes.Add(SceneManager.GetSceneAt(i).name);
//             }

//             var totalScenesToLoad = ActiveSceneGroup.Scenes.Count;

//             var operationGroup = new AsyncOperationGroup(totalScenesToLoad);
            
//             for (var i = 0; i < totalScenesToLoad; i++){

//                 var sceneData = group.Scenes[i];
//                 if(reloadDupScenes == false && loadedScenes.Contains(sceneData.Name)) continue;

//                 var operation = SceneManager.LoadSceneAsync(sceneData.Reference.Path, LoadSceneMode.Additive);

//                 operationGroup.Operations.Add(operation);

//                 OnSceneLodaded.Invoke(sceneData.Name);
//             }

//             //Espera a que todas las AsyncOperations en el grupo acaben
//             while(!operationGroup.isDone){

//                 progress?.Report(operationGroup.Progress);
//                 //RETOCAR EL AWAIT A FUTURO, LO DEJO POR AHORA IGUAL QUE EL TUTO
//                 await Task.Delay(100);

//             }
//             Scene activeScene = SceneManager.GetSceneByName(ActiveSceneGroup.FindSceneNameByType(SceneType.ActiveScene));

//             if (activeScene.IsValid()){

//                 SceneManager.SetActiveScene(activeScene);
//             }
        
//             OnSceneGroupLodaded.Invoke();
//         }




//         public async Task UnloadScenes(){

//             var scenes = new List<string>();
//             var activeScene = SceneManager.GetActiveScene().name;

//             int sceneCount = SceneManager.sceneCount;

//             for(var i = sceneCount - 1; i >0 ; i--){

//                 var sceneAt = SceneManager.GetSceneAt(i);
//                 if(!sceneAt.isLoaded) continue;

//                 var sceneName = sceneAt.name;
//                 //if(sceneName.Equals(activeScene) || sceneName == "nombreEscenaPrincipal") continue;
//                 if(sceneName.Equals(activeScene) || sceneName == "Bootstrapper") continue;
//                 scenes.Add(sceneName);

//             }

//             var operationGroup = new AsyncOperationGroup(scenes.Count);

//             foreach(var scene in scenes){

//                 var operation = SceneManager.UnloadSceneAsync(scene);
//                 if(operation == null) continue;

//                 operationGroup.Operations.Add(operation);

//                 OnSceneUnLodaded.Invoke(scene);
//             }
//             //Espera a que todas las AsyncOperations en el grupo acaben
//             while(!operationGroup.isDone){

//                 await Task.Delay(100);

//             }

//         }

        
//     }

//     public readonly struct AsyncOperationGroup{

//         public readonly List<AsyncOperation> Operations;

//         public float Progress => Operations.Count == 0 ? 0: Operations.Average(o => o.progress);
//         public bool isDone => Operations.All(o => o.isDone);

//         public AsyncOperationGroup(int initialCapacity){

//             Operations = new List<AsyncOperation>(initialCapacity);
//         }
//     }
// }