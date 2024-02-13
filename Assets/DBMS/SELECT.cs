using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SELECT : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public static void GET(string column, string url, Text text)
    {
        SELECT obj = FindObjectOfType<SELECT>();
        obj.StartCoroutine(obj.connect(column, url, text));
    }

    public IEnumerator connect(string column, string url, Text text)
    {

        print("before");
        WWWForm form = new WWWForm();
        form.AddField("column", column);
        UnityWebRequest server = UnityWebRequest.Post(url, form);

        yield return server.SendWebRequest();

        if (server.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("login => " + server.downloadHandler.text);
        }
        else
        {
            Debug.Log("failed. error #" + server.error);
        }

        text.text = server.downloadHandler.text;
        /*string echo = server.downloadHandler.text;
        string[] data = echo.Split('*');
        for (int i = 0; i < data.Length; i++)
        {
            text[i].text = data[i];
        }*/
    }
}
