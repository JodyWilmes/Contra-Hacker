using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class OnClickUIHandler : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler 
{ 
    // Start is called before the first frame update
    //add this script to an UI object to add an onclick event to it.
    public UnityEvent OnElementClick = new UnityEvent();
    public UnityEvent OnElementHoverOn = new UnityEvent();
    public UnityEvent OnElementHoverOff = new UnityEvent();

    public void OnPointerClick(PointerEventData eventData) // 3
    {
        OnElementClick.Invoke();
        Debug.Log("clickscript works");
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        OnElementHoverOn.Invoke();
        Debug.Log("hover on");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        OnElementHoverOff.Invoke();
        Debug.Log("hover off");
    }
    // Update is called once per frame

}
