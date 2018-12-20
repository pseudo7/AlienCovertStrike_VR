using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    public GameObject blowUpEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            Destroy(collision.collider.gameObject);
        if (collision.collider.CompareTag("GroundMech"))
            collision.collider.GetComponent<GroundMechHit>().HitGroundMech();
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Destroy(Instantiate(blowUpEffect, transform.position, Quaternion.identity), 1f);
    }
}
