using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropDownHandler : MonoBehaviour
{
    public void OnDropDownValueChanged(int index)
    {
        string selectedOption = gameObject.GetComponent<Dropdown>().options[index].text;
        Debug.Log("Selected option: " + selectedOption);

        // Perform actions based on the selected option
    }
}
