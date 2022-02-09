using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [SerializeField] public Canvas ShopMenu;

    // Start is called before the first frame update
    void Start()
    {
        ShopMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCloseShop()
    {
        if(ShopMenu.enabled == true)
        {
            ShopMenu.enabled = false;
        }
        else if(ShopMenu.enabled == false)
        {
            ShopMenu.enabled = true;
        }
    }


}
