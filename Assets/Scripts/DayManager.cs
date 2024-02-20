using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayManager : MonoBehaviour
{
    //this script can start a timer which calls a message event. Mailbox will listen to this event and call mailer's getmail
    // Start is called before the first frame update
    [SerializeField] public List<Day> Days; //set these scriptable objects in the editor
    public Day currentDay { get; private set; }
    public int dayTimer { get; private set; }
    public int dayCount { get; private set; } = 0;
    public int startingHour;
    public UnityEvent notifyMessagers;
    public UnityEvent onDayEnd;
    private bool dayEnded = false;
    public bool allSent = false; //if all mails are sent
    public static DayManager Instance;
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself. Prevents dupe scripts.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        //StartDay(0);
    }

    IEnumerator UpdateDay()
    {
        while ((dayTimer < currentDay.dayLength) && !dayEnded)
        {

            var lastMailSent = true; //did we get all mails?
            foreach (int timeframe in currentDay.messageSpread) //checks every mail that should be sent right now
            {
                if(timeframe == dayTimer)
                {
                    notifyMessagers.Invoke();
                    //Debug.Log("Message sent");
                }

                if (timeframe >= dayTimer) //there is still a mail that can be sent
                {
                    lastMailSent = false;
                }

            }

            foreach (GuaranteedMail guaranteedMail in currentDay.guaranteedMails)
            {
                if(guaranteedMail.time == dayTimer)
                {
                    Mailbox.Instance.AddMail(guaranteedMail.guaranteedMail);
                }

                if (guaranteedMail.time >= dayTimer) //there is still a mail that can be sent
                {
                    lastMailSent = false;
                }

            }

            allSent = lastMailSent;

            if(dayTimer == currentDay.dayLength - 30)
            {
                DialogueManager.Instance.AddDialogue("De dag is over 30 minuten voorbij. Ik kan maar beter opschieten.");
            }

            yield return new WaitForSeconds(1f);
            dayTimer++;
            //Debug.Log("seconde voorbij");
            //give a warning the day is about to end
        }
        Debug.Log("Day is Over");
        dayCount++;
        dayTimer = 0;
        dayEnded= false;
        allSent= false;
        onDayEnd.Invoke();
    }

    public void StartDay(int daynumber) { 
        if(dayCount < Days.Count) //make sure it's not outside array bounds
        {
            currentDay= Days[daynumber];
            StartCoroutine(UpdateDay());
        }
    }

    public void EndDayForced()
    {
        dayEnded = true;
        Debug.Log("day ends now");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
