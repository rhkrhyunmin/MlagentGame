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

    private float timer;

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

        // 에이전트가 가장 가까운 타겟을 향해 이동하도록 합니다.
        if (_goalTrm != null)
        {
            Vector3 targetDirection = _goalTrm.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(targetDirection);
            _rigid.velocity = transform.forward * moveSpeed;
        }
        //Debug.Log(GetCumulativeReward());
        AddReward(-1f / MaxStep);

        //StartCoroutine(AddNegativeRewardCoroutine());
    }

    IEnumerator AddNegativeRewardCoroutine()
    {
        while (true)
        {
            AddReward(-0.1f); // 보상으로 -1을 적용
            yield return new WaitForSeconds(1f); // 1초 대기
        }
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
        if (collision.gameObject.CompareTag("OBJ"))
        {
            //EatFish(collision.gameObject);
        }
       if(HasReachedGoal())
        {
            EndEpisode();
        }
    }
    private bool HasReachedGoal()
    {
        float distanceToGoal = Vector3.Distance(transform.position, _goalTrm.position);
        Debug.Log(distanceToGoal);
        if(distanceToGoal < 1.5f)
        {
            AddReward(1f);
        }
        return distanceToGoal < 0.5f;
    }


}
