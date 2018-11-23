using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetStateMachine : StateMachineBehaviour {

    private bool _enemyLeftForWaypoint;
    private GameObject _enemy;
    private Transform[] _waypoints;
    private int _currentWaypointIndex;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemyLeftForWaypoint = false;
        _enemy = animator.gameObject;
        _waypoints = _enemy.GetComponent<AI>()._waypoints;
        _currentWaypointIndex = _enemy.GetComponent<AI>()._currentWaypointIndex;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bool stillInWaypoint = false;
        // Enemy is assumed to be still in waypoint range, when it is no longer in range of any waypoint, assuming the enemy
        // left its last waypoint, and is on its way to next waypoint.
        if (!_enemyLeftForWaypoint)
        {
            for (int i = 0; i < _waypoints.Length; i++)
            {
                
                if (Vector3.Distance(_enemy.transform.position, _waypoints[i].transform.position) < 0.5)
                {
                    stillInWaypoint = true;
                    break;
                }
            }
            if (!stillInWaypoint)
            {
                _enemyLeftForWaypoint = true;
            }
        }

        if (_enemyLeftForWaypoint)
        {
            // If waypoint reached, look for next waypoint
            if (Vector3.Distance(_enemy.transform.position, _waypoints[_currentWaypointIndex % _waypoints.Length].transform.position) < 0.5)
            {
                animator.SetInteger("aiState", 0);
            }
        }

        // If player is seen, start chasing
        if (_enemy.GetComponent<AI>()._playerSeen)
        {
            animator.SetInteger("aiState", 2);
        }
    }
}
