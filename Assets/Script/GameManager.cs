using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject audioPanel;
    public AudioSource lagu;
    // Start is called before the first frame update
    void Start()
    {
        audioPanel.gameObject.SetActive(false);
        lagu.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GantiScene(String namaScane) {
        SceneManager.LoadScene(namaScane);
    }

    public void ExitGame() {
        Application.Quit();
    }


    public void AudioSetting() {
        audioPanel.gameObject.SetActive(true);
        Time.timeScale = 0;
    }

    public void ExitAudioSetting() {
        audioPanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }



}
