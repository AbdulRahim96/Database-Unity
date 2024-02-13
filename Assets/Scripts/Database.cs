using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Database : MonoBehaviour
{
    public List[] charactors;
    public Text text;
    public int count = 0;
    private int index = 0;
    public CameraMovementMobile camera;
    public List currentPlayer;
    private float money;
    public Text moneytext;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        charactors = FindObjectsOfType<List>();
        count = charactors.Length;
        money = Mathf.Lerp(money, currentPlayer.money, 0.1f);
        moneytext.text = money.ToString("0");
    }

    public void allSearch()
    {
        text.text = "";
        for (int i = 0; i < charactors.Length; i++ )
        {
            text.text += charactors[i].personInfo();
            text.text += charactors[i].traverse();
            text.text += "=============\n\n";
        }
    }

    public void bubbleSort()
    {
        int current, next;
        bool flag = false;
        while(!flag)
        {
            for(int i = 0; i < charactors.Length - 1; i++)
            {
                current = charactors[i].GetComponent<List>().getCNIC();
                next = charactors[i+1].GetComponent<List>().getCNIC();
                if (current > next)
                {
                    List temp = charactors[i];
                    charactors[i] = charactors[i + 1];
                    charactors[i + 1] = temp;
                    flag = true;
                }
            }
        }
    }

    public void binarySearch(string CNIC)
    {

    }
    public void linearSearch(string name)
    {
        text.text = "";
        for (int i = 0; i < charactors.Length; i++)
        {
            if(charactors[i].fName == name)
            {
                text.text += charactors[i].personInfo();
                text.text += charactors[i].traverse();
                text.text += "=============\n\n";
                return;
            }
        }
        text.text = "The name - " + name + " does not exist in Nadra database";
    }

    public void selectionNext()
    {
        if (index != charactors.Length - 1)
        {
            charactors[index].gameObject.tag = "Untagged";
            index++;
            charactors[index].gameObject.tag = "Player";
            camera.Init();
        }
    }

    public void selectionPrevious()
    {
        if (index != 0)
        {
            charactors[index].gameObject.tag = "Untagged";
            index--;
            charactors[index].gameObject.tag = "Player";
            camera.Init();
        }
    }

    public void selected()
    {
        charactors[index].gameObject.gameObject.GetComponent<InputHandler>().enabled = true;
    }

    public void switchUser(bool flag)
    {
        currentPlayer.gameObject.GetComponent<InputHandler>().enabled = flag;
    }
}
