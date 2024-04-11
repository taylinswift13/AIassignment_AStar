using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
}
