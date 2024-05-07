using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugerSpawn : MonoBehaviour
{
    public GameObject spawnTRM;
    private bool isSpawning = false;

    private void Update()
    {
        if (!isSpawning)
        {
            StartCoroutine(SpawnAndDeactivate(6f)); // 4초마다 스폰하고 비활성화하는 코루틴 실행
        }
    }

    IEnumerator SpawnAndDeactivate(float duration)
    {
        isSpawning = true;

        // 버거 스폰
        GameObject spawnedObject = PoolManager.Instance.SpawnFromPool("Buger");
        if (spawnedObject != null)
        {
            spawnedObject.transform.position = spawnTRM.transform.position; // 스폰 위치 설정
            spawnedObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration);

        // 4초 후에 버거 비활성화
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(false);
        }

        isSpawning = false;
    }
}
