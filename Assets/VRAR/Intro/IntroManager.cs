using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public Button playButton;
    public Button exitButton;
    public Button noButton;
    public Image exitImg;

    public void NextScene()
    {
        SkillManager.activatedCount = 0;    // Ω∫≈≥ƒ≠ ¿Œµ¶Ω∫ √ ±‚»≠
        playButton.GetComponent<AudioSource>().Play();
        Invoke("LoadMainScene", 1f);
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void OpenExitFrame()
    {
        exitButton.GetComponent<AudioSource>().Play();
        exitImg.gameObject.SetActive(true);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void NoExit()
    {
        noButton.GetComponent<AudioSource>().Play();
        exitImg.gameObject.SetActive(false);
    }
}
