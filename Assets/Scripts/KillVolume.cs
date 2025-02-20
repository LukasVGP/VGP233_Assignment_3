using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.ShowGameOver();
        }
    }
}
