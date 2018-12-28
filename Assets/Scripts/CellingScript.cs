using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellingScript : MonoBehaviour
{
    [SerializeField] bool showCelling = true;
    // Start is called before the first frame update
    void Start()
    {
        // for editing so you can hide the celling at runtime to see whats going on in the editor
        if (showCelling == true)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
