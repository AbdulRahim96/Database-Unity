using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Login : MonoBehaviour
{

    public InputField userName, password;

    public GameObject loading;
    public void LoginUser()
    {
        WWWForm form = new WWWForm();
        form.AddField("c1", userName.text);
        form.AddField("c2", password.text);
        StartCoroutine(LoginCheck(form));
    }

    public IEnumerator LoginCheck(WWWForm form)
    {
        UnityWebRequest server = UnityWebRequest.Post("http://localhost/unity/login.php", form);
        yield return server.SendWebRequest();
        if (server.result == UnityWebRequest.Result.Success)
        {
            if (server.downloadHandler.text == "null")
                SQLConnect.ShowMessage("User Not Found!", Color.red, 2);
            else
            {
                print(server.downloadHandler.text);
                string echo = server.downloadHandler.text;
                SQLConnect.data = echo.Split('*');
                string name = SQLConnect.data[0];
                string email = SQLConnect.data[1];
                int avatar = ValueCheck.ConvertToInteger(SQLConnect.data[2]);
                int cloth = ValueCheck.ConvertToInteger(SQLConnect.data[3]);
                int wallet = ValueCheck.ConvertToInteger(SQLConnect.data[4]);
                GetComponent<CharactorSelection>().selected(name, email, avatar, cloth, wallet);
            }
        }
        else
            Debug.Log("failed. error #" + server.error);
    }

    public IEnumerator SignupCheck(WWWForm form)
    {
        UnityWebRequest server = UnityWebRequest.Post("http://localhost/unity/login.php", form);
        yield return server.SendWebRequest();
        if (server.result == UnityWebRequest.Result.Success)
        {
            print("sign up:   " + server.downloadHandler.text);
            if (server.downloadHandler.text == "null")
            {
                SQLConnect.ShowMessage("New user created! ", Color.white, 2);
                FindObjectOfType<CharactorSelection>().selected();
            }
            else
                FindObjectOfType<CharactorSelection>().DisplayText("user already exists! ", Color.red);
        }
        else
            Debug.Log("failed. error #" + server.error);
    }
}
