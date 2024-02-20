using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnityMailHandleEvent : UnityEvent<MailHandleType>
{
}
public class MailInteraction : MonoBehaviour
{
    //script should subscribe to any buttons on creation

    /*
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
            public List<string> redFlags = new List<string>();
            public int difficulty;
            public bool isUrgent;
            public bool urgentMaxTime;
        }
     */



    //private MouseInteractor interactscript; (unused)
    [SerializeField] private int mailWorth;
    private Mailbox mailboxscript;
    private Mailer mailscript;
    [SerializeField] private Cryptolocker cryptolocker;
    public UnityMailHandleEvent OnGettingHacked = new UnityMailHandleEvent();
    public UnityEvent OnCorrectHandle;
    public UnityMailHandleEvent OnFalseHandle = new UnityMailHandleEvent();
    public UnityEvent OnMouseDownInput; //is used to delay things to show feedback
    
    [SerializeField] private MailReport mailReporter;
    //mail parent
    [SerializeField] private EventSystem canvasSystem;
    [SerializeField] private List<ToggleOverlayUI> overlayUIs;

    [SerializeField] private GameObject currentMailGO;

    //all text fields that should change
    [SerializeField] private List<GameObject> mailListItems= new List<GameObject>();
    [SerializeField] private TextMeshProUGUI subjectText;
    [SerializeField] private TextMeshProUGUI senderText;
    [SerializeField] private TextMeshProUGUI recipientText;
    [SerializeField] private TextMeshProUGUI bodyText;
    [SerializeField] private TextMeshProUGUI hyperlinkText;
    [SerializeField] private TextMeshProUGUI attachmentText;
    [SerializeField] private UnityEngine.UI.Image logoSprite;

    //all optional parts of the mail
    [SerializeField] private GameObject hyperlink;
    [SerializeField] private GameObject attachment;
    [SerializeField] private GameObject logo;

    [SerializeField] private GameObject listItemPrefab;
    [SerializeField] private GameObject mailList;

    // Start is called before the first frame update
    void Start()
    {
        //interactscript = GameObject.FindGameObjectsWithTag("InteractionDetectionScript")[0].GetComponent<MouseInteractor>();
        //interactscript.distributeClicked.AddListener(OnMailSelected);
        mailboxscript = GameObject.FindGameObjectWithTag("Mailbox").GetComponent<Mailbox>();
        mailscript = GameObject.FindGameObjectWithTag("Mailer").GetComponent<Mailer>();
        mailboxscript.onMailboxAdded.AddListener(OnMailboxAdded);
        mailboxscript.onMailboxRemoved.AddListener(OnMailboxRemoved);
        //OnFalseHandle.AddListener(mailscript.SendWarningMail);
        OnFalseHandle.AddListener((MailHandleType mailhandletype) => { 
            switch(mailhandletype)
            {
                case MailHandleType.clickDelete: DialogueManager.Instance.AddDialogue("Hmm, ik denk dat die mail belangrijk was. Die had ik misschien niet moeten verwijderen of rapporteren."); break; 
                case MailHandleType.clickLink: DialogueManager.Instance.AddDialogue("Hmm, ik heb het idee dat ik niet op de link moest klikken hier."); break; 
                case MailHandleType.clickForward: DialogueManager.Instance.AddDialogue("Ik denk niet dat die mail voor iemand anders bestemd was."); break; 
                case MailHandleType.clickAttachment: DialogueManager.Instance.AddDialogue("Volgens mij moest ik die attachment niet uitprinten."); break; 
                default:break;
            }
            
            
            //PlayerPossessions.Instance.AddMoney(mailWorth); 
        });
        OnCorrectHandle.AddListener(() => { PlayerPossessions.Instance.AddMoney(mailWorth); });
        OnCorrectHandle.AddListener(PlayerStats.Instance.AddRightHandle);
        OnGettingHacked.AddListener(HandleHacked);
        OnGettingHacked.AddListener((MailHandleType handleType) => { PlayerStats.Instance.AddPhishesFailed(); });
        OnFalseHandle.AddListener((MailHandleType handleType) => { PlayerStats.Instance.AddWrongHandle(); });
    }

    private void Update()
    {
        //check inputs for if the mouse is down, and fires the onInputMouseDown event.
        if(Input.GetMouseButtonDown(0) && !DialogueManager.Instance.inDialogue) //don't be in dialogue
        {
            OnMouseDownInput.Invoke();
        }
    }

    private void OnMailboxAdded(int index)
    {
        List<ProcessedMail> mailbox = mailboxscript.getMailbox();
        ProcessedMail addedMail = mailbox[index]; //is always correct cause add() is used, though it would be better if the event could pass the mail.
        GameObject item = Instantiate(listItemPrefab, mailList.transform);
        MailPreviewIndex previewValues = item.GetComponentInChildren<MailPreviewIndex>();
        previewValues.subjectText.text = addedMail.subject;
        previewValues.senderText.text = addedMail.sender;
        previewValues.button.onClick.AddListener(OnMailSelected);
        mailListItems.Add(item);
    }
    
    private void OnMailboxRemoved(int index)
    {
        //this should remove the gameobject from the list and the gameobject itself, and set the mail itself to inactive.
        Destroy(mailListItems[index]);
        mailListItems.RemoveAt(index);
        currentMailGO.SetActive(false);
    }

    private void OnMailSelected() { //this is subscribed to the buttons
            GameObject listItem = EventSystem.current.currentSelectedGameObject.transform.parent.gameObject; //this is hella scuffed
            Debug.Log(listItem);
            mailboxscript.ChangeSelectedMail(mailListItems.IndexOf(listItem));
            ProcessedMail mail = mailboxscript.GetSelectedMail();
            //set all relevant text values
            SetMailValues(mail);
        currentMailGO.SetActive(true);
    }

    public void HandleMailInteraction(int interaction) //is attached to the mail's buttons
    {
        if (!mailReporter.ReturnMode())
            {
            Debug.Log("reporting is " + mailReporter.isReporting);
            MailHandleType interactionEnum = (MailHandleType)interaction;
            //0 = delete, 1 = attachment, 2 = link, 3 = forward
            ProcessedMail mail = mailboxscript.GetSelectedMail();
            /*options: 
               -delete never triggers phish, but can be bad job'd
               -link and attachment trigger phish
               -forward triggers angry boss
            does not trigger when reporting  
         
             */

            if (mail != null)
            {
                if (!mail.isPhishing || (interactionEnum == MailHandleType.clickDelete)) //if it's not phishing or if you're deleting
                {
                    if (mail.mailHandleType == interactionEnum)
                    {
                        if (interactionEnum != MailHandleType.clickDelete)
                        {
                            Debug.Log("good job"); //get money
                            OnCorrectHandle.Invoke();
                        }
                    }
                    else { 
                        if (!mail.isPhishing)
                        {
                            Debug.Log("bad job"); //lose money or something
                            OnFalseHandle.Invoke(interactionEnum); //report what you did
                        }
                    }
                    mailboxscript.RemoveSelectedMail();
                }
                else
                {
                    //highlight the red flags. Make sure the red flags are done in order.
                    HighlightRedFlags(mail.redFlags, Color.red);
                    ExplainRedflags(mail.redFlags);
                    canvasSystem.enabled = false;
                    OnMouseDownInput.AddListener(() => { //this makes the thing buffer

                        Debug.Log($"You got hacked! The red flags were: {mail.redFlags}");
                        if (interactionEnum != MailHandleType.clickDelete)
                        {

                            OnGettingHacked.Invoke(interactionEnum);
                        }
                        foreach(ToggleOverlayUI overlayUI in overlayUIs)
                        {
                            overlayUI.ResetOverlay();
                        }
                        canvasSystem.enabled = true;
                        mailboxscript.RemoveSelectedMail();
                        OnMouseDownInput.RemoveAllListeners();
                    });
                }
            }
        }
    }

    private void HighlightRedFlags(List<RedFlag> redflags,Color color) {
        //cast the enum list to an int list
        List<int> redflagIndices = new List<int>();
        for(int i = 0; i < redflags.Count; i++)
        {
            redflagIndices.Add((int)redflags[i]);
        }

        //see if an overlayUI's index is in redflagindices, if so, apply color

        for(int i = 0; i < overlayUIs.Count; i++)
        {
            if (redflagIndices.Contains(i))
            {
                //do the color thing
                ToggleOverlayUI overlayUI = overlayUIs[i];

                overlayUI.SetOverlay(true);
                overlayUI.SetOverlayColor(color);

            }
        }

    
    }

    private void HandleHacked(MailHandleType interaction)
    {
        if(interaction == MailHandleType.clickForward)
        {
            mailscript.SendPhishWarningMail();
        }
        else {
            //lock screen
            cryptolocker.OnHacked();
        }
    }

    public void ReportCurrentMail() //checks the report button, subscribed to confirm on report screen
    {
        ProcessedMail mail = mailboxscript.GetSelectedMail();
        if (mail != null)
        {
            if (mail.isPhishing) //don't false report an important mail!
            {

                if (mailReporter.CheckReportedFlags())
                {
                    //correct!
                    HighlightRedFlags(mail.redFlags, Color.green);
                    DialogueManager.Instance.AddDialogue("Yes, ik heb de phishing mail gevonden en alles goed gerapporteerd!");
                    PlayerStats.Instance.AddPhishesFound();
                    //cleans up the report screen
                    canvasSystem.enabled = false; //disable canvas buttons
                    OnMouseDownInput.AddListener(() => { //this makes the thing buffer
                        OnCorrectHandle.Invoke();
                        foreach (ToggleOverlayUI overlayUI in overlayUIs)
                        {
                            overlayUI.ResetOverlay();
                        }
                        mailboxscript.RemoveSelectedMail();
                        canvasSystem.enabled = true; //turn buttons back on
                        OnMouseDownInput.RemoveAllListeners();
                    });

                }
                else
                {
                    //you didn't find everything... but i'll let you live
                    //make a list of redflags you missed
                    List<RedFlag> missedflags = new List<RedFlag>(); //- all flags you have;
                    List<RedFlag> correctflags = new List<RedFlag>(); //- all flags you have;
                    List<RedFlag> pickedflags = mailReporter.GetFlags();


                    foreach (RedFlag flag in mail.redFlags)
                    {
                        if (!pickedflags.Contains(flag))
                        {
                            missedflags.Add(flag);
                        }
                        else { correctflags.Add(flag); }
                    }

                    HighlightRedFlags(missedflags, Color.yellow);

                    ExplainRedflags(missedflags);

                    HighlightRedFlags(correctflags, Color.green);
                    canvasSystem.enabled = false;
                    OnMouseDownInput.AddListener(() => { //this makes the thing buffer
                        foreach (ToggleOverlayUI overlayUI in overlayUIs)
                        {
                            overlayUI.ResetOverlay();
                        }
                        mailboxscript.RemoveSelectedMail();
                        canvasSystem.enabled = true;
                        OnMouseDownInput.RemoveAllListeners();
                    });

                }

            }
            else { OnFalseHandle.Invoke(MailHandleType.clickDelete); 
            
            mailboxscript.RemoveSelectedMail();
            //cleans up the report screen
            } //you wrongly reported
            mailReporter.ClearFlags();
        }
    }

    private void ExplainRedflags(List<RedFlag> flags)
    {
        foreach (RedFlag missedFlag in flags) //tell the player what they missed
        {
            switch (missedFlag)
            {
                case RedFlag.wrongSubject: DialogueManager.Instance.AddDialogue("Oei, de mail heeft een vreemd onderwerp."); break;
                case RedFlag.wrongSender: DialogueManager.Instance.AddDialogue("De afzender is mij onbekend of hij lijkt op een andere vertrouwde."); break;
                case RedFlag.wrongReceiver: DialogueManager.Instance.AddDialogue("Ik vind dit iets te veel ontvangers hebben voor de context van de mail!"); break;
                case RedFlag.wrongBody: DialogueManager.Instance.AddDialogue("Deze body zet me onder tijdsdruk en heeft taal/spelfouten."); break;
                case RedFlag.wrongLink: DialogueManager.Instance.AddDialogue("Deze link is verdacht. De site is mij niet bekend."); break;
                case RedFlag.wrongAttachment: DialogueManager.Instance.AddDialogue("Ik vertrouw die .exe of .bat file niet. Die kunnen programmas draaien."); break;
                case RedFlag.wrongLogo: DialogueManager.Instance.AddDialogue("Dit logo ziet er nagemaakt uit."); break;
            }
        }
    }

    private void SetMailValues(ProcessedMail mail_)
    {
        subjectText.text = mail_.subject;
        senderText.text = "From: "+mail_.sender;
        recipientText.text = "To: "+mail_.recipient;
        bodyText.text = mail_.body;
        if (!string.IsNullOrEmpty(mail_.hyperlink))
        { //doesn't show attachment
            hyperlinkText.text = mail_.hyperlink;
            hyperlink.gameObject.SetActive(true);
        }
        else
        {
            hyperlink.gameObject.SetActive(false);
            //Debug.Log("no attachment");

        }
        if (!string.IsNullOrEmpty(mail_.attachment)) { //doesn't show attachment
            attachmentText.text = mail_.attachment;
            attachment.gameObject.SetActive(true);
        }
        else
        {
            attachment.gameObject.SetActive(false);
            //Debug.Log("no attachment");

        }
        if(mail_.logo != null) //doesn't show the logo if there is none
        {
            logoSprite.sprite = mail_.logo;
            logo.gameObject.SetActive(true);
        }
        else
        {
            logo.gameObject.SetActive(false);
            //Debug.Log("no logo");
        }
    }
}
