// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.Xml.Serialization;
// using UnityEngine;
// using UnityEngine.UI;

// namespace Systems.SceneManagement{

//     public class SceneLoader: MonoBehaviour{
//         //ESTE DE AQUI SE PUEDE UTILIZAR PARA LA BARRA DE ESTRES
//         [SerializeField] Image loadingBar; 
//         [SerializeField] float fillSpeed = 0.5f; 
//         [SerializeField] Canvas loadingCanvas; 
//         [SerializeField] Camera loadingCamera; 
//         [SerializeField] SceneGroup[] sceneGroups;  

//         float targetProgress;
//         bool isLoading;

//         public readonly SceneGroupManager manager = new SceneGroupManager();

//         void Awake()
//         {
//             manager.OnSceneLodaded += sceneName => Debug.Log("Loaded: " + sceneName);
//             manager.OnSceneUnLodaded += sceneName => Debug.Log("Unloaded: " + sceneName);
//             manager.OnSceneGroupLodaded += () => Debug.Log("Scene group loaded");
//         }

//         async void Start(){

//             await LoadSceneGroup(0);
//         }

//         void Update()
//         {
//             if(!isLoading) return;

//             float currentFillAmount = loadingBar.fillAmount;
//             float progressDifference = Mathf.Abs(currentFillAmount - targetProgress);
//             //RETOCAR ESTO LUEGO, NO INTERESA TENER EN CUENTA LA CARGA+++++++++

//             float dynamicFillSpeed = progressDifference * fillSpeed;
//             loadingBar.fillAmount = Mathf.Lerp(currentFillAmount, targetProgress, Time.deltaTime*dynamicFillSpeed);
//         }

//         public async Task LoadSceneGroup(int index){

//             //Aqui no vamos a mostrar la escena, solo la barra
//             loadingBar.fillAmount = 0f;
//             targetProgress = 1f;

//             if(index < 0 || index >= sceneGroups.Length){

//                 Debug.LogError("Invalid scene group index: " + index );
//                 return; 
//             }

//             LoadingProgress progress = new LoadingProgress();
//             progress.Progressed += target => targetProgress = Mathf.Max(target,targetProgress);
//             //ESTA ULTIMA LINEA HACE QUE LA BARRA PILLE EL Q SEA MAS GRANDE, SI LA CARGA DE LA VENTANA O LO Q LE HEMOS PUESTO
//             //IMPORTANTE:::: RETOCAR SI VA A SER PARA EL ESTRÉS

//             EnableLoadingCanvas();
//             await manager.LoadScenes(sceneGroups[index],progress);
//             EnableLoadingCanvas(false);
//         }

//         void EnableLoadingCanvas(bool enable = true){

//             isLoading = enable;
//             loadingCanvas.gameObject.SetActive(enable);
//             loadingCamera.gameObject.SetActive(enable);


//         }
//     }

//     public class LoadingProgress : IProgress<float>{

//         public event Action<float> Progressed;

//         const float ratio = 1f;

//         public void Report(float value){

//             Progressed?. Invoke(value / ratio);
//         }

//     }
// }