using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Education : Recall
{
    public Dropdown programDropDown;
    public string program;

    public void OnDropDownValueChanged(int index)
    {
        program = programDropDown.options[index].text;
        Debug.Log("Selected option: " + program);

        // Perform actions based on the selected option
    }

    public void enroll()
    {
        if (player.CNIC == 0)
            SQLConnect.ShowMessage("You have not registered in Nadra", Color.red, 2);
        else
            StartCoroutine(Insert());
    }

    public IEnumerator Insert()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "nadra");
        form.AddField("columnToFetch", "name");
        form.AddField("where", "cnic");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "getting Name:  ");
        yield return new WaitForSeconds(0.5f);
        string FullName = SQLConnect.data[0];

        WWWForm SUBMIT = new WWWForm();
        SUBMIT.AddField("table", "education");
        SUBMIT.AddField("column1", player.CNIC);
        SUBMIT.AddField("column2", FullName);
        SUBMIT.AddField("column3", DateTime.Now.ToString("yyyy-MM-dd"));
        SUBMIT.AddField("column4", program);
        SQLConnect.InsertRecord(SUBMIT);
        yield return new WaitForSeconds(0.3f);
        Verify();
    }

    public void Verify()
    {
        StartCoroutine(Verifying());
    }

    public IEnumerator Verifying()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "education");
        form.AddField("columnToFetch", "ID");
        form.AddField("where", "ID");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "getting ID:  ");
        yield return new WaitForSeconds(0.3f);
        if(SQLConnect.dataNotFound)
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

}
