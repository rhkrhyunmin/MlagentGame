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
    public Transform _goalTrm;
    public Transform _startTrm;

    private CharacterController characterController;
    private Rigidbody _rigid;

    private GameObject babyPenguin;

    private bool isFull;

    public override void Initialize()
    {
        characterController = GetComponent<CharacterController>();
        _rigid = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // ???????? ?????? 8?????.
        //sensor.AddObservation(isFull);
        sensor.AddObservation(Vector3.Distance(transform.position, _goalTrm.transform.position));
        sensor.AddObservation((_goalTrm.transform.position - transform.position).normalized);
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

        _rigid.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        Debug.Log(forwardAmount);
        transform.Rotate(Vector3.up * turnAmount * 100 * Time.fixedDeltaTime);

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("OBJ"))
        {
            EatFish(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
             AddReward(1f);
        }
    }

    private void EatFish(GameObject fishObject)
    {
        if (isFull) return;
        isFull = true;
    }

}
