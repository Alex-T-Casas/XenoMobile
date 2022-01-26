using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSwitcher : MonoBehaviour, IPointerDownHandler
{
    public delegate void OnWeaponSwitchPressed();
    public event OnWeaponSwitchPressed onWeaponSwitchPressed;
    public void OnPointerDown(PointerEventData eventData)
    {
        onWeaponSwitchPressed?.Invoke();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
