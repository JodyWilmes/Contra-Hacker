using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class contentsizefitterfix : MonoBehaviour
{
    // Start is called before the first frame update
    public void FixContent()
    {
        ContentSizeFitter contentSizeFitter = GetComponent<ContentSizeFitter>();
        contentSizeFitter.enabled= false;
        contentSizeFitter.enabled= true;
    }
    void Update()
    {
        FixContent();
    }
}
