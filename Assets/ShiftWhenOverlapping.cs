using UnityEngine;

public class ShiftWhenOverlapping : MonoBehaviour
{
    public float shiftDistance = 0.5f; // Jarak pergeseran ketika bertumpuk

    private void OnTriggerEnter(Collider other)
    {
        // Pastikan objek lain juga memiliki collider trigger
        if (other.isTrigger)
        {
            // Hitung arah pergeseran dengan vektor acak
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                0, // Pastikan tidak bergeser di sumbu Y
                Random.Range(-1f, 1f)
            ).normalized;

            // Geser objek saat ini ke arah acak
            transform.position += randomDirection * shiftDistance;

            Debug.Log($"Object {gameObject.name} shifted due to overlap with {other.gameObject.name}");
        }
    }
}
