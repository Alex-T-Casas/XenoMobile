using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] RectTransform Handle;
    [SerializeField] RectTransform Background;
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 DragPos = eventData.position;
        Vector2 BgPos = Background.position;

        Debug.DrawLine(DragPos, BgPos);

        Handle.localPosition = Vector2.ClampMagnitude(DragPos - BgPos, Background.rect.width/2);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer down");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Pointer Up");
        Handle.position = Background.position;
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
