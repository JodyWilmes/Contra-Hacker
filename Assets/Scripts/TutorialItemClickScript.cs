using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialItemClickScript : MonoBehaviour
{
    [SerializeField] private List<string> tutorialDialogue = new List<string>();
    // Start is called before the first frame update
    //this script adds dialogue when an object is clicked, which can be set in a list
    void Start()
    {
        MouseInteractor.Instance.distributeClicked.AddListener(HandleOneShotDialogue);
    }


    void HandleOneShotDialogue(GameObject target) //subscribed to onclick
    {
        if(target == gameObject && enabled)
        {
            foreach(var dialogue in tutorialDialogue) { 
                DialogueManager.Instance.AddDialogue(dialogue);
            }
            enabled= false;
        }

    }
}
