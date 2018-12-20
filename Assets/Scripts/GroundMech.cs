﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundMech : MonoBehaviour
{
    public GameObject missilePrefab;
    public Transform leftBarrel, rightBarrel;
    public float fireRate = 1.25f;
    public float missileSpeed = 5;
    public int playerCheckRadius = 20;
    static GameObject player;

    float countdown = -3;
    bool swapBarrel;
    float rotY;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rotY = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        if (Physics.CheckSphere(transform.position, playerCheckRadius, ~LayerMask.GetMask("Player"), QueryTriggerInteraction.Collide))
        {
            if ((int)rotY == 0)
            {
                if (player.transform.position.z - transform.position.z < 0)
                    return;
            }
            else
            {
                if (player.transform.position.z - transform.position.z > 0)
                    return;
            }
            FireMissileAtRate();
        }
        //foreach (var item in ))
        //{
        //}
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if ((int)rotY == 0)
    //    {
    //        if (player.transform.position.z - transform.position.z < 0)
    //            return;
    //    }
    //    else
    //    {
    //        if (player.transform.position.z - transform.position.z > 0)
    //            return;
    //    }

    //    if (other.CompareTag("MainCamera"))
    //        FireMissileAtRate();
    //}

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
        var spawnedMissile = Instantiate(missilePrefab, spawnPosition, Quaternion.identity, transform);
        spawnedMissile.GetComponent<Rigidbody>().AddForce((player.transform.position - spawnPosition).normalized * missileSpeed, ForceMode.VelocityChange);
        Destroy(spawnedMissile, 5);
    }
}
