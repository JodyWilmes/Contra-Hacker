using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityIntEvent : UnityEvent<int>
{
}
public class Mailbox : MonoBehaviour
{

    public static Mailbox Instance;
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


    public UnityIntEvent onMailboxAdded = new UnityIntEvent();
    public UnityIntEvent onMailboxRemoved = new UnityIntEvent();
    // Start is called before the first frame update
    [SerializeField] private List<ProcessedMail> mailbox;
    private ProcessedMail selectedMail;

    private void Start()
    {
        onMailboxRemoved.AddListener((i) => {
            if (DayManager.Instance.allSent && mailbox.Count == 1) //order of added functions makes this 1
            {
                DialogueManager.Instance.AddDialogue("Ik geloof dat dat alle mails waren. Ik kan de dag beëindigen.");
            }
            
             
        });
    }
    public void AddMail(ProcessedMail mail)
    {
        mailbox.Add(mail);
        onMailboxAdded.Invoke(mailbox.IndexOf(mail));
    }
    public void ClearAllMails()
    {
        int repeats = mailbox.Count;
        for (int i = 0; i < repeats; i++)
        {
            onMailboxRemoved.Invoke(0);
            mailbox.Remove(mailbox[0]);
        }
    }
    public void RemoveMail(ProcessedMail mail) { 
        onMailboxRemoved.Invoke(mailbox.IndexOf(mail));
        mailbox.Remove(mail);
    }

    public void RemoveSelectedMail()
    {
        ProcessedMail mail = selectedMail;
        onMailboxRemoved.Invoke(mailbox.IndexOf(mail));
        mailbox.Remove(mail);
    }
    public ProcessedMail GetSelectedMail()
    {
        return selectedMail;
    }


    public void ChangeSelectedMail(int index)
    {
        selectedMail= mailbox[index]; //for testing
    }
    public List<ProcessedMail> getMailbox()
    {
        return mailbox;
    }
    public void DeselectMail() { 
        selectedMail= null; 
    }

}

[System.Serializable]
public class Mail
{
    /* This is the template for mails. It should have: 
 -Sender
 -Receiver(s)
 -Date sent
 -Subject
 -Body
 -Hyperlink
 -Attachment
 -Logo
 -Phishing Chance
 -Difficulty
 Mails both have a "real" and a "fake" value for these. Each field is an array, so you can add multiple fake options. 
 Phishing chance is how often the mail is fake.
 Difficulty is used when choosing a mail and depends on the day.
 */
    public string[] senders;
    public string[] recipients;
    public string[] subjects;
    public string[] bodies;
    public string[] hyperlinks;
    public string[] attachments;
    public Sprite[] logoes;
    public float phishChance;
    public int difficulty;
    public bool isUrgent;
    public bool urgentMaxTime;
    public MailHandleType mailHandleType;
}
