using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void ToImage()
    {
        SceneManager.LoadScene(7);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }

    public void ToHowToPlay()
    {
        SceneManager.LoadScene(6);
    }

    public void ReturnToFirst()
    {
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
