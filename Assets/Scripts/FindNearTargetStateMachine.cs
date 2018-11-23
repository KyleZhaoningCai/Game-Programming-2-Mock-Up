using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindNearTargetStateMachine : StateMachineBehaviour {

    private GameObject _enemy;
    private NavMeshAgent _agent;
    private Transform[] _waypoints;
    private int _currentWaypointIndex;
    private float _waitTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        // Enemy will wait for 1 sec upon reaching waypoint
        _waitTimer = 1.0f;
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _waypoints = _enemy.GetComponent<AI>()._waypoints;
        _currentWaypointIndex = _enemy.GetComponent<AI>()._currentWaypointIndex;

        // Set current waypoint index, the index will increment infinitely, and real waypoint index is calculated by currentWaypointIndex / waypoint array length
        if (_currentWaypointIndex == 99)
        {
            _currentWaypointIndex = 0;
        }
        else
        {
            _currentWaypointIndex++;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _waitTimer -= Time.deltaTime;

        // After waiting for 1 sec, set a new waypoint for the enemy.
        if (_waitTimer <= 0)
        {
            _agent.SetDestination(_waypoints[_currentWaypointIndex % _waypoints.Length].position);
            animator.SetInteger("aiState", 1);
            _enemy.GetComponent<AI>().SetCurrentWaypointIndex(_currentWaypointIndex);
        }
    }
}
