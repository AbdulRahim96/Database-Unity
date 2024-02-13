using System.Collections;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Item : MonoBehaviour
{
    public int itemID;
    public string itemName;
    public int price = 300;
    public int stock = 3;
    bool available = true;
    public int clothIndex;
    private User player;
    public GameObject mesh;
    public Text nameText, priceText;
    void Start()
    {
        
        StartCoroutine(getData("Items:    "));
    }
    public void Initial()
    {
        available = stock > 0 ? true : false;
        mesh.SetActive(available);
        GetComponent<Collider>().enabled = available;
        nameText.text = itemName;
        priceText.text = "$" + price.ToString("0");
    }

    public void purchase(bool cash)
    {   

        if(cash)
        {
            if(player.wallet >= price)
            {
                player.UpdateUserMoney(price);
                stock--;
                StartCoroutine(UpdateBridge());
                StartCoroutine(UpdateStock());
                player.RefreshCloth(clothIndex, true);
                SQLConnect.ShowMessage(itemName + " purchased", Color.green, 2);
            }
            else
                SQLConnect.ShowMessage("Not enough money to buy", Color.red, 2);
        }
        else
        {
            if(!player.hasBankAccount)
            {
                SQLConnect.ShowMessage("You Don't have Credit card", Color.red, 2);
                return;
            }
            if (player.bankMoney >= price)
            {
                player.UpdateUserBank(price);
                stock--;
                StartCoroutine(UpdateBridge());
                StartCoroutine(UpdateStock());
                player.RefreshCloth(clothIndex, true);
                SQLConnect.ShowMessage(itemName + " purchased", Color.green, 2);
            }
            else
                SQLConnect.ShowMessage("Not enough money to buy", Color.red, 2);
        }
        
    }

    public void Steal()
    {
        if (player.CNIC != 0)
        {
            int fine = price*2;
            WWWForm form = new WWWForm();
            form.AddField("table", "policeStation");
            form.AddField("column1", player.CNIC);
            form.AddField("column2", "Stole " + itemName);
            form.AddField("column3", fine);
            form.AddField("column4", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            SQLConnect.InsertRecord(form);
        }
        else
        {
            player.UpdateUserMoney(player.wallet);
            player.transform.position = Police.Jail();
        }

        stock--;
        StartCoroutine(UpdateStock());
        player.RefreshCloth(clothIndex, true);
        SQLConnect.ShowMessage(itemName + " has Stolen", Color.red, 2);
    }

    public IEnumerator UpdateStock()
    {
        yield return new WaitForSeconds(0.5f);
        WWWForm form = new WWWForm();
        form.AddField("table", "item");
        form.AddField("columnToUpdate", "stock");
        form.AddField("input", stock);
        form.AddField("where", "itemid");
        form.AddField("condition", itemID);
        SQLConnect.UpdateRecord(form);
        yield return new WaitForSeconds(0.5f);
        Start();
    }

    public IEnumerator UpdateBridge()
    {
        yield return new WaitForSeconds(0.5f);
        WWWForm form = new WWWForm();
        form.AddField("table", "bridge");
        //string customerID = player.CNIC == 0 ? "null" : player.CNIC.ToString("0");
        form.AddField("column1", player.email);
        form.AddField("column2", itemID);
        form.AddField("column3", itemName);
        form.AddField("column4", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        SQLConnect.InsertRecord(form);
    }

    public IEnumerator getData(string source)
    {
        WWWForm form = new WWWForm();
        form.AddField("table", "item");
        form.AddField("where", "itemID");
        form.AddField("condition", itemID);

        form.AddField("column1", itemID);
        form.AddField("column2", itemName);
        form.AddField("column3", price);
        form.AddField("column4", stock);
        UnityWebRequest server = UnityWebRequest.Post("http://localhost/unity/singlerow.php", form);

        yield return server.SendWebRequest();

        if (server.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(source + server.downloadHandler.text);
            if (server.downloadHandler.text == "0 results")
                StartCoroutine(insert()); // insert new record of item
            else
            {
                string echo = server.downloadHandler.text;
                string[] data = echo.Split('*');
                itemID = ValueCheck.ConvertToInteger(data[0]);
                itemName = data[1];
                price = ValueCheck.ConvertToInteger(data[2]);
                stock = ValueCheck.ConvertToInteger(data[3]);
                Initial();
            }

        }
        else
            Debug.Log("failed. error #" + server.error);
    }

    public IEnumerator insert()
    {

        WWWForm form = new WWWForm();
        form.AddField("table", "item");
        form.AddField("column1", itemID);
        form.AddField("column2", itemName);
        form.AddField("column3", price);
        form.AddField("column4", stock);
        UnityWebRequest server = UnityWebRequest.Post("http://localhost/unity/insert.php", form);
        yield return server.SendWebRequest();

        if (server.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Item: " + server.downloadHandler.text);
        }
        else
        {
            Debug.Log("failed. error #" + server.error);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<User>())
            player = other.gameObject.GetComponent<User>();
    }


}
