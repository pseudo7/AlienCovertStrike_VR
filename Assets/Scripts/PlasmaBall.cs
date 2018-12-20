using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
            Destroy(collision.collider.gameObject);
        if (collision.collider.CompareTag("GroundMech"))
            collision.collider.GetComponent<GroundMechHit>().HitGroundMech();
        Destroy(gameObject);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("GroundMech"))
    //        other.GetComponent<GroundMechHit>().HitGourndMech();
    //}
}
