using UnityEngine;

public class HomeZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (GameManager.Instance != null && GameManager.Instance.AllCollected)
        {
            GameManager.Instance.Win();
        }
    }
}