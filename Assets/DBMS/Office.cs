using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Office : Recall
{
    public Dropdown jobTitleList, departmentList;
    public int salary;
    public string jobTitle, department;

    private string FullName;

    public void verifing()
    {
        Verify();
        if(player.CNIC != 0)
            StartCoroutine(GetData());
    }

    public IEnumerator GetData()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", tableName);
        form.AddField("columnToFetch", "employeeID");
        form.AddField("where", "employeeID");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "Getting Employee details:   ");
        yield return new WaitForSeconds(0.3f);
        if (SQLConnect.dataNotFound)
        {
            newUserOption.SetActive(true);
            existUserOption.SetActive(false);
        }
        else
        {
            newUserOption.SetActive(false);
            existUserOption.SetActive(true);

            WWWForm form2 = new WWWForm();
            form2.AddField("table", tableName);
            form2.AddField("columnToFetch", "salary");
            form2.AddField("where", "employeeID");
            form2.AddField("condition", player.CNIC);
            SQLConnect.SingleData(form2, "Getting Salary:   ");
            yield return new WaitForSeconds(0.3f);
            salary = ValueCheck.ConvertToInteger(SQLConnect.data[0]);
            StartCoroutine(gettingName());
        }
    }
    public void OnJobChanged(int index)
    {
        jobTitle = jobTitleList.options[index].text;
        salary = (index + 1) * 500;
    }

    public void OnDepartmentChanged(int index)
    {
        department = departmentList.options[index].text;
    }

    public void Apply()
    {
        if (player.CNIC == 0)
            SQLConnect.ShowMessage("You have not registered in Nadra", Color.red, 2);
        else
            StartCoroutine(Insert());
    }

    public IEnumerator Insert()
    {
        StartCoroutine(gettingName());
        yield return new WaitForSeconds(0.5f);
        string query = "insert into " + tableName + " Values(" + player.CNIC + ", '" + FullName + "', '" + department + "', '" + jobTitle + "', '" + DateTime.Now.ToString("yyyy-MM-dd") + "', " + salary + ");";
        SQLConnect.Query(query, "Employee Table:    ");
    }

    IEnumerator gettingName()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "nadra");
        form.AddField("columnToFetch", "name");
        form.AddField("where", "cnic");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "getting Name:  ");
        yield return new WaitForSeconds(0.3f);
        FullName = SQLConnect.data[0];
    }

    public void getSalary()
    {
        
        if (!player.salaryTaken)
        {
            if(player.hasBankAccount)
            {
                player.UpdateUserBank(-salary);
                SQLConnect.ShowMessage(salary + " has been deposited in " + FullName + "'s Account", Color.green, 3);
            }
            else
            {
                player.UpdateUserMoney(-salary);
                SQLConnect.ShowMessage(salary + " has been added in " + FullName + "'s Wallet", Color.green, 3);
            }
            player.salaryTaken = true;
        }
        else
            SQLConnect.ShowMessage(FullName + " has already recieved salary", Color.yellow, 2);
    }
}
