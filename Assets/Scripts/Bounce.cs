using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [Header("Bounce Settings")]
    [SerializeField] private float force = 10000f;
    [SerializeField] private float stunTime = 0.5f;
    private Vector3 hitDir;

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
            if (collision.gameObject.CompareTag("Player"))
            {
                hitDir = contact.normal;
                PlayerMovement playerMovement = collision.gameObject.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    playerMovement.HitPlayer(-hitDir * force, stunTime);
                }
                return;
            }
        }
    }
}
