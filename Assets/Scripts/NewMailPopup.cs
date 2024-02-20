using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMailPopup : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject indicator;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mailbox.Instance.getMailbox().Count > 0)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
    }
}
