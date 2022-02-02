using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AbilityWheel : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public AbilityWidget closestWidget = null;
    [SerializeField] AbilityWidget[] abilityWidgets;
    public void OnDrag(PointerEventData eventData)
    {
        Vector3 widgetPos = GetComponent<RectTransform>().position;
        Vector2 wheelCenter = new Vector2(widgetPos.x, widgetPos.y);

        Vector2 DragDir = (eventData.position - wheelCenter).normalized;

        float closestAngle = 360.0f;

        foreach (AbilityWidget widget in abilityWidgets)
        {
            Vector3 widgetDir = -widget.transform.right;
            Vector2 widgetDir2D = new Vector2(widgetDir.x, widgetDir.y);

            float angle = Vector2.Angle(DragDir, widgetDir2D);
            if(angle <= closestAngle)
            {
                closestWidget = widget;
                closestAngle = angle;
            }
            widget.SetExpand(true);
        }
        closestWidget.SetHighLighted(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        foreach(AbilityWidget widget in abilityWidgets)
        {
            widget.SetExpand(true);
        }
    }

    internal void AddNewAbility(AbilityBase newAblity)
    {
        int level = newAblity.GetLevel();

        if(level - 1 < abilityWidgets.Length && level -1 >= 0)
        {
            abilityWidgets[level - 1].AssignAbility(newAblity);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        foreach (AbilityWidget widget in abilityWidgets)
        {
            widget.SetExpand(false);
        }
        
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
