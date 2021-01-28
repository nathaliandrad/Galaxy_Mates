using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    public void StartApp()
    {
        Debug.Log("Opening up AR scene now!");
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    public void AppSettings()
    {
        Debug.Log("Opening up app settings now!");
    }

    public void VisitWebsite() {
        Application.OpenURL("bigfootds.com");
    }

    public void QuitApp() 
    {
        Debug.Log("Quitting app now!");
        Application.Quit();
    }
}
