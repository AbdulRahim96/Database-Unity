using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Logout : MonoBehaviour
{
    public GameObject menu;
    public GameObject Cam;
    public GameObject btn;
    public void Loggedout()
    {
        Cam.SetActive(false);
        menu.SetActive(true);
        btn.SetActive(false);
        var obj = GameObject.FindGameObjectWithTag("Player");
        Destroy(obj);
        GetComponent<Toggle>().isOn = false;
    }

    public void restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
