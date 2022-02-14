using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class CreditManager : MonoBehaviour
{
    public int playerCredits = 0;
    [SerializeField] public Text creditCountUI;


    // Start is called before the first frame update
    void Start()
    {
        UpdateCreditCount();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeCredits(int ChangeAmt)
    {
        if(ChangeAmt > 0 || (ChangeAmt < 0 && playerCredits >= -(ChangeAmt)))
        {
            playerCredits = playerCredits + ChangeAmt;
            UpdateCreditCount();
        }
        else
        {
            Debug.Log("You do not have enough credits for that purchase!");
        }
    }

    public void UpdateCreditCount()
    {
        creditCountUI.GetComponent<UnityEngine.UI.Text>().text = playerCredits.ToString();
    }

    public int GetCredits()
    {
        return playerCredits;
    }
}
