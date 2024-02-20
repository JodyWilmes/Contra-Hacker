using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDayScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MouseInteractor mouseInteractor;
    [SerializeField] private GameObject endScreen;
    [SerializeField] private GameObject finalScreen;
    void Start()
    {
        DayManager.Instance.onDayEnd.AddListener(EndDay);
    }

    // Update is called once per frame
    public void EndDay()
    {
        endScreen.SetActive(true);
        mouseInteractor.enabled= false;

        if (DayManager.Instance.dayCount >= DayManager.Instance.Days.Count) //this is the final day
        {
            finalScreen.SetActive(true);
        }

    }
    public void StartNextDay() //subscribed to button on endscreen
    {
        DayManager.Instance.StartDay(DayManager.Instance.dayCount);
        endScreen.SetActive(false);
        mouseInteractor.enabled = true;
        Mailbox.Instance.ClearAllMails();
    }
}
