using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SyncPlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement PM;
    void Start()
    {
        NetworkObject thisObject = GetComponent<NetworkObject>();
        //Target targetScript = FindObjectOfType<Target>();
        if (thisObject.HasStateAuthority)
        {
            PM = FindAnyObjectByType<PlayerMovement>();
            if (PM != null)
            {
                PM.GO = this.gameObject;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
