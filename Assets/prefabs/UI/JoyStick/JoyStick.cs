using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform Handle;
    [SerializeField] RectTransform Background;
    [SerializeField] RectTransform Pivot;

    public Vector2 Input
    {
        get;
        private set;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 DragPos = eventData.position;
        Vector2 BgPos = Background.position;

        Debug.DrawLine(DragPos, BgPos);

        Input = Vector2.ClampMagnitude(DragPos - BgPos, Background.rect.width / 2);
        Handle.localPosition = Input;
        Input = Input / (Background.rect.width / 2);
        //Debug.Log(Input);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 Location = eventData.position;
        Pivot.position = Location;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Handle.position = Background.position;
        Pivot.localPosition = Vector2.zero;
        Input = Vector2.zero;
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
