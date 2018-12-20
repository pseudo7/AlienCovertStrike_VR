using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMechHit : MonoBehaviour
{
    public int bulletHits = 3;

    float initHits;

    private void Start()
    {
        initHits = bulletHits;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Boundary"))
            transform.parent.Rotate(0, 180, 0);
    }

    public void HitGroundMech()
    {
        bulletHits--;
        UpdateHealthBar(bulletHits / initHits);

        if (bulletHits <= 0)
        {
            transform.GetComponentInParent<GroundMech>().enabled = false;
            transform.GetComponentInParent<Collider>().enabled = false;
            transform.GetComponentInParent<Animator>().enabled = false;
            transform.GetChild(1).gameObject.SetActive(true);
            Destroy(transform.parent.gameObject, 3f);
        }
    }

    void UpdateHealthBar(float health)
    {
        transform.GetChild(0).localScale = new Vector3(health, 1f);
    }
}
