using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Day : ScriptableObject
{
    public int currentDay; //day's id
    public int time;
    public int dayLength; //usually x seconds but the day may start late
    public List<int> messageSpread; //this details when messages come in, at which time.
    public int difficulty; //picks mails of this difficulty more easily

    public List<GuaranteedMail> guaranteedMails = new List<GuaranteedMail>();


}
    [System.Serializable]
    public class GuaranteedMail
    {
        public int time;
        public ProcessedMail guaranteedMail;
    }