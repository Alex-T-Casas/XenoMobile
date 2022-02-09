using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItem : MonoBehaviour
{
    ShopSystem _shopSystem;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Image Icon;
    public WeaponInfo WeaponInfo
    {
        get;
        private set;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal void Init(WeaponInfo weaponInfo, ShopSystem shopSystem)
    {
        _shopSystem = shopSystem;
        WeaponInfo = weaponInfo;
        Icon.sprite = weaponInfo.Icon;
        text.text = $"{weaponInfo.name}\n" +
            $"Rate: {weaponInfo.ShootSpeed}\n" +
            $"Damage: {weaponInfo.DamagePerBullet}\n" +
            $"Cost: {weaponInfo.cost}";
    }

    public void Perchase()
    {
        _shopSystem.PurchaseWeapon(WeaponInfo.name);
        gameObject.GetComponent<Button>().enabled = false;
    }
}
