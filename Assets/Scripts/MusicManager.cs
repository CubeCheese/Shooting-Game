using UnityEngine;
using UnityEngine.SceneManagement;

//Play Thema
public class MusicManager : MonoBehaviour
{
    public AudioClip[] mainTheme;

    private string sceneName;

    private void Start() { OnLevelWasLoaded(); }

    //Level Load
    private void OnLevelWasLoaded()
    {
        string newSceneName = SceneManager.GetActiveScene().name;

        if (newSceneName != sceneName)
        {
            sceneName = newSceneName;
            Invoke("PlayMusic", .2f);
        }
    }

    //Play Level Music
    private void PlayMusic()
    {
        AudioManager.instance.StopMusic();

        int clipIndex = 0;

        if (sceneName == "Menu") { clipIndex = 0; }
        else { clipIndex = 1; }

        AudioManager.instance.PlayMusic(mainTheme[clipIndex], 1);
        Invoke("PlayMusic", mainTheme[clipIndex].length);
    }
}