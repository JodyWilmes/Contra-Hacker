using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TrashcanConfetti : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private ParticleSystem confettiSpawner;
    void Start()
    {
        MouseInteractor.Instance.distributeClicked.AddListener(CreateConfetti);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateConfetti(GameObject target)
    {
        if(target == gameObject)
        {
            Debug.Log("Easter Egg!");
            confettiSpawner.Emit(10);
        }
    }
}
