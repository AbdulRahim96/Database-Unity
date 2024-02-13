using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject canvus;
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            canvus.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            canvus.SetActive(false);
        }
    }
}
