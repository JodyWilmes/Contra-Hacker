using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Mailer : MonoBehaviour
{
    //this script has all the mails stored inside and sends a mail to the mailbox when asked. Mails are loaded in another script.
    //script picks a random mail based on difficulty
    // Start is called before the first frame update
    [SerializeField] private List<Mail> allMails;
    [SerializeField] private MailsGetter mailsGetter;
    [SerializeField] private Mailbox mailbox;
    void Start()
    {
        allMails.AddRange(mailsGetter.GetMailObjects()); //tells mailsgetter to do its job
    }

    // functions for getting a random mail
    public void SendWarningMail(MailHandleType type) //should be called when the player makes a mistake
    {
        mailbox.AddMail(ConstructMail(mailsGetter.GetWarningMail(type)));
    }

    public void SendPhishWarningMail()
    {
        mailbox.AddMail(ConstructMail(mailsGetter.GetPhishWarningMail()));
    }

    private ProcessedMail GetRandomMail() //returns a random mail
    {
        int random = (int)Mathf.Floor(Random.Range(0, allMails.Count)); //grabs random mail from the list to send
        //Debug.Log(random);
        //Debug.Log(allMails.Count);
        Mail randomMail = allMails[random];

        ProcessedMail mail = ConstructMail(randomMail);



        return mail;



    }
    public void onMailSent()
    {
        mailbox.AddMail(GetRandomMail());
    }
    private ProcessedMail ConstructMail(Mail inputMail)
    {
        ProcessedMail finishedMail = new ProcessedMail();
        //it should: rng check the phishchance, then pick red flags if applicable, then construct the mail
        finishedMail.isPhishing = (Random.Range(0.0f,1.0f) <= inputMail.phishChance);
        if (finishedMail.isPhishing)
        {
            //generate red flags. WIP
            /*Red flags are random, but subject is linked to body*/

            List<RedFlag> possibleFlags = new List<RedFlag> { RedFlag.wrongLogo, RedFlag.wrongSender, RedFlag.wrongBody, RedFlag.wrongAttachment, RedFlag.wrongLink, RedFlag.wrongReceiver };

            if(inputMail.hyperlinks.Length == 0) { possibleFlags.Remove(RedFlag.wrongLink); }
            if(inputMail.attachments.Length == 0) { possibleFlags.Remove(RedFlag.wrongAttachment); }
            if(inputMail.logoes.Length == 0) { possibleFlags.Remove(RedFlag.wrongLogo); }


            Debug.Log("is vishing");
            Debug.Log(possibleFlags);
            
            do
            {
                finishedMail.redFlags.Add(possibleFlags[Random.Range(0,possibleFlags.Count)]);
                Debug.Log("red flag toegevoegd");
            } while (finishedMail.redFlags.Count < finishedMail.difficulty) ;


            if (finishedMail.redFlags.Contains(RedFlag.wrongBody)) //body always has an odd subject attached
            {
                finishedMail.redFlags.Add(RedFlag.wrongSubject);
            }

        }

        //all the values will be set to the "real" one, then red flags are changed

        finishedMail.sender = inputMail.senders[0];
        finishedMail.recipient = inputMail.recipients[0];
        finishedMail.subject = inputMail.subjects[0];
        finishedMail.body = inputMail.bodies[0];
        finishedMail.hyperlink = inputMail.hyperlinks.Length == 0? null: inputMail.hyperlinks[0];
        finishedMail.attachment = inputMail.attachments.Length == 0 ? null : inputMail.attachments[0];
        finishedMail.logo = inputMail.logoes.Length == 0 ? null : inputMail.logoes[0];
        finishedMail.difficulty = inputMail.difficulty;
        finishedMail.isUrgent = inputMail.isUrgent;
        finishedMail.urgentMaxTime = inputMail.urgentMaxTime;
        finishedMail.mailHandleType= inputMail.mailHandleType;

        for (int i = 0; i < finishedMail.redFlags.Count; i++)
        {
            switch (finishedMail.redFlags[i])
            {
                case RedFlag.wrongSender: finishedMail.sender = inputMail.senders[1];  break;
                case RedFlag.wrongSubject: finishedMail.subject = inputMail.subjects[1];  break;
                case RedFlag.wrongReceiver: finishedMail.recipient = inputMail.recipients[1];  break;
                case RedFlag.wrongBody: finishedMail.body = inputMail.bodies[1]; break;
                case RedFlag.wrongLink: finishedMail.hyperlink = inputMail.hyperlinks.Length == 0 ? null : inputMail.hyperlinks[1]; break;
                case RedFlag.wrongAttachment: finishedMail.attachment = inputMail.attachments.Length == 0 ? null : inputMail.attachments[1]; break;
                case RedFlag.wrongLogo: finishedMail.logo = inputMail.logoes.Length == 0 ? null : inputMail.logoes[1]; break;
                default: break;
            }
        }

        return finishedMail;

    }
}

[System.Serializable]
public class ProcessedMail
{
    public string sender;
    public string recipient;
    public string subject;
    public string body;
    public string hyperlink;
    public string attachment;
    public Sprite logo;
    public bool isPhishing;
    public List<RedFlag> redFlags = new List<RedFlag>();
    public int difficulty;
    public bool isUrgent;
    public bool urgentMaxTime;
    public MailHandleType mailHandleType;
}