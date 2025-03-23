using UnityEngine;

public class KillVolume : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit kill volume");
            GameManager.Instance.PlayerDied();
        }
    }
}
