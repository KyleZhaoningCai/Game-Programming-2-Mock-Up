using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FleeOppositieDirectionStateMachine : StateMachineBehaviour {

    private GameObject _enemy;
    private NavMeshAgent _agent;
    private GameObject _player;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // If player is within a short range when the enemy has no ammo, the enemy runs to opposite direction. Otherwise, the enemy recharges.
        if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 8)
        {
            _enemy.transform.rotation = Quaternion.LookRotation(_enemy.transform.position - new Vector3(_player.transform.position.x, _enemy.transform.position.y, _player.transform.position.z));
            Vector3 runTo = _enemy.transform.position + _enemy.transform.forward;
            NavMeshHit hit;
            NavMesh.SamplePosition(runTo, out hit, 5, 1);
            _agent.SetDestination(hit.position);
        }
        else
        {
            animator.SetInteger("aiState", 4);
        }

    }
}
