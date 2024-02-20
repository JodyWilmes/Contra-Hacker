using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadActivator : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject dontdestroy = this.gameObject;
        Debug.Log(dontdestroy);
        DontDestroyOnLoad(dontdestroy);
        Debug.Log("i won't be silenced");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
