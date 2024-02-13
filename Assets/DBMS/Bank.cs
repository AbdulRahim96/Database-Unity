using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bank : Recall
{
    public Text Name, accountNumber, BankMoney, id;
    public Text[] userTexts;
    public InputField Amount;
    public void Verify()
    {
        StartCoroutine(GetData());
    }

    public void ViewTransactions()
    {
        resetData(userTexts);
        StartCoroutine(UserBankRecord());
    }

    IEnumerator GetData()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "bank");
        form.AddField("columnToFetch", "Beneficiary_CNIC");
        form.AddField("where", "Beneficiary_CNIC");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "Getting Bank details:   ");
        yield return new WaitForSeconds(0.5f);
        if(SQLConnect.dataNotFound)
        {
            newUserOption.SetActive(true);
            existUserOption.SetActive(false);
        }
        else
        {
            newUserOption.SetActive(false);
            existUserOption.SetActive(true);
            StartCoroutine(GetData("Beneficiary_Name", Name));
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(GetData("Account_Number", accountNumber));
            BankMoney.text = player.bankMoney.ToString("0");
        }

    }

    IEnumerator UserBankRecord()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "transactions");
        form.AddField("columnToFetch1", "TransactionID");
        form.AddField("columnToFetch2", "Amount");
        form.AddField("columnToFetch3", "transaction_date");
        form.AddField("where", "Beneficiary_CNIC");
        form.AddField("condition", player.CNIC);
        SQLConnect.UserRecords(form, "User records:    ");
        yield return new WaitForSeconds(0.4f);
        if (SQLConnect.dataNotFound)
        {
            id.text = "No Records";
        }
        else
        {
            id.text = Name.text;
            for (int i = 0; i < SQLConnect.data.Length; i++)
                userTexts[i].text = SQLConnect.data[i];
        }
    }

    IEnumerator GetData(string columnToFetch, Text UI)
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "bank");
        form.AddField("columnToFetch", columnToFetch);
        form.AddField("where", "Beneficiary_CNIC");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "Getting Bank details:   ");
        yield return new WaitForSeconds(0.3f);
        UI.text = columnToFetch + " - " + SQLConnect.data[0];
    }
    int SetAccountNumber()
    {
        return Random.Range(100000000, 999999999);
    }

    public void CreateAccount()
    {
        if (player.CNIC == 0)
            SQLConnect.ShowMessage("You have not registered in Nadra", Color.red, 2);
        else
            StartCoroutine(CreateBankAccount());
    }

    public IEnumerator CreateBankAccount()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "nadra");
        form.AddField("columnToFetch", "name");
        form.AddField("where", "cnic");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "getting Name:  ");
        yield return new WaitForSeconds(0.5f);
        string FullName = SQLConnect.data[0];
        print("Banking: " + FullName);
        WWWForm SUBMIT = new WWWForm();
        SUBMIT.AddField("table", "bank");
        SUBMIT.AddField("column1", SetAccountNumber());
        SUBMIT.AddField("column2", FullName);
        SUBMIT.AddField("column3", player.CNIC);
        SUBMIT.AddField("column4", 5000);
        SQLConnect.InsertRecord(SUBMIT);
        player.bankMoney = 5000;
        player.hasBankAccount = true;
        yield return new WaitForSeconds(0.5f);
        Verify();
    }
    public void Deposite()
    {
        int val = ValueCheck.ConvertToInteger(Amount.text);
        if (player.wallet >= val)
        {
            player.UpdateUserBank(-val);
            player.UpdateUserMoney(val);
            BankMoney.text = player.bankMoney.ToString("0");
        }
        else
            SQLConnect.ShowMessage("Not Enough Money to Deposite.", Color.red, 2);
    }
    public void Withdraw()
    {
        int val = ValueCheck.ConvertToInteger(Amount.text);
        if (player.bankMoney >= val)
        {
            player.UpdateUserBank(val);
            player.UpdateUserMoney(-val);
            BankMoney.text = player.bankMoney.ToString("0");
        }
        else
            SQLConnect.ShowMessage("Not Enough Money to Withdraw.", Color.red, 2);
    }
}
