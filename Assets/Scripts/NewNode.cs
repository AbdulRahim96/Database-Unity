using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewNode : MonoBehaviour
{
    public string dataType;
    public List person;
    public GameObject button;

    public string[] data;
    private int count = 0;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            person = other.gameObject.GetComponent<List>();
            button.SetActive(true);
            SecurityClear.isClear = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            button.SetActive(false);
    }

    public void addNode(Text text)
    {
        if(person.head != null)
        {
            bool exist = false;
            for (MYNode i = person.head; i != null; i = i.next)
                if (dataType == i.dataType)
                    exist = true; //This node already exist for this person

            if (!exist)
            {
                person.createNode(dataType, data[0], data[1], data[2]);
                text.text = "Submitted";
            }
                
            else
                text.text = "This User already exist in the Database";
        }
        else
            text.text = "No data records from Nadra, Please register yourself to Nadra first";

    }

    public void headNode(Text text)
    {
        if (person.head == null)
        {
            if(data[0] == null || data[1] == null || data[2] == null)
            {
                text.text = "Please fill the form";
                text.color = Color.red;
            }
            else
            {
                person.fName = data[0];
                person.lastName = data[1];
                person.Gender = data[2];
                person.setCNIC(Random.Range(40000000, 49999999));
                person.Start();
                person.createNode(dataType, "", "", "");
                text.text = "User created";
                text.color = Color.green;
            }
        }
        else
            text.text = "This User already exist in the Database";

    }

    public void addData(string input)
    {
        data[count++] += input;
    }

    public void viewSingle(Text text)
    {
        for (MYNode i = person.head; i != null; i = i.next)
        {
            if(dataType == i.dataType)
            {
                text.text = i.viewSingleNode();
                return;
            }
        }
        text.text = "No records for " + person.fName;
    }
}
