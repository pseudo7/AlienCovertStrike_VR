using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlasmaBall : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Destroy(collision.collider.gameObject);
        }
        if (collision.collider.CompareTag("GroundMech"))
        {
            Destroy(collision.collider.transform.parent.gameObject);
        }
    }
}
