using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ghostMode : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private MeshRenderer m_Renderer;


    // Update is called once per frame
    void Update()
    {
        m_Renderer.enabled = !m_Renderer.enabled;
    }
}
