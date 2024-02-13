using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BinaryTree : MonoBehaviour
{
    treeNode root = null;
    public string str, str2;
    public Text text, resultText;
    public int count = 5;
    public int selection;
    // Start is called before the first frame update
    public void Start()
    {

        selection = Random.Range(0, 2);
        root = null;
        str = str2 = "";
        if (selection == 0)
            text.text = "find postfix order of:\n";
        else
            text.text = "find infix order of:\n";
        for (int i = 0; i < count; i++)
        {
            int random = Random.Range(0, 9);
            if (AddNode(random))
                text.text += random + ",";

        }
        if (selection == 0)
            postFix(root);
        else
            inFix(root);

        GameObject.FindGameObjectWithTag("controls").SetActive(false);
    }

    bool AddNode(int num)
    {
        treeNode n = new treeNode(num);
        if (root == null)
        {
            root = n;
            return true;
        }
        else
        {
            //print(num);
            treeNode cn = root;
            while (cn != null)
            {
                if (cn.data == num)
                {
                    Debug.LogError("same num found");
                    return false;
                }
                    
                if (cn.data > num)
                {
                    if (cn.lc == null)
                    {
                        cn.lc = n;
                        return true;
                    }
                    else
                    {
                        cn = cn.lc;
                    }
                }
                else
                {
                    if (cn.rc == null)
                    {
                        cn.rc = n;
                        return true;
                    }
                    else
                    {
                        cn = cn.rc;
                    }
                }

            }
            return false;
        }
    }

    public void postFix(treeNode rt)
    {
        if(rt != null)
        {
            postFix(rt.lc);
            postFix(rt.rc);
            str += rt.data.ToString("0");
            str2 += rt.data.ToString("0") + ",";
        }
    }

    public void inFix(treeNode rt)
    {
        if (rt != null)
        {
            str += rt.data.ToString("0");
            str2 += rt.data.ToString("0") + ",";
            inFix(rt.lc);
            inFix(rt.rc);
        }
    }


    public void match(string input)
    {
        
        if (input == str2 || input == str)
            resultText.text = "Match";
        else
            resultText.text = "Did not match";

        GameObject.FindGameObjectWithTag("Player").GetComponent<List>().money += (count * 20);
    }

    public void exit()
    {
        GameObject.FindGameObjectWithTag("controls").SetActive(true);
        Destroy(gameObject);
    }

    

    public class treeNode
    {
        public treeNode rc, lc;
        public int data;

        public treeNode(int val)
        {
            data = val;
            rc = lc = null;
        }
    }
}
