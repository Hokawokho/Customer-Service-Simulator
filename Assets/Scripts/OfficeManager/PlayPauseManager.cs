using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayPauseManager : MonoBehaviour
{
    [Tooltip("Añadir aqui GameObjects que sigan siendo interactuables con el menú"
        +  "de pausa abierto. No tocar \"State Before Pause\".")]
    [SerializeField] private List<UnpausableObject> extraObjectsToPause;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            TogglePause();
        }
    }

    public void TogglePause() {
        // Pausar/reanudar juego.
        GeneralManager.Instance.pause = !GeneralManager.Instance.pause;
        bool isGamePaused = GeneralManager.Instance.pause;

        // Comprobar si hay items extra que desactivar
        if (extraObjectsToPause != null) {
            foreach (UnpausableObject unObj in extraObjectsToPause) {
                GameObject obj = unObj.objectToPause;
                if (isGamePaused) unObj.activeBeforePause = obj.activeSelf;
                    
                // Si el objeto es fatherScript, usar su función específica.
                fatherScript fScr = obj.GetComponent<fatherScript>();
                if (fScr != null) {
                    if (isGamePaused || !unObj.activeBeforePause) fScr.freezeSons();
                    else if (unObj.activeBeforePause) fScr.unfreezeSons();
                } else {
                    if (isGamePaused) obj.SetActive(false);
                    else obj.SetActive(unObj.activeBeforePause);
                }
            }
        }
    }

}

[Serializable]
public class UnpausableObject {
    public GameObject objectToPause;
    public bool activeBeforePause;
}
