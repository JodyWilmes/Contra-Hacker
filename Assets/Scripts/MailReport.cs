using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MailReport : MonoBehaviour
{
    //this script will check the current mail's red flags against those selected. If correct, it will trigger an OnCorrectReport and an OnFalseReport event

    [SerializeField] private GameObject reportScreen;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Toggle toggleButton;
    [SerializeField] private List<RedFlag> chosenRedFlags;
    [SerializeField] private ProcessedMail currentMail;
    [SerializeField] private List<OnClickUIHandler> onClickUIHandlers; //everything in this list will be disabled on not report mode
    [SerializeField] public bool isReporting { get; private set; } = false;
    [SerializeField] public UnityEvent<bool> OnReportModeChanged = new UnityEvent<bool> ();
    // Start is called before the first frame update
    void Start()
    {
        foreach (OnClickUIHandler uiHandler in onClickUIHandlers)
        {
            uiHandler.enabled = toggleButton.isOn;
        }
    }
    public bool ReturnMode()
    {
        return toggleButton.isOn;
    }
    public List<RedFlag> GetFlags()
    {
        return chosenRedFlags;
    }
    public void ClearFlags()
    {
        chosenRedFlags.Clear();
    }
    public void ToggleReportScreen()
    {
        if (reportScreen != null)
        {
            reportScreen.SetActive(toggleButton.isOn);
            foreach (OnClickUIHandler uiHandler in onClickUIHandlers)
            {
                uiHandler.enabled = toggleButton.isOn;
            }
            Debug.Log("changed reporting to" +toggleButton.isOn);
            OnReportModeChanged.Invoke(toggleButton.isOn);
            chosenRedFlags.Clear ();
        }
    }

    public void AddReportedFlag(int flag)
    {
        if(toggleButton.isOn)
        {
            //public enum RedFlag { wrongSender, wrongReceiver, wrongSubject, wrongBody, wrongLink, wrongAttachment, wrongLogo }
            if (!chosenRedFlags.Contains((RedFlag)flag))
            {
                chosenRedFlags.Add((RedFlag)flag);
                confirmButton.interactable = true;
            }
            else
            {
                chosenRedFlags.Remove((RedFlag)flag);
                if(chosenRedFlags.Count <= 0)
                {
                    confirmButton.interactable = false;
                }
            }
        }
    }
    public bool CheckReportedFlags() //this will be subscribed to the button in the report screen
    {
        
        currentMail = Mailbox.Instance.GetSelectedMail();
        //go through every selected flag and see if it is in the current mail's red flags. Then go through every mail red flags and see if they are in the selected
        bool hasSameElements = true;
        if (currentMail.isPhishing)
        {
            List<RedFlag> mailFlags = currentMail.redFlags;
            foreach (RedFlag flag in chosenRedFlags)
            {
                if (!mailFlags.Contains(flag))
                {
                    hasSameElements = false;
                }
            }
            foreach (RedFlag flag in mailFlags)
            {
                if (!chosenRedFlags.Contains(flag))
                {
                    hasSameElements = false;
                }
            }
            if (mailFlags.Count == 0 || chosenRedFlags.Count == 0)
            {
                hasSameElements = false;
            }
        }
        else { hasSameElements = false; }
        return hasSameElements;
    }

    // Update is called once per frame
    private void OnDisable()
    {
        toggleButton.isOn = false;
        confirmButton.interactable= false;
    }
}
