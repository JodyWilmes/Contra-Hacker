using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OfficeCameraFocus : MonoBehaviour
{
    //this script can focus on a clickable gameObject and change its outlines
    [SerializeField] private MouseInteractor mouseInteractor;
    [SerializeField] private Outline currentOutlineScript;
    [SerializeField] private GameObject currentFocus;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private Transform originalPosition;
    [SerializeField] private Transform originalLookAt;
    [SerializeField] private GameObject backArrow;
    [SerializeField] private GameObject endDayButton;
    // Start is called before the first frame update
    void Start()
    {
        mouseInteractor = MouseInteractor.Instance;
        mouseInteractor.OnHoverChanged.AddListener(OnHoverChanged);
        mouseInteractor.OnHoverExit.AddListener(OnHoverOff);
        mouseInteractor.distributeClicked.AddListener(OnObjectFocus);
        mouseInteractor.OnMissClick.AddListener(OnObjectDefocus);
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            OnObjectDefocus();
        }
    }
    // Update is called once per frame
    public void OnHoverOff() {
        //Debug.Log("hover off runs");
        if (currentOutlineScript != null)
        {
            currentOutlineScript.enabled = false;
            //Debug.Log("outline disabled");
        }
    }
    private void OnHoverChanged(GameObject target)
    {
        //Debug.Log("hover changed runs");
        if(currentFocus == null)
        {
            currentOutlineScript = target.GetComponent<Outline>();
            if(currentOutlineScript != null )
            {
                currentOutlineScript.enabled= true;
            }
        }
    }
    private void OnObjectFocus(GameObject focus)
    {   
        if (currentOutlineScript != null)
        {
            //subscribed to the onclick
            //this should move the virtual camera
            //gets a "CinemachineCameraPosition" named gameobject in the target's children
            currentFocus = focus;
            Transform cameraposition = focus.transform.Find("CinemachineCameraPosition");
            Transform cameralookat = focus.transform.Find("CinemachineCameraLookAt");
            Debug.Log(cameraposition);
            cinemachineCamera.LookAt = cameralookat;
            cinemachineCamera.Follow= cameraposition;
            //currentOutlineScript.enabled = true;
            currentOutlineScript.enabled = false;
            backArrow.SetActive(true);
            endDayButton.SetActive(false);
        }
    }

    private void OnObjectDefocus()
    {
        currentFocus = null;
        cinemachineCamera.LookAt = originalLookAt;
        cinemachineCamera.Follow = originalPosition;
        backArrow.SetActive(false);
        endDayButton.SetActive(true);
    }
}
