using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recall : MonoBehaviour
{
    public GameObject canvus;
    public string tableName;
    public User player;

    public GameObject newUserOption, existUserOption;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject.GetComponent<User>();
            canvus.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            canvus.SetActive(false);
    }

    public void Verify()
    {
        if(player.CNIC == 0) // user does not exist
        {
            newUserOption.SetActive(true);
            existUserOption.SetActive(false);
        }
        else
        {
            newUserOption.SetActive(false);
            existUserOption.SetActive(true);
        }
    }

    /*public IEnumerator Verify(int IDNumber)
    {
        WWWForm form = new WWWForm();
        form.AddField("table", tableName);
        form.AddField("columnToFetch", "name");
        form.AddField("where", "cnic");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "getting Name:  ");
        yield return new WaitForSeconds(0.5f);
        string FullName = SQLConnect.data[0];
    }*/

    public void resetData(Text[] texts)
    {
        for (int i = 0; i < texts.Length; i++)
            texts[i].text = "-------";
    }
}
