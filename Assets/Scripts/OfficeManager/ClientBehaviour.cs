using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClientBehaviour : MonoBehaviour
{
    [Tooltip("Animacion a reproducir al spawnear")]
    public AnimationClip animSpawn;
    [Tooltip("Animacion a reproducir al finalizar")]
    public AnimationClip animEnd;
    [Tooltip("Animar texto a usar")]
    public AnimationClip animTextP;
    public AnimationClip animTextC;

    [Tooltip("Campo de texto a usar")]
    public TextMeshPro textField;

    private Animator animator;
    private ClientManager clientManager;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("No Animator found on " + gameObject.name);
        }
        else
        {
            animator.Play(animSpawn.name);
        }
        clientManager = FindObjectOfType<ClientManager>();
        
    }

    //Funcion a la que se llama al finalizar el acercamiento, para que el cliente genere una tarea
    public void provideTask()
    {
        //Aqui se inicia la barra de frustracion del cliente
        StressBar stressBar = FindObjectOfType<StressBar>();
        if (stressBar != null)
        {
            stressBar.StartCharging();
        }
        else
        {
            Debug.LogWarning("StressBar not found in the scene.");
        }
        
        //Aqui se genera una tarea aleatoria para el cliente
        int idx = Random.Range(0, 2);
        switch (idx)
        {
            case 0:
                //Generar tarea de imprimir
                Debug.Log("Cliente " + gameObject.name + " ha generado una tarea de imprimir.");
                CompareDocsManager comp = FindObjectOfType<CompareDocsManager>();

                animator.Play(animTextP.name);
                if (comp != null)
                {
                    comp.NewTask();
                }
                else
                {
                    Debug.LogWarning("CompareDocsManager not found in the scene.");
                }
                break;
            case 1:
                //Generar tarea de comparar
                Debug.Log("Cliente " + gameObject.name + " ha generado una tarea de comparar.");
                PrintManager print = FindObjectOfType<PrintManager>();

                animator.Play(animTextC.name);
                if (print != null)
                {
                    print.NewTask();
                }
                else
                {
                    Debug.LogWarning("PrintManager not found in the scene.");
                }
                break;
            default:
                //No hacer nada
                Debug.Log("Cliente " + gameObject.name + " no ha generado ninguna tarea.");
                break;
        }
                
    }

    //Funcion a llamar una vez se haya terminado la tarea
    public void finishTask(int success)
    {
        //Se contabiliza el resultado de la tarea
        if (success == 1)
        {
            Debug.Log("Cliente " + gameObject.name + " ha terminado la tarea con exito.");
            if(clientManager != null)
            {
                clientManager.TaskCompleted();
            }
            else
            {
                Debug.LogWarning("ClientManager not found in the scene.");
            }
            
            DistractionManager distractionManager = FindObjectOfType<DistractionManager>();
            if (distractionManager != null)
            {
                distractionManager.ResetStressBarAfterDelay(3f);
            }
            else
            {
                Debug.LogWarning("DistractionManager not found in the scene.");
            }
        }
        else if (success == 0)
        {
            Debug.Log("Cliente " + gameObject.name + " ha terminado la tarea sin exito.");
            if(clientManager != null)
            {
                clientManager.TaskFailed();
            }
            else
            {
                Debug.LogWarning("ClientManager not found in the scene.");
            }
        }
        else
        {
            Debug.Log("Cliente " + gameObject.name + " no ha terminado la tarea.");
        }

        //Aqui se finaliza la tarea del cliente
        if (animEnd != null && animator != null)
        {
            animator.Play(animEnd.name);
        }
        else
        {
            Debug.LogWarning("No Animation or Animator found on " + gameObject.name);
        }
    }

    //Funcion a llamar al finalizar la animacion de "salida"
    public void despawnClient()
    {
        //Aqui se destruye el cliente
        Destroy(gameObject);
    }

    public void printPrint()
    {
        if (textField != null)
        {
            textField.text = "Imprime porfa";
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI found on " + gameObject.name);
        }
    }

    public void printCompare()
    {
        if(textField != null)
        {
            textField.text = "Compara porfa";
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI found on " + gameObject.name);
        }
    }
    public void printNothing()
    {
        if(textField != null)
        {
            textField.text = "";
        }
        else
        {
            Debug.LogWarning("No TextMeshProUGUI found on " + gameObject.name);
        }
    }
}
