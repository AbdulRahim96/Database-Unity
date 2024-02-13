using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorSelection : MonoBehaviour
{
    public InputField username, password, email;
    public Button create;
    public GameObject[] Avatars;
    public GameObject[] instances;
    public int AvatarIndex;
    public Text messageText;
    public GameObject cam;
    public void OnEnable()
    {
        for (int i = 0; i < Avatars.Length; i++)
            Avatars[i].SetActive(false);
        Avatars[AvatarIndex].SetActive(true);
    }

    public void CheckInputField()
    {
        if (string.IsNullOrEmpty(username.text) || string.IsNullOrEmpty(password.text) || string.IsNullOrEmpty(email.text))
        {
            Debug.Log("Input field is empty!");
            create.interactable = false;
        }
        else
            create.interactable = true;
    }

    public void selected() // new user creation
    {
        GameObject obj = Instantiate(instances[AvatarIndex]);
        obj.GetComponent<User>().username = username.text;
        obj.GetComponent<User>().email = email.text;
        obj.GetComponent<User>().avatar = AvatarIndex;
        obj.GetComponent<User>().Set(password.text);
        gameObject.SetActive(false);
        cam.SetActive(true);
        createUser();
    }

    public void selected(string n, string e, int a, int c, int w)
    {
        GameObject obj = Instantiate(instances[a]);
        obj.GetComponent<User>().UserInitialize(n, e, c, w);
        gameObject.SetActive(false);
        cam.SetActive(true);
    }

    public void NewAccount()
    {
        WWWForm form = new WWWForm();
        form.AddField("c1", email.text);
        form.AddField("c2", password.text);
        Login obj = GetComponent<Login>();
        obj.StartCoroutine(obj.SignupCheck(form));
    }

    public void DisplayText(string text, Color color)
    {
        messageText.color = color;
        StartCoroutine(ShowText(text));
    }

    IEnumerator ShowText(string text)
    {
        messageText.text = text;
        yield return new WaitForSeconds(2);
        messageText.text = "";
        messageText.color = Color.white;
    }

    void validityCheck()
    {
       // bool check = username.text.Length.
    }

    public void selectionNext()
    {
        if (AvatarIndex != Avatars.Length - 1)
        {
            Avatars[AvatarIndex].SetActive(false);
            AvatarIndex++;
            Avatars[AvatarIndex].SetActive(true);
        }
    }

    public void selectionPrevious()
    {
        if (AvatarIndex != 0)
        {
            Avatars[AvatarIndex].SetActive(false);
            AvatarIndex -= 1;
            Avatars[AvatarIndex].SetActive(true);
        }
    }

    void createUser()
    {
        // add user in unitydatabase.user
        WWWForm form = new WWWForm();
        form.AddField("table", "user");
        form.AddField("column1", username.text);
        form.AddField("column2", password.text);
        form.AddField("column3", email.text);
        form.AddField("column4", AvatarIndex);
        SQLConnect.Register(form);
    }
}
