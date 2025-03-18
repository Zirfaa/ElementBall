using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer Audio;
    public Slider Musik;
    public Slider gameMusik;
    public AudioSource lagu;
    // Start is called before the first frame update
    void Start()
    {
        //lagu.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setVolumeMusik() {
        Audio.SetFloat("vol", Musik.value);
    }

    public void setVolumeGameMusik() {
        Audio.SetFloat("vol", gameMusik.value);
    }


}
