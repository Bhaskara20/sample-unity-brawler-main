using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using TMPro;
using baskara.FusionBites;

public class PlayerStats : NetworkBehaviour
{
    //[Networked(OnChanged = nameof(UpdatePlayerName))] public NetworkString<_32> playerName { get; set; }
    [Networked]
    public NetworkString<_32> playerName { get; set; }

    [SerializeField] TextMeshProUGUI playerNameLabel;
    // ChangeDetector untuk mendeteksi perubahan playerName
    private ChangeDetector _changeDetector;


    /*private IEnumerator Start()
    {
        if (this.HasStateAuthority) { 
            playerName = FusionConnection.Instance._playerName;
        }
        yield return new WaitUntil(() => this.isActiveAndEnabled);

        yield return new WaitUntil(() => playerName.ToString() != null);

        playerNameLabel.text = playerName.ToString();
    }

    public override void Spawned()
    {
        base.Spawned(); 
    }*/

    //Deprecated
    /*
    private void Start()
    {
        if (this.HasStateAuthority)
        {
            playerName = FusionConnection.Instance._playerName;
        }
    }

    protected static void UpdatePlayerName(Changed<PlayerStats> changed)
    {
        changed.Behaviour.playerNameLabel.text = changed.Behaviour.playerName.ToString();
    }*/


    //cara fusion 2
    // Inisialisasi ChangeDetector di Spawned
    public override void Spawned()
    {
        _changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);

        if (this.HasStateAuthority)
        {
            playerName = FusionConnection.Instance._playerName;
        }
    }

    // Gunakan Render untuk mengecek perubahan dengan ChangeDetector
    public override void Render()
    {
        foreach (var change in _changeDetector.DetectChanges(this))
        {
            switch (change)
            {
                case nameof(playerName):
                    playerNameLabel.text = playerName.ToString();
                    break;
            }
        }
    }

    private void Update()
    {
        // Pastikan nama label selalu sinkron dengan playerName
        if (playerNameLabel.text != playerName.ToString())
        {
            playerNameLabel.text = playerName.ToString();
        }
    }
}
