using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMech : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform leftBarrel, rightBarrel;
    public float fireRate = 1.25f;
    public float missileSpeed = 5;
    public int playerCheckRadius = 20;
    public Transform torso;

    static Transform playerCam;

    float countdown = -3;
    bool swapBarrel;


    private void Start()
    {
        playerCam = Camera.main.transform;
    }

    private void FixedUpdate()
    {
        if (Physics.CheckSphere(transform.position, playerCheckRadius, LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide))
            FireMissileAtRate();
    }

    private void LateUpdate()
    {
        torso.LookAt(playerCam.position);
    }

    void FireMissileAtRate()
    {
        if (countdown > 1 / fireRate)
            FireMissile();
        else
            countdown += Time.deltaTime;
    }

    void FireMissile()
    {
        countdown = 0;
        var spawnPosition = Random.onUnitSphere * .1f + ((swapBarrel = !swapBarrel) ? leftBarrel.position : rightBarrel.position);
        var spawnedMissile = Instantiate(missilePrefab, spawnPosition, missilePrefab.transform.rotation, transform);
        spawnedMissile.GetComponent<Rigidbody>().AddForce((playerCam.transform.position + new Vector3(0, .5f) - spawnPosition).normalized * missileSpeed, ForceMode.VelocityChange);
        Destroy(spawnedMissile, 5);
    }
}
