using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InheritNode : NewNode
{
    
    public string totalMoney;
    public int money = 0;
    
    
    public void accountDetails(Text text)
    {
        money = person.gameObject.GetComponent<Account>().BankAmount;
        text.text += "\nTotal Amount: " + money;
    }

    public void withdraw(string str)
    {
        int amount = int.Parse(str);
        if(amount <= money)
        {
            money -= amount;
            person.money += amount;
            person.gameObject.GetComponent<Account>().BankAmount = money;
        }
    }

    public void deposite(string str)
    {
        int amount = int.Parse(str);
        if (amount <= person.money)
        {
            money += amount;
            person.money -= amount;
            person.gameObject.GetComponent<Account>().BankAmount = money;
        }
    }
}
