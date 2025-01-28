using UnityEngine;
using Fusion;
using TMPro;
using UnityEngine.UI;

public class SyncIntegerFusion2 : NetworkBehaviour
{
    // Properti yang disinkronkan
    [Networked] public int SyncedValue { get; private set; }

    // Referensi ke TextMeshPro untuk menampilkan nilai
    public Text textMesh;
    

    public PlayerMovement PM;


    private void Start()
    {
        NetworkObject thisObject = GetComponent<NetworkObject>();
        //Target targetScript = FindObjectOfType<Target>();
        if (thisObject.HasStateAuthority)
        {
            PM = FindAnyObjectByType<PlayerMovement>();
            if (PM != null)
            {
                PM.playerScript = this;
            }

            
        }
    }

    private void Update()
    {
        if (Object.HasInputAuthority)
        {
            // Minta State Authority untuk memperbarui nilai
            //RPC_UpdateValueOnServer(SyncedValue + 1);
        }

        // Pastikan teks selalu menghadap kamera
        if (textMesh != null && Camera.main != null)
        {
            //textMesh.transform.LookAt(Camera.main.transform);
            //textMesh.transform.Rotate(0, 180, 0); // Membalik teks agar tidak terbalik
        }

        

    }

    /*public override void FixedUpdateNetwork()
    {
        // Perbarui tampilan nilai pada setiap klien
        UpdateText(SyncedValue);
    }*/

    public override void Render()
    {
        // Perbarui tampilan teks pada semua klien
        UpdateText(SyncedValue);

    }

    // RPC untuk memperbarui nilai pada State Authority
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_UpdateValueOnServer(int newValue)
    {
        // Hanya State Authority yang dapat memperbarui nilai sinkronisasi
        if (Object.HasStateAuthority)
        {
            SyncedValue = newValue;
        }
    }

    // Perbarui teks di atas pemain
    private void UpdateText(int value)
    {
        if (textMesh != null)
        {
            textMesh.text = $"Value: {value}";
        }
    }


   
}
