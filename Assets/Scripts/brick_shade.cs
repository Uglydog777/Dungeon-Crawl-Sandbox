using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class brick_shade : MonoBehaviour
{

    Material b_Material;

    // Start is called before the first frame update
    void Start()
    {
        // randomly set a gray shade for the individual bricks in the wall

        int rgbColor = Random.Range(50, 100);
        GetComponent<Renderer>().material.color = new Color32((byte)rgbColor, (byte)rgbColor, (byte)rgbColor, 255);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
