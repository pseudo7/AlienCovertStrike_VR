using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAlien : MonoBehaviour
{
    public GameObject alienDieShotPrefab;
    public float shotSpeed = 35f;

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
        spawnedMissile.GetComponent<Rigidbody>().AddForce(((player.transform.position + Camera.main.transform.forward * 10) - transform.position).normalized * shotSpeed, ForceMode.VelocityChange);
        Destroy(spawnedMissile, 5);
    }
}
