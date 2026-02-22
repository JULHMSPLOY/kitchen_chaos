using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance { get; private set; }
    public float SFX = 1f;
    public float Music = 1f;
    private AudioSource musicAudioSource;

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private AudioMixer audioMixer;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
            return;
        }

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicAudioSource.spatialBlend = 0f;
        musicAudioSource.volume = 1f;

        musicAudioSource.clip = audioClipRefsSO.music;
        musicAudioSource.Play();

        SetMusic(1f);
        SetSFX(1f);
    }

    public AudioClipRefsSO GetAudioClipRefsSO() {
        return audioClipRefsSO;
    }

    public void PlaySound(AudioClip[] clipArray, Vector3 position, float volume = 1f)
    {
        if (clipArray == null || clipArray.Length == 0) return;

        PlaySound(
            clipArray[Random.Range(0, clipArray.Length)],
            position,
            volume
        );
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f) {
        if (clip == null) return;

        GameObject soundObject = new GameObject("Sound");
        soundObject.transform.position = position;

        AudioSource audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume * SFX;
        audioSource.spatialBlend = 0f; // 2D sound 

        audioSource.outputAudioMixerGroup = 
        audioMixer.FindMatchingGroups("SFX")[0];

        audioSource.Play();

        Destroy(soundObject, clip.length);
    }

    public void SetMusic(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
    }

    public void SetSFX(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f);
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
    }

    public AudioMixerGroup GetMixerGroup(string groupName)
    {
        return audioMixer.FindMatchingGroups(groupName)[0];
    }
}
