using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public GameObject[] buildings;
    public GameObject[] wallEnemies;
    public GameObject[] groundEnemies;

    public int numberOfBuildingPresets = 5;
    public Transform enemyParent, presetParent;

    List<BuildingPreset> buildingPresetList;

    bool swapSpace;

    void Awake()
    {
        buildingPresetList = new List<BuildingPreset>();
        SpawnBuilding();
    }

#if UNITY_EDITOR

    [UnityEditor.MenuItem("Pseudo/Capture")]
    public static void Capture()
    {
        ScreenCapture.CaptureScreenshot(System.DateTime.Now.Ticks.ToString());
    }
#endif

    void SpawnBuilding()
    {
        for (int i = -numberOfBuildingPresets / 2; i < numberOfBuildingPresets / 2; i++)
            buildingPresetList.Add(Instantiate(buildings[Random.Range(0, buildings.Length)], new Vector3(0, -2, i * 100), Quaternion.identity, presetParent).GetComponent<BuildingPreset>());
        SpawnEnemies();
        StartCoroutine(ChangeExposerure());
    }

    void SpawnEnemies()
    {
        foreach (var preset in buildingPresetList)
        {
            foreach (var building in preset.buildings)
            {
                var spawn = wallEnemies[Random.Range(0, wallEnemies.Length)];
                var spawnedOnWall = Instantiate(spawn, GetSpawnPos(building.BuildingInfo), Quaternion.Euler(-90, 0, 90), enemyParent);
                spawnedOnWall.transform.localScale = spawnedOnWall.transform.position.x < 0 ? new Vector3(1, -1, 1) : Vector3.one;
                //if (spawned.transform.position.x < 0) spawn.transform.SetPositionAndRotation(spawn.transform.position, Quaternion.Euler(spawn.transform.rotation.eulerAngles + Vector3.up * 180));
            }
            //for (int i = 0; i < 5; i++)
            var spawnedOnGround = Instantiate(groundEnemies[Random.Range(0, groundEnemies.Length)], new Vector3(Random.Range(-1, 1) * 4, -2, Random.Range(preset.transform.position.z, preset.transform.position.z + 50)), Quaternion.identity);
            spawnedOnGround.transform.rotation = Quaternion.Euler(0, spawnedOnGround.transform.position.z > 0 ? 180 : 0, 0);
        }
    }

    IEnumerator ChangeExposerure()
    {
        while (gameObject.activeInHierarchy)
        {
            yield return new WaitForSeconds(Random.Range(2f, 4f));
            FlashTheSky();
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            LeanTween.value(gameObject, UpdateSkyboxColor, swapSpace ? Color.red : Color.white, !swapSpace ? Color.red : Color.white, Random.Range(1f, 3f));
            swapSpace = !swapSpace;
        }
    }

    private void OnApplicationQuit()
    {
        ResetSkybox();
    }

    void ResetSkybox()
    {
        RenderSettings.skybox.SetFloat("_Exposure", 1);
        RenderSettings.skybox.SetColor("_Tint", new Color(.5f, .5f, .5f));
    }

    void FlashTheSky()
    {
        var delay = Random.Range(.25f, 1.5f);
        LeanTween.value(gameObject, UpdateSkyboxValue, swapSpace ? 2.5f : 1, !swapSpace ? 2.5f : 1, delay);
        LeanTween.value(gameObject, UpdateSkyboxValue, swapSpace ? 2.5f : 1, !swapSpace ? 2.5f : 1, delay);
    }

    void UpdateSkyboxValue(float val)
    {
        RenderSettings.skybox.SetFloat("_Exposure", val);
    }

    void UpdateSkyboxColor(Color val)
    {
        RenderSettings.skybox.SetColor("_Tint", val);
    }

    Vector3 GetSpawnPos(BuildingInfo info)
    {
        return new Vector3(Random.Range(info.ll.x, info.rr.x), Random.Range(info.ll.y, info.rr.y), Random.Range(info.ll.z, info.rr.z));
    }
}
