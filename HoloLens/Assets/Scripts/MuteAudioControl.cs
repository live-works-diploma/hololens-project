using UnityEngine;

public class MuteAudioControl : MonoBehaviour
{
    void Start()
    {
        AudioListener.volume = 1.0f;
    }

    public void ToggleMute(bool muteAudio) => AudioListener.volume = muteAudio ? 0f : 1.0f;
}
