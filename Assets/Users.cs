using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Users : MonoBehaviour
{

    public GameObject[] avatars;
    public GameObject[] instance;
    public int index;

    public static bool fast = false;
    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectionNext()
    {
        if (index != avatars.Length - 1)
        {
            avatars[index].SetActive(false);
            index++;
            avatars[index].SetActive(true);
        }
    }

    public void selectionPrevious()
    {
        if (index != 0)
        {
            avatars[index].SetActive(false);
            index -= 1;
            avatars[index].SetActive(true);
        }
    }

    public void selected()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<InputHandler>().enabled = false;
        Instantiate(instance[index], transform.position, transform.rotation);
        avatars[index].SetActive(false);
    }

    public void newUser(bool flag)
    {
        avatars[index].SetActive(flag);
    }

    public void sprint(bool flag)
    {
        fast = flag;
    }
}
