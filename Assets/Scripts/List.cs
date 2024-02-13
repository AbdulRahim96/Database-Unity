using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class List : MonoBehaviour
{
    public MYNode head = null, tail = null;
    public int counter = 0;
    public Text text;
    public string fName, lastName, ID, Gender;
    private int CNIC;
    public int money = 500;

    public void Start()
    {
        ID = CNIC.ToString("0");
        text.text = fName;
    }

    public void setCNIC(int id)
    {
        CNIC = id;
    }

    public int getCNIC()
    {
        return CNIC;
    }
    public void createNode(string type, string d1, string d2, string d3)
    {
        MYNode newnode = new MYNode(type, fName, lastName, ID, Gender, d1, d2, d3);
        if(head == null)
        {
            head = newnode;
            tail = newnode;
        }
        else
        {
            newnode.prev = tail;
            tail.next = newnode;
            tail = newnode;
        }
        Debug.Log(type + " node added for: " + fName);
        counter++;
        
    }

    public string traverse()
    {
        Debug.Log("traverse");
        string txt = "";
        for (MYNode i = head.next; i != null; i = i.next)
            txt += "\n\n" + i.viewNode();

        return txt;
    }

    public string personInfo()
    {
        string txt = "Nadra\n" + fName + " " + lastName + "\nCNIC: " + CNIC + "\nGender: " + Gender + "\n\n";
        return txt;

    }

}
