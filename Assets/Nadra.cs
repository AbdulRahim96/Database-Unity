using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Nadra : Recall
{
    // Start is called before the first frame update
    public InputField fullName, address, changingAddress;
    public Button create;
    public Dropdown dayDropdown;
    public Dropdown monthDropdown;
    public Dropdown yearDropdown;
    public Text nameText, cnic;
    public string DOB;
    private void Start()
    {
        // Populate day dropdown with options 1-31
        for (int day = 1; day <= 30; day++)
        {
            dayDropdown.options.Add(new Dropdown.OptionData(day.ToString()));
        }
        dayDropdown.value = 0; // Select the first option (index 0)
        dayDropdown.RefreshShownValue();

        // Populate month dropdown with options January-December
        string[] months = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        monthDropdown.AddOptions(new List<string>(months));
        monthDropdown.value = 0; // Select the first option (index 0)
        monthDropdown.RefreshShownValue();

        // Populate year dropdown with options 1900-2023
        int currentYear = DateTime.Now.Year;
        for (int year = 1950; year <= currentYear; year++)
        {
            yearDropdown.options.Add(new Dropdown.OptionData(year.ToString()));
        }
        yearDropdown.value = yearDropdown.options.Count - 1; // Select the last option (current year)
        yearDropdown.RefreshShownValue();
    }

    public void GetSelectedDateOfBirth()
    {
        int day = int.Parse(dayDropdown.options[dayDropdown.value].text);
        int month = monthDropdown.value + 1; // Months are zero-based, so add 1
        int year = int.Parse(yearDropdown.options[yearDropdown.value].text);

        // Create a DateTime object with the selected date of birth
        DateTime dateOfBirth = new DateTime(year, month, day);
        DOB = dateOfBirth.ToString("yyyy-MM-dd");
        Debug.Log("Selected Date of Birth: " + dateOfBirth.ToString("yyyy-MM-dd"));
    }


    public void CheckInputField()
    {
        if (string.IsNullOrEmpty(fullName.text) || string.IsNullOrEmpty(address.text))
        {
            Debug.Log("Input field is empty!");
            create.interactable = false;
        }
        else
            create.interactable = true;
    }

    public void SetData()
    {
        if(player.CNIC != 0)
        {
            StartCoroutine(GetData());
        }
    }

    public IEnumerator GetData()
    {
        cnic.text = "CNIC - " + player.CNIC.ToString("0");
        WWWForm form = new WWWForm();
        form.AddField("table", tableName);
        form.AddField("columnToFetch", "name");
        form.AddField("where", "CNIC");
        form.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form, "Getting Name:   ");
        yield return new WaitForSeconds(0.3f);
        nameText.text = "Name - " + SQLConnect.data[0];

        WWWForm form2 = new WWWForm();
        form2.AddField("table", tableName);
        form2.AddField("columnToFetch", "address");
        form2.AddField("where", "CNIC");
        form2.AddField("condition", player.CNIC);
        SQLConnect.SingleData(form2, "Getting address:   ");
        yield return new WaitForSeconds(0.3f);
        changingAddress.text = SQLConnect.data[0];
    }
    

    public void ChangeAddress()
    {
        WWWForm form = new WWWForm();
        form.AddField("table", tableName);
        form.AddField("columnToUpdate", "address");
        form.AddField("input", changingAddress.text);
        form.AddField("where", "CNIC");
        form.AddField("condition", player.CNIC);
        SQLConnect.UpdateRecord(form);
    }


    int setCNIC()
    {
        return UnityEngine.Random.Range(40000000, 49999999);
    }

    public void insertData()
    {
        WWWForm form = new WWWForm();
        int cnic = setCNIC();
        GetSelectedDateOfBirth();
        form.AddField("table", tableName);
        form.AddField("column1", cnic);
        form.AddField("column2", fullName.text);
        form.AddField("column3", address.text);
        form.AddField("column4", DOB);
        SQLConnect.InsertRecord(form);
        player.UpdateUserCNIC(cnic);
        Verify();
        SetData();
    }
}
