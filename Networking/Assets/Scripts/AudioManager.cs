using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public enum AudioChannel { Master, SFX, Music };

    public float masterVolumePercent = 0.25f;
    public float sfxVolumePercent = 1;
    public float musicVolumePercent = 1;

    AudioSource sfx2DSource;
    AudioSource[] aSources;
    int activeAudioSourceIndex;

    public static AudioManager instance;

    private Transform audiolistener;
    private Transform playerTransform;

    SoundLibrary library;

    void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();
            aSources = new AudioSource[2];
            for (int i = 0; i < 2; i++)
            {
                GameObject newAudioSource = new GameObject("Audio source " + (i + 1));
                aSources[i] = newAudioSource.AddComponent<AudioSource>();
                newAudioSource.transform.parent = transform;
            }

            GameObject newSfx2DSource = new GameObject("2D Sfx source ");
            sfx2DSource = newSfx2DSource.AddComponent<AudioSource>();
            newSfx2DSource.transform.parent = transform;

            audiolistener = FindObjectOfType<AudioListener>().transform;
            playerTransform = FindObjectOfType<Player>().transform;

            masterVolumePercent = PlayerPrefs.GetFloat("Master volume", masterVolumePercent);
            sfxVolumePercent = PlayerPrefs.GetFloat("SFX volume", sfxVolumePercent);
            musicVolumePercent = PlayerPrefs.GetFloat("Music volume", musicVolumePercent);
        }
    }

    void Update()
    {
        if (playerTransform != null)
            audiolistener.position = playerTransform.position;
    }

    public void PlayMusic(AudioClip clip, bool loop, float fadeDuration = 1) {
        activeAudioSourceIndex = 1 - activeAudioSourceIndex;
        aSources[activeAudioSourceIndex].clip = clip;
        aSources[activeAudioSourceIndex].Play();
        aSources[activeAudioSourceIndex].loop = loop;

        StartCoroutine(AnimateAudioCrossfade(fadeDuration));
    }

    public void SetVolume(float volumePercent, AudioChannel channel) {
        switch (channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = volumePercent;
                break;
            case AudioChannel.SFX:
                sfxVolumePercent = volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = volumePercent;
                break;
        }

        aSources[0].volume = musicVolumePercent * masterVolumePercent;
        aSources[1].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("Master volume", masterVolumePercent);
        PlayerPrefs.SetFloat("SFX volume", sfxVolumePercent);
        PlayerPrefs.SetFloat("Music volume", musicVolumePercent);
    }

    public void PlaySound(AudioClip clip, Vector3 pos) {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, pos, sfxVolumePercent * masterVolumePercent);
    }

    public void PlaySound(string name, Vector3 pos) {
        PlaySound(library.GetClipFromName(name), pos);
    }

    public void PlaySound2D(string name) {
        sfx2DSource.PlayOneShot(library.GetClipFromName(name), sfxVolumePercent * masterVolumePercent);
    }

    public bool isPlaying() { return aSources[activeAudioSourceIndex].isPlaying; }

    IEnumerator AnimateAudioCrossfade(float duration) {
        float percent = 0;

        while (percent < 1) {
            percent += Time.deltaTime * 1 / duration;
            //Fade Sound In
            aSources[activeAudioSourceIndex].volume = Mathf.Lerp(0, musicVolumePercent * masterVolumePercent, percent);
            //Fade Sound out
            aSources[1 - activeAudioSourceIndex].volume = Mathf.Lerp(musicVolumePercent * masterVolumePercent, 0, percent);

            yield return null;
        }
    }
}