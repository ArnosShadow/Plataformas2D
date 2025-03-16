using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;

    [Header("Sonidos del Juego")]
    [SerializeField] private AudioClip sonidoMoneda;
    [SerializeField] private AudioClip sonidoAtaque;
    [SerializeField] private AudioClip sonidoMuerte;

    private static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static AudioManager Instance
    {
        get => instance;
    }

    public void PlaySoundSFX(AudioClip clip)
    {
        if (clip != null)
        {
            sfxAudioSource.PlayOneShot(clip);
        }
    }

    public void PlayMonedaSound()
    {
        PlaySoundSFX(sonidoMoneda);
    }

    public void PlayAtaqueSound()
    {
        PlaySoundSFX(sonidoAtaque);
    }

    public void PlayMuerteSound()
    {
        PlaySoundSFX(sonidoMuerte);
    }
}
