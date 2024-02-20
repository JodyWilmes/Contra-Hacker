using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*this is a script purely for testing and code in here will most likely be added and deleted over time.*/
public class TestScript : MonoBehaviour
{
    private MouseInteractor mouseInteractor;
    // Start is called before the first frame update
    void Start()
    {
        mouseInteractor= GameObject.FindGameObjectWithTag("InteractionDetectionScript").GetComponent<MouseInteractor>();
        mouseInteractor?.distributeClicked.AddListener(TestFunction);
        
    }
    private void TestFunction(GameObject _object)
    {
        Debug.Log(this.gameObject, _object);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
