using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    [SerializeField] private List<string> bufferedDialogue = new List<string>();
    [SerializeField] private GameObject dialogueBoxPrefab;
    [SerializeField] private GameObject currentDialogueBox;
    [SerializeField] private GameObject canvas;
    [SerializeField] private EventSystem canvasSystem;
    public bool inDialogue;

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
    // Start is called before the first frame update
    public void AddDialogue(string dialogue)
    {
        bufferedDialogue.Add(dialogue);
        if (currentDialogueBox == null)
        {
            currentDialogueBox = Instantiate(dialogueBoxPrefab, canvas.transform);
            currentDialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = bufferedDialogue[0];
            canvasSystem.enabled = false; //disable canvas interactions
            MouseInteractor.Instance.enabled = false; //disable other interactions
            inDialogue= true;
        }
    }

    private void ShowNextDialogue()
    {

        //if there's more dialogue than just this, show the next one, else return player control

        if (bufferedDialogue.Count > 1) //theres more than one dialogue remaining
        {
            Destroy(currentDialogueBox); //destroy previous
            currentDialogueBox = null;

            bufferedDialogue.RemoveAt(0);

            currentDialogueBox = Instantiate(dialogueBoxPrefab, canvas.transform);
            currentDialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = bufferedDialogue[0];
        }
        else if(bufferedDialogue.Count == 1)
        {
            Destroy(currentDialogueBox); //destroy previous
            currentDialogueBox = null;
            bufferedDialogue.RemoveAt(0);
            canvasSystem.enabled = true; //enable canvas interactions
            MouseInteractor.Instance.enabled = true; //enable other interactions
            inDialogue = false;
        }


        /*if (currentDialogueBox != null) { //destroys previous box
        }
        if(bufferedDialogue.Count> 0)
        {
            currentDialogueBox = Instantiate(dialogueBoxPrefab, canvas.transform);
            currentDialogueBox.GetComponentInChildren<TextMeshProUGUI>().text = bufferedDialogue[0];
            bufferedDialogue.RemoveAt(0);
            if (bufferedDialogue.Count == 0) //was last dialogue
            {
                Destroy(currentDialogueBox);
                currentDialogueBox = null;
                canvasSystem.enabled = true; //enable canvas interactions
                MouseInteractor.Instance.enabled = true; //enable other interactions
            }
        }*/

    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            ShowNextDialogue();
        }
        
    }

    //This script contains a list of dialogue that must be played. If the list size is larger than 0, it displays the dialogue on screen
    //by spawning a prefab.



}
