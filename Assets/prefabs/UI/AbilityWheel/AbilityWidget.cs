using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWidget : MonoBehaviour
{
    AbilityBase ability;
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform icon;
    [SerializeField] RectTransform CoolDown;
    private float scaleSpeed = 20.0f;

    private Vector3 GoalScale = new Vector3(1, 1, 1);
    [SerializeField] float ExpandedScale = 2.0f;
    [SerializeField] float HighlightedScale = 2.5f;
    private bool isExpanded;

    Material CooldownMatt;
    void Start()
    {
        CooldownMatt = Instantiate(CoolDown.GetComponent<Image>().material);
        CoolDown.GetComponent<Image>().material = CooldownMatt;
    }

    void Update()
    {
        
            background.localScale = Vector3.Lerp(background.localScale, GoalScale, Time.deltaTime * scaleSpeed);
    }

    void SetCooldownProgress(float progress)
    {
        CooldownMatt.SetFloat("_Progress", progress);
    }

    internal void SetExpand(bool isExpanded)
    {
        if(isExpanded)
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
        else
        {
            if (isHighLighted())
            {
                ability.ActivateAbility();
            }
            GoalScale = new Vector3(1, 1, 1);
        }
    }

    internal void SetHighLighted(bool isHighlighted)
    {
        if (isHighlighted)
        {
            GoalScale = new Vector3(1, 1, 1) * HighlightedScale;
        }
        else
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
    }

    private bool isHighLighted()
    {
        return GoalScale == new Vector3(1, 1, 1) * HighlightedScale;
    }

     public Vector2 GetCenter()
    {
        return background.rect.center;
    }

    internal void AssignAbility(AbilityBase newAblity)
    {
        ability = newAblity;
        icon.GetComponent<Image>().sprite = ability.GetIcon();
    }
}
