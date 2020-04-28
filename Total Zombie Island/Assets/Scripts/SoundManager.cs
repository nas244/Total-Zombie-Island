using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public enum Sound { Shot, Music, Hurt, Start, Finished, Dead, Countdown, Headshot, Beep, Win, Fail, Reload, Misc }

    static SoundManager instance;

    private void Awake()
    {
        instance = this;
    }
    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    public static void FadeAudio(AudioSource audioSource, float duration, float targetVolume)
    {
        instance.StartCoroutine(StartFade(audioSource, duration, targetVolume));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        
        switch (SceneManager.GetActiveScene().name)
        {
            case "Sniper Minigame":
                foreach (GameHandler.SoundAudioClip soundAudioClip in GameHandler.instance.soundAudioClipArray)
                {
                    if (soundAudioClip.sound == sound)
                    {
                        return soundAudioClip.audioClip;
                    }
                }
                Debug.LogError("Sound" + sound + " not found!");
                return null;

            case "Boss Battle Minigame":
                foreach (BattleSystem.SoundAudioClip soundAudioClip in BattleSystem.instance.soundAudioClipArray)
                {
                    if (soundAudioClip.sound == sound)
                    {
                        return soundAudioClip.audioClip;
                    }
                }
                Debug.LogError("Sound" + sound + " not found!");
                return null;

            default:
                foreach (GameHandler.SoundAudioClip soundAudioClip in GameHandler.instance.soundAudioClipArray)
                {
                    if (soundAudioClip.sound == sound)
                    {
                        return soundAudioClip.audioClip;
                    }
                }
                Debug.LogError("Sound" + sound + " not found!");
                return null;
        }
    }

    public static IEnumerator StartFade(AudioSource AudioSource, float Duration, float TargetVolume)
    {
        float currentTime = 0;
        float start = AudioSource.volume;

        while (currentTime < Duration)
        {
            currentTime += Time.deltaTime;
            AudioSource.volume = Mathf.Lerp(start, TargetVolume, currentTime / Duration);
            yield return null;
        }
        yield break;
    }
}
