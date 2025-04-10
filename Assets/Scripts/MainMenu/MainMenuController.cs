using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private AudioClip mainMenuTheme;
    [SerializeField] private Animator creditsPanelAnimator;
    [SerializeField] private GameObject closeBttn;

    private bool _creditsMoving;

    void Start()
    {
        if (mainMenuTheme != null) GeneralManager.Instance.audioManager.PlayMusic(mainMenuTheme);
    }

    public void StartGame() {
        GeneralManager.Instance.GoToMainGameScene();
    }

    public void Credits() {
        if (_creditsMoving) return; 
        _creditsMoving = true;

        creditsPanelAnimator.SetBool("activated", true);
        StartCoroutine(WaitForCreditsPanel(true));
    }

    private IEnumerator WaitForCreditsPanel(bool openingPanel) {
        int animIndex = openingPanel ? 1 : 3;
        float waitTime = creditsPanelAnimator.runtimeAnimatorController.animationClips[animIndex].length;
        Debug.Log(waitTime);
        yield return new WaitForSeconds(waitTime);

        if (openingPanel) closeBttn.SetActive(true);
        _creditsMoving = false;
    }

    public void CreditsClose() {
        if (_creditsMoving) return;
        _creditsMoving = true;

        closeBttn.SetActive(false);
        creditsPanelAnimator.SetBool("activated", false);
        StartCoroutine(WaitForCreditsPanel(false));
    }

    public void QuitGame() {
        GeneralManager.Instance.Quit();
    }
}
