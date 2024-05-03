using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    Rigidbody rigidb;

    public void PlayGame()
    {
        SceneManager.LoadScene("Endless");
        Time.timeScale = 1f;
        GetComponent<PlayerMovement>().enabled = false;
        rigidb.useGravity = false;
    }

    public void QuitGame()
    {
        UnityEngine.Debug.Log("QUIT!");
        Application.Quit();
    }
}
