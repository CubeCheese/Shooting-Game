using UnityEngine;

//Play Sfx Sound
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public enum AudioChannel { Master, Sfx, Music };

    public float masterVolumePercent { get; private set; }
    public float sfxVolumePercent { get; private set; }
    public float musicVolumePercent { get; private set; }

    private AudioSource sfx2DSource;
    private AudioSource[] musicSources;
    private int currentMusicIndex;

    private Transform audioListener;

    private SoundLibrary library;

    private void Awake()
    {
        if (instance != null) { Destroy(gameObject); }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            library = GetComponent<SoundLibrary>();

            musicSources = new AudioSource[2];

            for (int i = 0; i < 2; i++)
            {
                GameObject newMusicSource = new GameObject("Music source " + (i + 1));
                musicSources[i] = newMusicSource.AddComponent<AudioSource>();
                newMusicSource.transform.parent = transform;
            }

            GameObject newSfx2Dsource = new GameObject("2D Sfx Source");
            sfx2DSource = newSfx2Dsource.AddComponent<AudioSource>();
            newSfx2Dsource.transform.parent = transform;

            audioListener = GetComponentInChildren<AudioListener>().transform;

            masterVolumePercent = PlayerPrefs.GetFloat("master vol", 1);
            sfxVolumePercent = PlayerPrefs.GetFloat("sfx vol", 1);
            musicVolumePercent = PlayerPrefs.GetFloat("music vol", 1);
        }
    }

    //Set Volume
    public void SetVolume(float _volumePercent, AudioChannel _channel)
    {
        switch (_channel)
        {
            case AudioChannel.Master:
                masterVolumePercent = _volumePercent;
                break;
            case AudioChannel.Sfx:
                sfxVolumePercent = _volumePercent;
                break;
            case AudioChannel.Music:
                musicVolumePercent = _volumePercent;
                break;
        }

        musicSources[0].volume = musicVolumePercent * masterVolumePercent;
        for (int index = 1; index < musicSources.Length; index++)
            musicSources[index].volume = musicVolumePercent * masterVolumePercent;

        PlayerPrefs.SetFloat("master vol", masterVolumePercent);
        PlayerPrefs.SetFloat("sfx vol", sfxVolumePercent);
        PlayerPrefs.SetFloat("music vol", musicVolumePercent);
        PlayerPrefs.Save();
    }
    
    //Play Thema Music
    public void PlayMusic(AudioClip _clip, int _currentMusicIndex)
    {
        currentMusicIndex = _currentMusicIndex;
        musicSources[currentMusicIndex].clip = _clip;
        musicSources[currentMusicIndex].Play();
    }

    //Stop Thema Music
    public void StopMusic() { musicSources[currentMusicIndex].Stop(); }

    //Play Sfx Sound
    public void PlaySound(string _soundName) { sfx2DSource.PlayOneShot(library.GetClipFromName(_soundName), sfxVolumePercent * masterVolumePercent); }
}