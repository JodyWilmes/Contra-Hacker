using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MouseInteractor : MonoBehaviour
{
    private Camera maincamera;
    public class GameObjectEvent : UnityEvent<GameObject> { }
    public GameObjectEvent distributeClicked = new GameObjectEvent(); //this event is called when a clickable object is clicked
    public GameObjectEvent OnHoverChanged = new GameObjectEvent(); //this event is called when a clickable object is hovered on
    public UnityEvent OnHoverExit; //this event is called when a clickable object is hovered on
    public UnityEvent OnMissClick; //this event is called when a non clickable object is clicked
    private GameObject currentHover;
    private GameObject lastHover;
    // Start is called before the first frame update


    public static MouseInteractor Instance;
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

    void Start()
    {
        maincamera = Camera.main;
        SceneManager.activeSceneChanged += onSceneChange;
    }


    private void onSceneChange(Scene arg0, Scene arg1)
    {
        maincamera = FindFirstObjectByType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        detectClickableObject();
    }
    private bool checkForClick() {
        return Input.GetMouseButtonDown(0);
    }
    private void detectClickableObject() { 
        Ray ray = maincamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit)) {
            if (hit.transform.gameObject.tag == "ClickInteractible") { 
                currentHover = hit.transform.gameObject;
                if (currentHover != lastHover) {
                    OnHoverExit.Invoke();
                    OnHoverChanged?.Invoke(hit.transform.gameObject);
                    //Debug.Log($"{hit.transform.gameObject.name} changed hover");
                }
                lastHover= hit.transform.gameObject;
                //Debug.Log($"{hit.transform.gameObject.name} hovered on");
                if (checkForClick())
                {
                    //Debug.Log($"{hit.transform.gameObject.name} clicked on");
                    distributeClicked?.Invoke(hit.transform.gameObject);
                }
                return;
            }
            else
            {
                OnHoverExit.Invoke();

                lastHover = null;
                if (checkForClick())
                {
                    //Debug.Log($"{hit.transform.gameObject.name} clicked on");
                    OnMissClick?.Invoke();
                }
            }
        }
        Vector3 mouseposition = maincamera.ScreenToWorldPoint(Input.mousePosition);
        /*
        Vector2 mouseposition2d = new Vector2(mouseposition.x, mouseposition.y);
        RaycastHit2D hit2D = Physics2D.Raycast(mouseposition2d, Vector2.zero);
        if(hit2D.collider!= null)
        {
            if (hit2D.transform.gameObject.CompareTag("ClickInteractible"))
            {
                Debug.Log("2d click");
                distributeClicked?.Invoke(hit2D.transform.gameObject);
                return;
            }
        }
        */
    }
}
