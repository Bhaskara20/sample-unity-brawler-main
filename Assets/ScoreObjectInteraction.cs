using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreObjectInteraction : NetworkBehaviour
{
    [Tooltip("Jumlah skor yang diberikan saat berinteraksi.")]
    [SerializeField] private int scoreValue = 10;

    private SyncIntegerFusion2 playerScript;
    public RandomObjectSpawner RO;

    public NetworkRunner runner;
    public NetworkObject networkObject;

    // Start is called before the first frame update
    void Start()
    {
        networkObject = GetComponent<NetworkObject>();
        runner = FindAnyObjectByType<NetworkRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        if (RO == null) {
            RO = FindAnyObjectByType<RandomObjectSpawner>();
        }
    }

    // Mengatur objek untuk dinonaktifkan secara sinkron
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_DisableObject()
    {
        //gameObject.SetActive(false);
        runner.Despawn(networkObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            
            playerScript = other.GetComponent<SyncIntegerFusion2>();
            if (playerScript != null && playerScript.Object.HasInputAuthority)
            {
                RO.RPC_UpdateRandoValueOnServer();
                playerScript.RPC_UpdateValueOnServer(playerScript.SyncedValue + scoreValue);
                Debug.Log("Score updated on hover");
                RPC_DisableObject();
                runner.Despawn(networkObject); //harus aktif lagi setelah debugging
            }
        }
    }
}
