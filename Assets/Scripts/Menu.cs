using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Menu
public class Menu : MonoBehaviour
{
    public GameObject mainMenuHolder;
    public GameObject optionsMenuHolder;

    [Header("Volume Slider")]
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Resolution Toggles")]
    public Toggle[] resolutionToggles;
    public Toggle fullscreenToggle;
    public int[] screenWidths;
    private int activeScreenResIndex;

    private void Start()
    {
        activeScreenResIndex = PlayerPrefs.GetInt("screen res index");
        bool isFullscreen = (PlayerPrefs.GetInt("fullscreen") == 1) ? true : false;

        masterVolumeSlider.value = AudioManager.instance.masterVolumePercent;
        musicVolumeSlider.value = AudioManager.instance.musicVolumePercent;
        sfxVolumeSlider.value = AudioManager.instance.sfxVolumePercent;

        for (int i = 0; i < resolutionToggles.Length; i++) { resolutionToggles[i].isOn = i == activeScreenResIndex; }

        fullscreenToggle.isOn = isFullscreen;
    }

    //발표 편의상 게임 시작 시 레벨 저장 초기화
    public void Play() { SceneManager.LoadScene("Stage 1"); PlayerPrefs.SetInt("Level", 0); }

    public void Quit() { Application.Quit(); }

    //Option Menu
    public void OptionsMenu()
    {
        mainMenuHolder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }

    //Main Menu
    public void MainMenu()
    {
        mainMenuHolder.SetActive(true);
        optionsMenuHolder.SetActive(false);
    }

    //Screen Resolution
    public void SetScreenResolution(int i)
    {
        if (resolutionToggles[i].isOn)
        {
            activeScreenResIndex = i;
            float aspectRatio = 16 / 9f;
            Screen.SetResolution((int)(screenWidths[i] / aspectRatio), screenWidths[i], false);
            PlayerPrefs.SetInt("screen res index", activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetFullscreen(bool isFullscreen)
    {
        for (int i = 0; i < resolutionToggles.Length; i++) { resolutionToggles[i].interactable = !isFullscreen; }

        if (isFullscreen)
        {
            Resolution[] allResolutions = Screen.resolutions;
            Resolution maxResolution = allResolutions[allResolutions.Length - 1];
            Screen.SetResolution(maxResolution.width, maxResolution.height, true);
        }
        else { SetScreenResolution(activeScreenResIndex); }

        PlayerPrefs.SetInt("fullscreen", ((isFullscreen) ? 1 : 0));
        PlayerPrefs.Save();
    }

    //Volume
    public void SetMasterVolume() { AudioManager.instance.SetVolume(masterVolumeSlider.value, AudioManager.AudioChannel.Master); }

    public void SetMusicVolume() { AudioManager.instance.SetVolume(musicVolumeSlider.value, AudioManager.AudioChannel.Music); }

    public void SetSfxVolume() { AudioManager.instance.SetVolume(sfxVolumeSlider.value, AudioManager.AudioChannel.Sfx); }
}
