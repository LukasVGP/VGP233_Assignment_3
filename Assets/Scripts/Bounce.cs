using UnityEngine;

public class Bounce : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float force = 10000f;
    [SerializeField] private float stunTime = 0.5f;
    private Vector3 hitDir;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ContactPoint contact = collision.GetContact(0);
            hitDir = contact.normal;

            PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.HitPlayer(-hitDir * force, stunTime);
                Debug.DrawRay(contact.point, contact.normal * 2, Color.red, 2f);
            }
        }
    }
}
