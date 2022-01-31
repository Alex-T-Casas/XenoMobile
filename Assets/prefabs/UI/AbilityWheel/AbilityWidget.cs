using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityWidget : MonoBehaviour
{
    [SerializeField] RectTransform background;
    [SerializeField] RectTransform icon;
    private float scaleSpeed = 20.0f;

    private Vector3 GoalScale = new Vector3(1, 1, 1);
    [SerializeField] float ExpandedScale = 2.0f;
    [SerializeField] float HighlightedScale = 2.5f;
    private bool isExpanded;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
            background.localScale = Vector3.Lerp(background.localScale, GoalScale, Time.deltaTime * scaleSpeed);
    }

    internal void SetExpand(bool isExpanded)
    {
        if(isExpanded)
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
        else
        {
            GoalScale = new Vector3(1, 1, 1);
        }
    }

    internal void SetHighLighted(bool isHighLighted)
    {
        if(isHighLighted)
        {
            GoalScale = new Vector3(1, 1, 1) * HighlightedScale;
        }
        else
        {
            GoalScale = new Vector3(1, 1, 1) * ExpandedScale;
        }
    }

     public Vector2 GetCenter()
    {
        return background.rect.center;
    }
}
