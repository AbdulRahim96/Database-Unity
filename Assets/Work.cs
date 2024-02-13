using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Work : MonoBehaviour
{

    public GameObject tree;
    
    public void startWork(int difficulty)
    {
        GameObject obj = Instantiate(tree);
        obj.GetComponent<BinaryTree>().count = difficulty;
    }
}
