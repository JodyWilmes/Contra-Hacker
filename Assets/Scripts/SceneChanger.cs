using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    //this script will change to the appropriate scene when the object this is attached to is clicked.
    // Start is called before the first frame update
    [SerializeField] private string sceneName;
    private Scene targetScene;
    private MouseInteractor mouseInteractor;
    void Start()
    {
        //subscribe to the mouse interactor
        mouseInteractor = GameObject.FindGameObjectWithTag("InteractionDetectionScript").GetComponent<MouseInteractor>();
        mouseInteractor?.distributeClicked.AddListener(switchScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void switchScene(GameObject clickedObject)
    {
        if (clickedObject == this.gameObject)
        {
            Debug.Log($"Switching scene to {sceneName}");
            targetScene = SceneManager.GetSceneByName(sceneName);
            SceneManager.LoadScene(sceneName);
        }
        return;
    }
}
