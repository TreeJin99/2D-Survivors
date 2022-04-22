using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstScen : MonoBehaviour
{
    public void startGame()
    {
        SceneManager.LoadScene("Main");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
