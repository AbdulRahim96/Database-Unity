using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class User : MonoBehaviour
{

    public string username, email;
    private string password;
    public int CNIC;
    public int avatar, wallet = 500, bankMoney;

    public GameObject[] cloths;

    public Text walletText, nameText;
    
    [HideInInspector]
    public bool salaryTaken = false, hasBankAccount = false;
    private float money;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        // Getting CNIC
        WWWForm form = new WWWForm();
        form.AddField("table", "user");
        form.AddField("columnToFetch", "cnic");
        form.AddField("where", "email");
        form.AddField("condition", email);
        SQLConnect.SingleData(form, "Getting CNIC:  ");
        yield return new WaitForSeconds(0.3f);
        if (!SQLConnect.dataNotFound) // if data is found 
        {
            CNIC = ValueCheck.ConvertToInteger(SQLConnect.data[0]); // then store in variable
            StartCoroutine(crimeCheck());
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(BankInfo());
        }
        else
            print("new user");

    }
    // FixedUpdate is called every frame
    private void FixedUpdate()
    {
        money = Mathf.Lerp(money, wallet, 0.1f);
        walletText.text = "$" + money.ToString("0");
        nameText.text = username;
    }

    public void Set(string p)
    {
        password = p;
    }
    
    public string Get()
    {
        return password;
    }

    public void UserInitialize(string username, string email, int cloth, int wallet)
    {
        this.username = username;
        this.email = email;
        this.wallet = wallet;
        RefreshCloth(cloth, false);
    }

    public void RefreshCloth(int index, bool UpdateToDatabase)
    {
        for (int i = 0; i < cloths.Length; i++)
            cloths[i].SetActive(false);
        cloths[index].SetActive(true);
        if(UpdateToDatabase)
        {
            WWWForm form = new WWWForm();
            form.AddField("table", "user");
            form.AddField("columnToUpdate", "clothIndex");
            form.AddField("input", index);
            form.AddField("where", "email");
            form.AddField("condition", email);
            SQLConnect.UpdateRecord(form);
        }
    }

    public IEnumerator BankInfo()
    {
        WWWForm SUBMIT = new WWWForm();
        SUBMIT.AddField("table", "bank");
        SUBMIT.AddField("columnToFetch", "amount");
        SUBMIT.AddField("where", "Beneficiary_CNIC");
        SUBMIT.AddField("condition", CNIC);
        SQLConnect.SingleData(SUBMIT, "Bank:    ");
        yield return new WaitForSeconds(0.3f);
        if (!SQLConnect.dataNotFound)
        {
            bankMoney = ValueCheck.ConvertToInteger(SQLConnect.data[0]);
            hasBankAccount = true;
            print("has bank account");
        }
    }

    public IEnumerator crimeCheck()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "policeStation");
        form.AddField("columnToFetch", "sum(fine_amount)");
        form.AddField("where", "criminalID");
        form.AddField("condition", CNIC);
        SQLConnect.SingleData(form, "Crime Check:   ");
        yield return new WaitForSeconds(0.3f);
        if(!SQLConnect.dataNotFound)
        {
            int total = ValueCheck.ConvertToInteger(SQLConnect.data[0]);
            print("Total Fine: " + total);
            if (total > 4500)
                transform.position = Police.Jail();
        }
    }

    public void UpdateUserMoney(int amount)
    {
        wallet -= amount;
        WWWForm form = new WWWForm();
        form.AddField("table", "user");
        form.AddField("columnToUpdate", "wallet");
        form.AddField("input", wallet);
        form.AddField("where", "email");
        form.AddField("condition", email);
        SQLConnect.UpdateRecord(form);
        print(wallet + " is saved in user database");
    }

    public void UpdateUserBank(int amount)
    {
        bankMoney -= amount;
        WWWForm form = new WWWForm();
        form.AddField("table", "bank");
        form.AddField("columnToUpdate", "amount");
        form.AddField("input", bankMoney);
        form.AddField("where", "Beneficiary_CNIC");
        form.AddField("condition", CNIC);
        SQLConnect.UpdateRecord(form);
        print(bankMoney + " is saved in bank database");
        Transaction(amount);
    }

    public void UpdateUserCNIC(int id)
    {
        CNIC = id;
        WWWForm form = new WWWForm();
        form.AddField("table", "user");
        form.AddField("columnToUpdate", "cnic");
        form.AddField("input", CNIC);
        form.AddField("where", "email");
        form.AddField("condition", email);
        SQLConnect.UpdateRecord(form);
        SQLConnect.ShowMessage(username + " is registered\nCNIC - " + CNIC, Color.green, 3);
    }

    public void Transaction(int amount)
    {
        WWWForm SUBMIT = new WWWForm();
        SUBMIT.AddField("table", "transactions");
        int transactionID = UnityEngine.Random.Range(100000000, 999999999);
        SUBMIT.AddField("column1", transactionID);
        SUBMIT.AddField("column2", CNIC);
        SUBMIT.AddField("column3", amount);
        SUBMIT.AddField("column4", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        SQLConnect.InsertRecord(SUBMIT);
    }

    public IEnumerator getCNIC()
    {
        yield return new WaitForSeconds(5f);
        WWWForm form = new WWWForm();
        form.AddField("table", "user");
        form.AddField("columnToFetch", "cnic");
        form.AddField("where", "email");
        form.AddField("condition", email);
        SQLConnect.SingleData(form, "Get CNIC:  ");
        yield return new WaitForSeconds(0.5f);
        if(!SQLConnect.dataNotFound) // if data is found 
            CNIC = ValueCheck.ConvertToInteger(SQLConnect.data[0]); // then store in variable
        StartCoroutine(crimeCheck());
    }
}
