using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerStats Instance;
    public int rightHandles { get; private set; }
    public int wrongHandles { get; private set; }
    public int phishesFound { get; private set; }
    public int phishesFellFor { get; private set; }

    [SerializeField] private TextMeshProUGUI rightHandleText;
    [SerializeField] private TextMeshProUGUI wrongHandleText;
    [SerializeField] private TextMeshProUGUI rightPhishText;
    [SerializeField] private TextMeshProUGUI wrongPhishText;

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
    public void UpdateUI()
    {
        rightHandleText.text = $"Mails goed beantwoord: {rightHandles}";
        wrongHandleText.text = $"Mails fout beantwoord: {wrongHandles}";
        rightPhishText.text = $"Phishing-mails gevonden: {phishesFound}";
        wrongPhishText.text = $"Aantal keren gehackt: {phishesFellFor}";
    }
    public void AddRightHandle()
    {
        rightHandles++;
        UpdateUI();
    }
    
    public void AddWrongHandle() 
    { 
        wrongHandles++;
        UpdateUI();
    }

    public void AddPhishesFound()
    {
        phishesFound++;
        UpdateUI();
    }
    
    public void AddPhishesFailed()
    {
        phishesFellFor++;
        UpdateUI();
    }

}
