using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cryptolocker : MonoBehaviour
{
    [SerializeField] private GameObject lockScreen;
    [SerializeField] private int LockTime;
    private int timer;
    // Start is called before the first frame update

    public void OnHacked()
    {
        lockScreen.SetActive(true);
        StartCoroutine(KeepLocked());
    }
    IEnumerator KeepLocked() {
        timer = 0;
        while (timer < LockTime)
        {
            yield return new WaitForSeconds(1);
            timer++;
        }
        lockScreen.SetActive(false);
    }
    // Update is called once per frame
}
