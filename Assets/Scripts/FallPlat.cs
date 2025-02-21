using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FallPlat : MonoBehaviour
{
    public float fallTime = 0.5f;
    private Vector3 startPosition;

    // Static list to track all platforms
    private static List<FallPlat> allPlatforms = new List<FallPlat>();

    void Start()
    {
        startPosition = transform.position;
        allPlatforms.Add(this);
    }

    void OnDestroy()
    {
        allPlatforms.Remove(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall(fallTime));
        }
    }

    IEnumerator Fall(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }

    public static void ResetAllPlatforms()
    {
        foreach (FallPlat platform in allPlatforms)
        {
            platform.gameObject.SetActive(true);
            platform.transform.position = platform.startPosition;
        }
    }
}
