using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCam : MonoBehaviour
{
    private GameObject mainCam;
    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(mainCam.transform);
    }
}
