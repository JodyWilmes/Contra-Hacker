using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNotifier : MonoBehaviour
{
    // This script creates a notification prefab at the specified area when the function is called
    [SerializeField] private GameObject messagePrefab;
    [SerializeField] private RectTransform targetLocation; 
    void Start()
    {
        
    }
    public void CreateNotification()
    {
        GameObject prefab = Instantiate(messagePrefab,targetLocation);
    }
    // Update is called once per frame
}
