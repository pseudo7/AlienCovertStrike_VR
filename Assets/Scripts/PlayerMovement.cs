using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float runningSpeed = 2f;
    public float runningMovement = .1f;
    public float runningFrequency = .5f;
    public float plasmaLifeTime = 3;

    public int magazineCapacity = 5;
    public float fireRate = 2;

    float countdown;

    public Transform[] paths;
    public Transform[] lanes;

    public Text accText;

    public GvrReticlePointer gvrReticle;

    public GameObject plasmaBlastPrefab;
    public Transform gunBarrelTransform;
    public Transform magazineTransform;
    public Transform enemyParent;

    int pos = 1;
    static int index = 0;
    bool allowStrife = true;
    public float bulletSpeed = 50;

    GameObject plasmaBullet;
    Quaternion plasmaRotation;

    Transform mainCamTransform;

    Stack<GameObject> magazineStack;

    private void Start()
    {
        StartCoroutine(RunningMovement());
        StartCoroutine(IncreaseSteps());
        plasmaRotation = Quaternion.Euler(0, 180, 0);
        mainCamTransform = Camera.main.transform;
        magazineStack = new Stack<GameObject>(magazineCapacity);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        MovePlayerForward();

        CheckStrife();

        ResetAcc();
        FireAtRate();
        accText.text = string.Format("Enemies\nLeft:{0}", enemyParent.childCount.ToString("0#"));
    }

    float GetRunningValue()
    {
        var rotY = mainCamTransform.rotation.eulerAngles.y;
        var val = rotY > 180 ? 360 - rotY : rotY;
        return ((val / 180) - .5f) * -2;
    }

    void FireAtRate()
    {
        if (countdown > 1 / fireRate)
            Fire();
        else countdown += Time.deltaTime;
    }

    float CloserValue(float a, float b, float val)
    {
        //CloserValue(0, 90, Mathf.Abs(mainCamTransform.rotation.eulerAngles.y % 180));
        return Mathf.Abs(val - a) < Mathf.Abs(val - b) ? a : b;
    }

    void CheckStrife()
    {
        if (Input.acceleration.x < -.4f)
            MovePlayerLeft();
        else if (Input.acceleration.x > .4f)
            MovePlayerRight();
    }

    void Fire()
    {
        countdown = 0;
        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamTransform.position, mainCamTransform.forward, out hitInfo, 125f, LayerMask.GetMask("Player", "Enemy"), QueryTriggerInteraction.Collide))
        {
            Debug.Log(hitInfo.collider.tag + " " + hitInfo.collider.name);
            if (hitInfo.collider.CompareTag("Enemy") || hitInfo.collider.CompareTag("GroundMech"))
                if (magazineStack.Count != magazineCapacity)
                {
                    plasmaBullet = Instantiate(plasmaBlastPrefab, gunBarrelTransform.position, plasmaRotation);
                    plasmaBullet.GetComponent<Rigidbody>().AddForce((hitInfo.point - gunBarrelTransform.position).normalized * bulletSpeed, ForceMode.VelocityChange);
                    magazineStack.Push(plasmaBullet);
                    UpdateMagazine(magazineCapacity - magazineStack.Count);
                    StartCoroutine(ClearFromMagazine(plasmaLifeTime, plasmaBullet));
                }
        }
    }

    IEnumerator ClearFromMagazine(float delay, GameObject go)
    {
        yield return new WaitForSeconds(delay);
        magazineStack.Pop();
        Destroy(go);
        UpdateMagazine(magazineCapacity - magazineStack.Count);
    }

    void MovePlayerLeft()
    {
        if (!allowStrife)
            return;
        if (pos > 0)
        {
            pos--;
            allowStrife = false;
        }
    }

    void UpdateMagazine(int bullets)
    {
        for (int i = 0; i < bullets; i++)
            magazineTransform.GetChild(i).gameObject.SetActive(true);
        for (int i = bullets; i < magazineCapacity; i++)
            magazineTransform.GetChild(i).gameObject.SetActive(false);
    }

    void MovePlayerRight()
    {
        if (!allowStrife)
            return;
        if (pos < 2)
        {
            pos++;
            allowStrife = false;
        }
    }

    void ResetAcc()
    {
        if (Input.acceleration.x < .4f && Input.acceleration.x > -.4f) allowStrife = true;
    }

    void MovePlayerForward()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + runningSpeed * Time.deltaTime * GetRunningValue());
    }

    IEnumerator IncreaseSteps()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(runningFrequency);
            index++;
        }
    }

    IEnumerator RunningMovement()
    {
        int length = paths.Length;
        while (gameObject.activeInHierarchy)
        {
            var nextPos = paths[index % length].position;
            transform.position = Vector3.Lerp(transform.position, new Vector3(nextPos.x + lanes[pos].position.x, nextPos.y, transform.position.z), .1f);
            yield return new WaitForEndOfFrame();
        }
    }
}
