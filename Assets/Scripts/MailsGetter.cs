using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;



public class MailsGetter : MonoBehaviour
{

    //this script gets either the JSON or ScriptableObject containing the list of all mails and translates them
    //this script is early in the excecution order
    [SerializeField] private List<Mail> allMails;
    [SerializeField] private List<MailObject> allMailObjects;
    [SerializeField] private List<WarningMail> warningMails;
    [SerializeField] private MailObject phishWarningMail;

    [System.Serializable]
    private class WarningMail
    {
        [SerializeField] public MailObject warningMail;
        [SerializeField] public List<MailHandleType> warningtypes; //what types this mail responds to
    }


    // Start is called before the first frame update
    void Start()
    {
        //allMails = Resources.LoadAll<MailObject>("Mails");
        //Debug.Log(allMails.Length);
    }
    public List<Mail> GetMailObjects() { 
        
        foreach (MailObject mail in allMailObjects) {
            allMails.Add(ConstructMail(mail));
        }
        return allMails;
    }
    public Mail GetWarningMail(MailHandleType type)
    {
        MailObject mail;
        int index = 0;//get the index of the warningmail that contains the type in its warningtypes
        foreach (WarningMail warningMail in warningMails)
        {
            if(warningMail.warningtypes.Contains(type)) { 
                index = warningMails.IndexOf(warningMail);
                Debug.Log($"the warning mail is {index}");
                break;
            }
        }
        mail = warningMails[index].warningMail;
        return ConstructMail(mail);
    }

    public Mail GetPhishWarningMail() { return ConstructMail(phishWarningMail); }
    // Update is called once per frame
    private Mail ConstructMail(MailObject mailObject)
    {
        Mail mail = new Mail();


        /*
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
         
         */

        mail.senders = mailObject.senders;
        mail.recipients = mailObject.recipients;
        mail.subjects = mailObject.subjects;
        mail.bodies = mailObject.bodies;
        mail.hyperlinks= mailObject.hyperlinks;
        mail.attachments = mailObject.attachments;
        mail.logoes= mailObject.logoes;
        mail.phishChance= mailObject.phishChance;
        mail.difficulty= mailObject.difficulty;
        mail.isUrgent = mailObject.isUrgent;
        mail.urgentMaxTime = mailObject.urgentMaxTime;
        mail.mailHandleType = mailObject.handleType;


        return mail;
    }
}
