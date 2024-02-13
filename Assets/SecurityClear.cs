using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SecurityClear : MonoBehaviour
{
    public static bool isClear = false;
    public Button submitButton;
    public GameObject stackSecurity, form;

    // Update is called once per frame
    void FixedUpdate()
    {
        submitButton.interactable = isClear;
        GetComponent<Toggle>().isOn = isClear;
        GetComponent<Toggle>().interactable = !isClear;
    }

    public void reCaptcha()
    {
        if(!isClear)
        {
            GameObject obj = Instantiate(stackSecurity);
            obj.GetComponent<Stack>().form = form;
            form.SetActive(false);
        }
    }
}
