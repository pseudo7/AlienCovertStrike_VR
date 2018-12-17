using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMechHit : MonoBehaviour
{
    public int bulletHits = 3;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("HIT");

        if (collision.collider.CompareTag("Bullet"))
        {
            Debug.Log("HIT BULLET");
            if (--bulletHits <= 0)
                Destroy(gameObject);
        }
    }
}
