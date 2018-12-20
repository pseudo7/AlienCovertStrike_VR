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
        Debug.LogWarning("HitGourndMech Called");
        bulletHits--;
        UpdateHealthBar(bulletHits / initHits);

        if (bulletHits <= 0)
            Destroy(transform.parent.gameObject);
    }

    void UpdateHealthBar(float health)
    {
        transform.GetChild(0).localScale = new Vector3(health, 1f);
    }
}
