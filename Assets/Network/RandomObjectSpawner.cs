using Fusion;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class RandomObjectSpawner : NetworkBehaviour
{
    [Networked] public int Rando { get; private set; }
    [Networked] public float CountdownTimer { get; private set; }
    [Networked] public float GameplayCountDown { get; private set; } // Gameplay timer

    [Networked] public bool isGameEnded { get; private set; } // Gameplay boolean

    public GameObject Collectable;
    public NetworkRunner runner;
    public Text timerText;
    public Text gameplayTimerText;
    public Animator panelAnimator;
    public Animator endAnimator;

    public int previousRando = -1;

    public Vector3 spawnAreaCenter = new Vector3(500f, 5.6f, 500f);
    public float spawnAreaRadius = 5f;
    public float minDistanceBetweenObjects = 1.5f;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    public GameObject waitingText;

    void Start()
    {
        runner = FindAnyObjectByType<NetworkRunner>();
        timerText = GameObject.Find("CountDown").GetComponent<Text>();
        gameplayTimerText = GameObject.Find("GameplayCountDown").GetComponent<Text>(); // Referensi ke teks GameplayCountDown
        panelAnimator = GameObject.Find("ScorePanel").GetComponent<Animator>();
        endAnimator = GameObject.Find("EndMenuPanel").GetComponent<Animator>();
        waitingText = GameObject.Find("waiting");

        if (Object.HasStateAuthority)
        {
            StartGameplayCountDown(30f); // Set durasi gameplay (misalnya 30 detik)
        }
    }

    void Update()
    {
        if (waitingText == null)
        {
            waitingText = GameObject.Find("waiting");
        }
        if (Object.HasStateAuthority)
        {
            if (CheckPlayerCount()) // Periksa apakah jumlah pemain cukup
            {
                // Kurangi waktu timer
                CountdownTimer -= Time.deltaTime;
                GameplayCountDown -= Time.deltaTime;
                waitingText.SetActive(false);

                if (GameplayCountDown <= 0)
                {
                    GameplayCountDown = 0;
                    RPC_EndGame();
                }

                if (CountdownTimer <= 0 && GameplayCountDown > 0)
                {
                    CountdownTimer = 0;
                    RPC_UpdateRandoValueOnServer();
                    StartCountdown(2.5f);
                }
            }
            else
            {
                //gameplayTimerText.text = "Waiting for Other Player.....";
                //waitingText.SetActive(true);
            }
        }

        if (isGameEnded)
        {
            Debug.Log("Game Over! Triggering animations on all clients.");
            panelAnimator.Play("panelInAnim");
            endAnimator.Play("EndInAnim");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_UpdateRandoValueOnServer()
    {
        if (Object.HasStateAuthority && GameplayCountDown > 0)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();

            if (spawnPosition != Vector3.zero)
            {
                runner.Spawn(Collectable, spawnPosition);
                spawnedPositions.Add(spawnPosition);
            }
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        const int maxAttempts = 10;
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * spawnAreaRadius;
            Vector3 randomPosition = new Vector3(
                spawnAreaCenter.x + randomCircle.x,
                spawnAreaCenter.y,
                spawnAreaCenter.z + randomCircle.y
            );

            if (IsPositionValid(randomPosition))
            {
                return randomPosition;
            }
        }

        Debug.LogWarning("Failed to find a valid spawn position after max attempts.");
        return Vector3.zero;
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (Vector3 spawnedPos in spawnedPositions)
        {
            if (Vector3.Distance(position, spawnedPos) < minDistanceBetweenObjects)
            {
                return false;
            }
        }
        return true;
    }

    public void StartCountdown(float duration)
    {
        if (Object.HasStateAuthority)
        {
            CountdownTimer = duration;
        }
    }

    public void StartGameplayCountDown(float duration)
    {
        if (Object.HasStateAuthority)
        {
            GameplayCountDown = duration;
        }
    }

    private void EndGame()
    {
        if (Object.HasStateAuthority)
        {
            isGameEnded = true; // Update state agar semua client tahu game telah berakhir
            Debug.Log("kelar gamenya harusnya");
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_EndGame()
    {
        if (Object.HasStateAuthority)
        {
            isGameEnded = true;
            Debug.Log("kelar gamenya harusnya");
        }
    }

    public override void Render()
    {
        UpdateTimerText(CountdownTimer);
        UpdateGameplayTimerText(GameplayCountDown);
    }

    private void UpdateTimerText(float timer)
    {
        if (timerText != null)
        {
            timerText.text = $"Spawn Timer: {timer:F1} s";
        }
    }

    private void UpdateGameplayTimerText(float timer)
    {
        if (gameplayTimerText != null)
        {
            gameplayTimerText.text = $"Gameplay Timer: {timer:F1} s";
        }
    }

    public void ResetSpawner()
    {
        spawnedPositions.Clear(); // Hapus semua posisi objek yang di-*spawn*
        CountdownTimer = 0;       // Reset timer
                                  // Tambahkan reset lain jika perlu
    }

    private bool CheckPlayerCount()
    {
        // Periksa jumlah pemain di room/session
        int playerCount = runner.ActivePlayers.Count(); // Mendapatkan jumlah pemain aktif

        if (playerCount >= 2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
