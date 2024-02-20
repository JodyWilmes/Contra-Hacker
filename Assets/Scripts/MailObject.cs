using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class MailObject : ScriptableObject
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
    public MailHandleType handleType;


}
