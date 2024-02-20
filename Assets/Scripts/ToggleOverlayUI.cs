using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToggleOverlayUI : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Image overlay;
    private Color initialColor;
    private bool selected = false;

    //when clicked, it should turn red, when hovered, should turn grey if not selected.


    private void Start()
    {
        initialColor= overlay.color;
    }
    public void ToggleOverlay() //handles entire overlay
    {
        if(selected)
        {
            overlay.enabled = true;
            SetOverlayColor(initialColor);
        }
        else
        {
            overlay.enabled= !overlay.enabled;
            SetOverlayColor(new Color(0.9f,0.9f,0.9f)); //light grey
        }
    }
    public void ToggleOverlaySelection()
    {
        selected= !selected;
        overlay.enabled = false;
    }

    public void SetOverlay(bool setting) //is only used in toggle report screen
    {
        overlay.enabled = setting;
        if(setting == false)
        {
            selected = false;
        }
 
    }
    public void SetOverlayColor(Color color)
    {
        overlay.color = color;
    }
    public void ResetOverlay()
    {
        overlay.enabled = false;
        overlay.color= initialColor;
        selected= false;
    }
}
