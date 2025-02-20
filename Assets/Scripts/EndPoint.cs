using UnityEngine;

public class EndPoint : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.ShowWinScreen();
        }
    }
}
