using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAlien : MonoBehaviour
{
    public GameObject alienDieShotPrefab;

    static GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter(Collision collision)
    {
        FireDieShot();
    }

    void FireDieShot()
    {
        var spawnedMissile = Instantiate(alienDieShotPrefab, transform.position, Quaternion.LookRotation(player.transform.position - transform.position));
        spawnedMissile.GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position).normalized * 25, ForceMode.VelocityChange);
        Destroy(spawnedMissile, 5);
    }
}
