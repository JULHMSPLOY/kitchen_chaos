using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;
    [SerializeField] private AudioMixer audioMixer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    // ======================================================
    // 🔹 เล่นเสียงแบบ Clip เดี่ยว
    // ======================================================
    public void PlaySound(AudioClip clip, Vector3 position)
    {
        PlaySound(clip, position, 1f);
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume)
    {
        if (clip == null) return;

        AudioSource audioSource =
            new GameObject("Sound").AddComponent<AudioSource>();

        audioSource.transform.position = position;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.spatialBlend = 1f; // ทำให้เป็น 3D sound

        audioSource.outputAudioMixerGroup =
            GetMixerGroup("SFX");

        audioSource.Play();

        Destroy(audioSource.gameObject, clip.length);
    }

    // ======================================================
    // 🔹 เล่นเสียงแบบ Array (สุ่มในนี้)
    // ======================================================
    public void PlaySound(AudioClip[] clips, Vector3 position)
    {
        PlaySound(clips, position, 1f);
    }

    public void PlaySound(AudioClip[] clips, Vector3 position, float volume)
    {
        if (clips == null || clips.Length == 0) return;

        AudioClip randomClip =
            clips[Random.Range(0, clips.Length)];

        PlaySound(randomClip, position, volume);
    }

    // ======================================================
    // 🔹 Audio Mixer
    // ======================================================
    public AudioMixerGroup GetMixerGroup(string groupName)
    {
        AudioMixerGroup[] groups =
            audioMixer.FindMatchingGroups(groupName);

        if (groups.Length > 0)
            return groups[0];

        return null;
    }

    // ======================================================
    // 🔹 Getter
    // ======================================================
    public AudioClipRefsSO GetAudioClipRefsSO()
    {
        return audioClipRefsSO;
    }
}