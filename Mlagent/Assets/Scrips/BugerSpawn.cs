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
            StartCoroutine(SpawnAndDeactivate(6f)); // 4�ʸ��� �����ϰ� ��Ȱ��ȭ�ϴ� �ڷ�ƾ ����
        }
    }

    IEnumerator SpawnAndDeactivate(float duration)
    {
        isSpawning = true;

        // ���� ����
        GameObject spawnedObject = PoolManager.Instance.SpawnFromPool("Buger");
        if (spawnedObject != null)
        {
            spawnedObject.transform.position = spawnTRM.transform.position; // ���� ��ġ ����
            spawnedObject.SetActive(true);
        }

        yield return new WaitForSeconds(duration);

        // 4�� �Ŀ� ���� ��Ȱ��ȭ
        if (spawnedObject != null)
        {
            spawnedObject.SetActive(false);
        }

        isSpawning = false;
    }
}
