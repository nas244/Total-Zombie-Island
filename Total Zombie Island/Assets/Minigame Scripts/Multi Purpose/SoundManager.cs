using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public enum Sound { Shot, Music, Hurt, Start, Finished, Dead, Countdown, Headshot, Beep, Win, Fail, Reload, Misc }

    public static void PlaySound(Sound sound)
    {
        GameObject soundGameObject = new GameObject("Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
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

        /*foreach (GameHandler.SoundAudioClip soundAudioClip in GameHandler.instance.soundAudioClipArray)
        {
            if (soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }
        }
        Debug.LogError("Sound" + sound + " not found!");
        return null;*/
    }
}
