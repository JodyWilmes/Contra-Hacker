using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFollow : MonoBehaviour
    //this script can be added to a gameObject to follow it without another without being a child (i.e. ignoring rotation)
{
    [SerializeField] private GameObject followTarget;
    [SerializeField] private Vector3 transformOffset;
    // Start is called before the first frame update

    private void Start()
    {
        transformOffset = followTarget.transform.position - transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = followTarget.transform.position - transformOffset;
    }
}
