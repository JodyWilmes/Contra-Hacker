using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;
using UnityEngine.UI;
using Unity.VisualScripting;
using TMPro;

public class MailMaker : MonoBehaviour
{
    [SerializeField] private GameObject MailObject;
    List<GameObject> MailObjectsList = new List<GameObject>();

    private string AllMailsString;          // String in which all the mails will be collected together.
    List<MailItem> MailsList = new List<MailItem>();
    string[] SeperatedMails;

    void Start()
    {
        StartCoroutine(GetRequest("https://jodywilmes.github.io/Stage/MailInfo/Mails.html"));   // The link to the page that will contain all of the information for the mails
    }

    IEnumerator GetRequest(string uri)              // Coroutine that gets all the info from the webpage
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    // AllMailsString = pages[page] + ":\nReceived: " + webRequest.downloadHandler.text;
                    AllMailsString = webRequest.downloadHandler.text;
                    //Debug.Log(AllMailsString);
                    CreateMails();
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }
    
     private void CreateMails()
     {
        SeperateMails();
        SplitMails();
        // SeperateMails2();
     }
    
    // First seperations for the mails, here you seperate each seperate complete mail. The mails 
    // will after this all be as seperate strings, nothing inside the mail will be seperated yet.
    // Than make a object for every mail
     private void SeperateMails()
     {
        string[] MailsArray = AllMailsString.Split("*****");

        foreach (string _mail in MailsArray)
        {
            // Debug.Log(Mail);
            MailItem newMail = new MailItem(_mail);  // Create new mail for each mail in the array, where the fullMail gets assigned as what the string was
            MailsList.Add(newMail);         // Add the mail to a list with all the mails
        }
     }

     private void SplitMails()
     {
        string mailContent;
        int totalAmountOfItems = 0;
        foreach (MailItem _mail in MailsList)           // First for each mail split the big string with all the information
        {
            mailContent = _mail.CompleteMail;
            string[] looseMailParts = mailContent.Split("+++");

            foreach (string mailPart in looseMailParts)
            {
                totalAmountOfItems+=1;
            }
            // Debug.Log("total amount of items in the list:" + totalAmountOfItems);
            if (totalAmountOfItems == 17)           // Checks if the total amount of items to make the mail is 17, if its not the mail is unusable
            {
                _mail.ValidMail = true;
                ValueAssigning(_mail, looseMailParts);
            }
            else if (totalAmountOfItems > 17 || totalAmountOfItems < 17)
            {
                _mail.ValidMail = false;
            }
            totalAmountOfItems = 0;
        }
     }

    private void ValueAssigning(MailItem _mail, string[] _mailParts) // Here all the values get assigned to what they should be, to be able to acces them easier, this is its own function bc its a lot of lines, all almost doing the same
    {
        _mail.PhishingKans = Int32.Parse(_mailParts[0]);        // Gezien dit een int is, maar in de lijst enkel strings staan moet deze eerst omgezet worden in een int
        _mail.PlayerActie = _mailParts[1];
        _mail.FeedbackCorrect = _mailParts[2];
        _mail.FeedbackIncorrect = _mailParts[3];
        _mail.Moeilijkheid = Int32.Parse(_mailParts[4]);        // Gezien dit een int is, maar in de lijst enkel strings staan moet deze eerst omgezet worden in een int

        _mail.NormaleTitel = _mailParts[5];
        _mail.PhishingTitel = _mailParts[6];
        _mail.NormaleAfzender = _mailParts[7];
        _mail.PhishingAfzender = _mailParts[8];
        _mail.NormaleOntvanger = _mailParts[9];
        _mail.PhishingOntvanger= _mailParts[10];
        _mail.NormaleBody = _mailParts[11];
        _mail.PhishingBody = _mailParts[12];
        _mail.NormaleLink = _mailParts[13];
        _mail.PhishingLink = _mailParts[14];
        _mail.NormaleBijlage = _mailParts[15];
        _mail.PhishingBijlage= _mailParts[16];

        int randomNr = Random.Range(1, 101);        // Pak een random nummer tussen 1 en 100, als dit nummer minder is, of gelijk is aan de phising kans dan is de mail phishing, anders is het false
        if (randomNr <= _mail.PhishingKans)
        {
            _mail.IsPhishing = true;
            _mail.PhishingMail();
        }
        else if (randomNr > _mail.PhishingKans)
        {
            _mail.IsPhishing = false;
            _mail.NormalMail();
        }

        if (_mail.PlayerActie == "DOORSTUREN" || _mail.PlayerActie == "VERWIJDEREN" || _mail.PlayerActie == "RAPPORTEREN")          // Controleer of er een geldige actie is die de player moet doen
        {
            _mail.ValidMail = true;
        }
        else
        {
            _mail.ValidMail= false;
        }

        if (_mail.NormaleLink == "NONE" || _mail.PhishingLink == "NONE")                // Controleer of de mail een link heeft
        {
            _mail.HeeftLink = false;
        }
        else
        {
            _mail.HeeftLink = true;
        }

        if (_mail.NormaleBijlage == "NONE" || _mail.PhishingBijlage == "NONE")          // Controleer of de mail een Bijlage heeft
        {
            _mail.HeeftBijlage = false;
        }
        else
        {
            _mail.HeeftBijlage = true;
        }

        if (_mail.ValidMail)
        {
            MakeObject(_mail);
        }
    }

    private void MakeObject(MailItem mail)
    {
        Instantiate(MailObject);                // Make the gameobject for the mail

        for (int i = 0; i < MailObject.transform.childCount; i++)
        {
            GameObject child = MailObject.transform.GetChild(i).gameObject;         // Search for all the children the gameobject has, and than for each one check the tag of the object, and if it is the same, set the text of the component to the text it needs
            
            if (child.tag == "Titel")
            {
                SetChildComponent(child, mail.Titel);
            }
            else if (child.tag == "Afzender")
            {
                SetChildComponent(child, mail.Afzender);
            }
            else if (child.tag == "Ontvanger")
            {
                SetChildComponent(child, mail.Ontvanger);
            }
            else if (child.tag == "Body")
            {
                SetChildComponent(child, mail.Body);
            }
            else if (child.tag == "Link")
            {
                SetChildComponent(child, mail.Link);
            }
            else if (child.tag == "Link" && !mail.HeeftLink)
            {
                child.SetActive(false);
            }
            else if (child.tag == "Bijlage")
            {
                SetChildComponent(child, mail.Bijlage);
            }
            else if (child.tag == "BijlageObject" && !mail.HeeftBijlage)
            {
                child.SetActive(false);
            }
        }
        MailObject.SetActive(false);
        MailObjectsList.Add(MailObject);
    }

    private void SetChildComponent(GameObject ChildObject, string MailComponentText)        // Set the childObject, the text component to the corresponding text from the mail
    {
        TMP_Text TextComponent = ChildObject.GetComponent<TMP_Text>();
        if (TextComponent != null)
        {
            TextComponent.text = MailComponentText;
        }
    }

    /*
     * Instantiate new gameobject for each valid mail
     * get child objects from this gameobject
     * add child objects to a list with all objects
     * go past all items in the list, if tag = one of the tags youre looking for (Titel, Afzender, ect), set the string to the text component
     * set the gameObject to be inactive
     */
    
}
public class MailItem
{
    #region values
    public bool ValidMail;              // To see if the mail has all the information it needs and is useable
    public string CompleteMail;         // The full mail, which gets seperated in the other fields

//                                   Nr in array            Omscrhijving
    public int PhishingKans;            // 0        // Checkt hoeveel % kans de mail heeft om phishing te zijn
    public string PlayerActie;          // 1        // Checkt wat de speler moet doen met de mail, dit is dus DOORSTUREN, VERWIJDEREN, RAPPORTEREN
    public string FeedbackCorrect;      // 2        // Feedback als de speler het goed heeft gedaan
    public string FeedbackIncorrect;    // 3        // Feedback als de speler het niet goed heeft gedaan
    public int Moeilijkheid;            // 4        // Hoe moeilijk dat de mail is om te herkennen/wat er mee gedaan moet worden

    public string Titel;                            // Titel
    public string NormaleTitel;         // 5        // Normale titel
    public string PhishingTitel;        // 6        // Phishing titel
    public string Afzender;                         // Afzender
    public string NormaleAfzender;      // 7        // Normale afzender
    public string PhishingAfzender;     // 8        // Phishing afzender
    public string Ontvanger;                        // Ontvanger
    public string NormaleOntvanger;     // 9        // Normale ontvanger
    public string PhishingOntvanger;    // 10       // Phising ontvanger
    public string Body;                             // Body
    public string NormaleBody;          // 11       // Normale body
    public string PhishingBody;         // 12       // Phishing body
    public string Link;                             // Link
    public string NormaleLink;          // 13       // Normale link, optioneel, als er staat ^^^NONE^^^ dan is het leeg en wordt de bool HeeftLink false
    public string PhishingLink;         // 14       // Physhing link, optioneel, als er staat ^^^NONE^^^ dan is het leeg en wordt de bool HeeftLink false
    public string Bijlage;                          // Bijlage
    public string NormaleBijlage;       // 15       // Normale bijlage, optioneel, als er staat ^^^NONE^^^ dan is het leeg en wordt de bool HeeftBijlage false
    public string PhishingBijlage;      // 16       // Phishing bijlage, optioneel, als er staat ^^^NONE^^^ dan is het leeg en wordt de bool HeeftBijlage false

    public bool IsPhishing;             // Checkt of de mail phishing is, en dus welke onderdelen gebruikt moeten worden in de mail
    public bool HeeftLink;              // Checkt of de mail een bijlage heeft, als dit niet zo is dan wordt deze dus ook niet weergeven
    public bool HeeftBijlage;           // Checkt of de mail een bijlage heeft, als dit niet zo is dan wordt deze dus ook niet weergeven
    #endregion


    public MailItem(string FullMail)
    {
        CompleteMail = FullMail;
        
    }

    // Functies om de variables van titel, afzender, ect, naar de juiste te zetten (normaal of phishing), waardoor het later makkelijker is om deze te gebruiken
    public void NormalMail()
    {
        Titel = NormaleTitel;
        Afzender = NormaleAfzender;
        Ontvanger = NormaleOntvanger;
        Body = NormaleBody;
        Link = NormaleLink;
        Bijlage = NormaleBijlage;
    }

    public void PhishingMail()
    {
        Titel = PhishingTitel;
        Afzender = PhishingAfzender;
        Ontvanger = PhishingOntvanger;
        Body = PhishingBody;
        Link = PhishingLink;
        Bijlage = PhishingBijlage;
    }
}