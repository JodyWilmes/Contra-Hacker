using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerUIScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI clockTextField;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }
    void UpdateUI()
    {
        int minutes = DayManager.Instance.dayTimer;
        int hours = (int)Mathf.Floor(minutes/60) + DayManager.Instance.startingHour;
        minutes%=60;
        clockTextField.text = $"{hours/10}{hours%10}:{minutes/10}{minutes%10}";
    }
}
