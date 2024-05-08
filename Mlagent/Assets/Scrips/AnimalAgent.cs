using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.AI;

public class AnimalAgent : Agent
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    public GameObject heartPrefab;
    public GameObject regurgitatedFishPrefab;

    private AnimalArea animalArea;
    private new Rigidbody rigidbody;
    //private GameObject babyPenguin;

    private bool isFull;

    public override void Initialize()
    {
        animalArea = transform.parent.Find("AnmalArea").GetComponent<AnimalArea>();
        //babyPenguin = animalArea.penguinBaby;
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        isFull = false;
        animalArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관측값의 개수는 8개이다.
        sensor.AddObservation(isFull);
        //sensor.AddObservation(Vector3.Distance(transform.position, babyPenguin.transform.position));
        //sensor.AddObservation((babyPenguin.transform.position - transform.position).normalized);
        sensor.AddObservation(transform.forward);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var DiscreteActions = actions.DiscreteActions;

        // 앞으로 가지 않을지(0), 앞으로 갈지(1)
        int forwardAmount = DiscreteActions[0];

        // 회전을 안할지(0), 왼쪽으로 회전(1), 오른쪽으로 회전(2)
        int turnAmount = 0;
        if (DiscreteActions[1] == 1)
        {
            turnAmount = -1;
        }
        else if (DiscreteActions[1] == 2)
        {
            turnAmount = 1;
        }

        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        AddReward(-1f / MaxStep);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            DiscreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            DiscreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            DiscreteActionsOut[1] = 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("food"))
        {
            EatFish(collision.gameObject);
        }
        /*else if (collision.gameObject.CompareTag("BABY_PENGUIN"))
        {
            RegurgitateFish();
        }*/
    }

    private void EatFish(GameObject fishObject)
    {
        if (isFull) return;
        isFull = true;

        //animalArea.RemoveFishInList(fishObject);
        AddReward(1f);
    }

/*    private void RegurgitateFish()
    {
        if (!isFull) return;
        isFull = false;

        GameObject regurgitatedFish = Instantiate(regurgitatedFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.localPosition = babyPenguin.transform.localPosition + Vector3.up * 0.01f;
        Destroy(regurgitatedFish, 4f);

        GameObject heart = Instantiate(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.localPosition = babyPenguin.transform.localPosition + Vector3.up;
        Destroy(heart, 4f);

        AddReward(1f);

        if (animalArea.remainingFish <= 0)
        {
            EndEpisode();
        }
    }*/
}
