using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    // Referensi ke UI Text untuk menampilkan leaderboard
    public Text leaderboardText;

    private void Update()
    {
        UpdateLeaderboard();
    }

    private void UpdateLeaderboard()
    {
        // Dapatkan semua objek pemain
        var players = FindObjectsOfType<SyncIntegerFusion2>();

        // Bangun string leaderboard
        string leaderboardContent = "Leaderboard:\n";
        foreach (var player in players)
        {
            leaderboardContent += $"Player {player.Object.InputAuthority.PlayerId}: {player.SyncedValue}\n";
        }

        // Perbarui UI teks leaderboard
        if (leaderboardText != null)
        {
            leaderboardText.text = leaderboardContent;
        }
    }
}
