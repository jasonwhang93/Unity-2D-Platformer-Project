using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance; // ����� ������ �ν��Ͻ�

    public AudioSource musicSource; // ��� ������ ����� ����� �ҽ�

    private void Awake()
    {
        // Check if another instance of AudioManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Set reference to musicSource
        musicSource = GetComponent<AudioSource>();
    }


    // ��� ���� ��� �Լ�
    public void PlayMusic(AudioClip music)
    {
        musicSource.clip = music;
        musicSource.Play();
    }

    // ��� ���� ���� �Լ�
    public void StopMusic()
    {
        musicSource.Stop();
    }
}
