using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerNotification : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TargetTextMesh;
    [SerializeField] private GameObject messageParent;
    [SerializeField] private int fadeTime;
    [SerializeField] private int activeTime;
    [SerializeField] private int speed;
    [SerializeField] private int fadeSteps;
    [SerializeField] public string messageText;
    private float alpha = 1f;

    IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(activeTime);
        while (TargetTextMesh.color.a > 0)
        {
            yield return new WaitForSeconds(fadeTime/fadeSteps);
            alpha -= (1/(float)fadeSteps);
            TargetTextMesh.alpha = alpha;
            messageParent.transform.Translate(new Vector3(0,speed));
        }
        Destroy(messageParent);
    }
    // Start is called before the first frame update
    void Awake()
    {
        TargetTextMesh.text= messageText;
        StartCoroutine(WaitForFade());
    }

    // Update is called once per frame

}
