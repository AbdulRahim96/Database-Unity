using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Police : Recall
{
    public Text[] allTexts, userTexts;
    public Text id, totalText;
    public int total;
    public Transform jail, outside;
    public Button payButton;

    public void Fine()
    {
        StartCoroutine(totalFine());
    }

    public static Vector3 Jail()
    {
        Police obj = FindObjectOfType<Police>();
        return obj.jail.position;
    }

    public static Vector3 Outside()
    {
        Police obj = FindObjectOfType<Police>();
        return obj.outside.position;
    }

    public void showAll()
    {
       StartCoroutine(all());
    }

    IEnumerator all()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "policeStation");
        form.AddField("column1", "criminalID");
        form.AddField("column2", "crime_description");
        form.AddField("column3", "fine_amount");
        form.AddField("column4", "crime_date");
        form.AddField("order", "crime_date");
        SQLConnect.All(form, "Police:    ");
        yield return new WaitForSeconds(0.4f);
        if (SQLConnect.dataNotFound)
        {
            //no records
        }
        else
            for (int i = 0; i < SQLConnect.data.Length; i++)
                allTexts[i].text = SQLConnect.data[i];
    }

    public void showUser()
    {
       StartCoroutine(UserRecord());
    }

    IEnumerator UserRecord()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", tableName);
        form.AddField("columnToFetch1", "crime_description");
        form.AddField("columnToFetch2", "fine_amount");
        form.AddField("columnToFetch3", "crime_date");
        form.AddField("where", "criminalID");
        form.AddField("condition", player.CNIC);
        SQLConnect.UserRecords(form, "User records:    ");
        yield return new WaitForSeconds(0.4f);
        if (SQLConnect.dataNotFound)
        {
            id.text = "No Records";
        }
        else
        {
            id.text = "Criminal ID - " + player.CNIC.ToString("0");
            for (int i = 0; i < SQLConnect.data.Length; i++)
                userTexts[i].text = SQLConnect.data[i];
        }
    }


    public void Pay()
    {
        if (player.wallet >= total || player.bankMoney >= total)
        {
            if (player.wallet >= total)
                player.UpdateUserMoney(total);
            else
                player.UpdateUserBank(total);
            PayAll(player.CNIC);
            player.gameObject.transform.position = Outside();
        }
        else
            SQLConnect.ShowMessage("Not enough money!", Color.red, 2);
    }

    IEnumerator totalFine()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "policeStation");
        form.AddField("columnToFetch", "sum(fine_amount)");
        form.AddField("where", "criminalID");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "Crime Check:   ");
        yield return new WaitForSeconds(0.3f);
        if(SQLConnect.dataNotFound)
        {
            //no records
            payButton.interactable = false;
        }
        else
        {
            total = ValueCheck.ConvertToInteger(SQLConnect.data[0]);
            totalText.text = "Pay $" + total.ToString("0");
            payButton.interactable = transform;
        }
    }

    void PayAll(int CNIC)
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "policeStation");
        form.AddField("where", "criminalID");
        form.AddField("condition", CNIC);
        SQLConnect.DeleteRecord(form);
        resetData(allTexts);
        resetData(userTexts);
        player.transform.position = Outside();
        SQLConnect.ShowMessage("All Records are removed!", Color.green, 3);
    }

    /*public void resetData()
    {
        for (int i = 0; i < allTexts.Length; i++)
            allTexts[i].text = "-------";
        for (int i = 0; i < userTexts.Length; i++)
            userTexts[i].text = "-------";
    }*/
}
