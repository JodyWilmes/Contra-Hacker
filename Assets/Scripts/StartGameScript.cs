using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class StartGameScript : MonoBehaviour
{
    [SerializeField] private Volume globalVolume;
    [SerializeField] private MouseInteractor mouseInteractor;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private List<GameObject> disabledGameObjects; //these will be disabled at the start and enabled when starting
    // Start is called before the first frame update
    void Start()
    {
        mouseInteractor.enabled = false;
        VolumeProfile profile = globalVolume.sharedProfile;
        if (!profile.TryGet<DepthOfField>(out var dof))
        {
            dof = profile.Add<DepthOfField>(false);
        }
        dof.active = true;
        foreach (GameObject gObject in disabledGameObjects)
        {
            gObject.SetActive(false);
        }
    }

    // Update is called once per frame
    public void StartGame()
    {
        VolumeProfile profile = globalVolume.sharedProfile;
        if (!profile.TryGet<DepthOfField>(out var dof))
        {
            dof = profile.Add<DepthOfField>(false);
        }
        dof.active = false;
        mouseInteractor.enabled = true;
        DayManager.Instance.StartDay(0);
        titleScreen.SetActive(false);
        foreach (GameObject gObject in disabledGameObjects)
        {
            gObject.SetActive(true);
        }
        DialogueManager.Instance.AddDialogue("Herman: Hallo, nieuwe medewerker! Welkom bij je nieuwe baan in de administratie!");
        DialogueManager.Instance.AddDialogue("Bekijk je telefoon maar even voor de info over je nieuwe e-mail, en de contacten van je werk.");
    }
}
