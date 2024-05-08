using Google.Protobuf.WellKnownTypes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AnimalArea : MonoBehaviour
{
    public AnimalAgent penguinAgent;
    //public GameObject penguinBaby;
    public List<GameObject> _animal;
    public GameObject animalPrefab;
    //public Fish fishPrefab;
    public TextMeshProUGUI cumulativeRewardText;

    public List<Transform> spawnTRM;
    private List<GameObject> animalFullList;

    public int remainingFish
    {
        get { return animalFullList.Count; }
    }

    public static Vector3 ChooseRandomPosition(List<Transform> center, float radius)
    {
        Transform randomDirection = center[Random.Range(0,center.Count)];

        Vector3 randomPosition = randomDirection.position + Random.insideUnitSphere * radius;

        return randomPosition;
    }

    /*    private void PlacePenguin()
        {
            Rigidbody rigidbody = penguinAgent.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            penguinAgent.transform.position = ChooseRandomPosition(transform.parent.position, 0f, 360f, 0f, 9f) + Vector3.up * 0.5f;
            penguinAgent.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
        }*/

    /*    private void PlaceBaby()
        {
            Rigidbody rigidbody = penguinBaby.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;

            penguinBaby.transform.position = ChooseRandomPosition(transform.parent.position, -45f, 45f, 4f, 9f) + Vector3.up * 0.5f;
            penguinBaby.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }*/

    private void SpawnFish(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject newAnimal = Instantiate(animalPrefab);
            _animal.Add(newAnimal);
            newAnimal.transform.position = ChooseRandomPosition(spawnTRM, 0f);
            newAnimal.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
            
            newAnimal.transform.SetParent(transform);

            //animalFullList.Add(newAnimal);
        }
    }

    private void RemoveAllFish()
    {
        if (animalFullList != null)
        {
            foreach (GameObject obj in animalFullList)
            {
                Destroy(obj);
            }
        }
        animalFullList = new List<GameObject>();
    }

    public void RemoveFishInList(GameObject fishObject)
    {
        animalFullList.Remove(fishObject);
        Destroy(fishObject);
    }

    public void ResetArea()
    {
        RemoveAllFish();
        //PlaceBaby();
        SpawnFish(8);
    }

    private void FixedUpdate()
    {
        cumulativeRewardText.text = penguinAgent.GetCumulativeReward().ToString("0.00");
    }
}
