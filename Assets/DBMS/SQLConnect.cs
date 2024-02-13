using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.UI;

public class SQLConnect : MonoBehaviour
{
    public static string[] data;
    public static bool dataNotFound = false;
    public GameObject messageBox;
    // Start is called before the first frame update
    void Start()
    {
        DateTime currentDate = DateTime.Now;
        Debug.Log("Current date: " + currentDate.ToString("yyyy-MM-dd"));
        Debug.Log("Current date with time: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //register();

    }

    /*public void validation()
    {
        if(CNIC.text.Length == 0 || firstName.text.Length == 0 || lastName.text.Length == 0 || dob.text.Length == 0)
            createButton.interactable = false;
        else
            createButton.interactable = true;
    }*/

    public void register()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "item");
        form.AddField("columnToFetch", "stock");
        form.AddField("where", "itemid");
        form.AddField("condition", "123");
        //SingleData(form);
    }

    public IEnumerator getData(WWWForm form, string url, string source)
    {
        UnityWebRequest server = UnityWebRequest.Post(url, form);

        yield return server.SendWebRequest();
        
        if(server.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(source + server.downloadHandler.text);
            dataNotFound = (server.downloadHandler.text == "");
            if (dataNotFound)
                print("data not found");
        }
        else
        {
            ShowMessage("failed. error #" + server.error, Color.red, 5);
        }

        string echo = server.downloadHandler.text;
        data = echo.Split('*');
        for (int i = 0; i < data.Length; i++)
            print(data[i]);
    }

    public IEnumerator connect(WWWForm form, string url)
    {
        UnityWebRequest server = UnityWebRequest.Post(url, form);
        yield return server.SendWebRequest();

        if (server.result == UnityWebRequest.Result.Success)
        {
            print(server.downloadHandler.text);
           // ShowMessage(server.downloadHandler.text, Color.cyan);
        }
        else
        {
            ShowMessage("failed. error #" + server.error, Color.red, 5);
        }
    }

    public static void All(WWWForm form, string source)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.getData(form, "http://localhost/unity/data.php", source));
    }

    public static void UserRecords(WWWForm form, string source)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.getData(form, "http://localhost/unity/user.php", source));
    }


    public static void Login(WWWForm form)
    {
        
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.connect(form, "http://localhost/unity/login.php"));
    }

    public static void Register(WWWForm form)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.connect(form, "http://localhost/unity/register.php"));
    }

    public static void InsertRecord(WWWForm form)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.connect(form, "http://localhost/unity/insert.php"));
    }

    public static void DeleteRecord(WWWForm form)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.connect(form, "http://localhost/unity/delete.php"));
    }

    public static void UpdateRecord(WWWForm form)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.connect(form, "http://localhost/unity/update.php"));
    }

    public static void SingleData(WWWForm form, string source)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.getData(form, "http://localhost/unity/singledata.php", source));
    }

    public static void SingleRow(WWWForm form, string source)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        obj.StartCoroutine(obj.getData(form, "http://localhost/unity/singlerow.php", source));
    }

    public static void Query(string query, string source)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        WWWForm form = new WWWForm();
        form.AddField("query", query);
        obj.StartCoroutine(obj.connect(form, "http://localhost/unity/query.php"));
    }

    public static void ShowMessage(string msg, Color color, float time)
    {
        SQLConnect obj = FindObjectOfType<SQLConnect>();
        GameObject prompt = Instantiate(obj.messageBox);
        prompt.GetComponentInChildren<Text>().text = msg;
        prompt.GetComponentInChildren<Text>().color = color;
        Destroy(prompt, time);
    }


}
