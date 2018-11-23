using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StopLookAroundStateMachine : StateMachineBehaviour {

    private GameObject _enemy;
    private NavMeshAgent _agent;
    private GameObject _player;
    private float _rechargeTimer;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _rechargeTimer = 3.0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_enemy.transform.position);
        // If player is too close while the enemy has no ammo, enemy starts fleeing. Otherwise, the enemy takes 3 sec to recharge.
        if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 8 && _enemy.GetComponent<AI>()._ammo <= 0)
        {
            animator.SetInteger("aiState", 5);
        }
        else if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 8 && _enemy.GetComponent<AI>()._ammo > 0)
        {
            animator.SetInteger("aiState", 3);
        }
        else if (Vector3.Distance(_enemy.transform.position, _player.transform.position) > 8)
        {
            _rechargeTimer -= Time.deltaTime;

            if (_rechargeTimer >= 2.5f)
            {
                _enemy.transform.rotation *= Quaternion.Euler(0, -45f * 5 * Time.deltaTime, 0);
            }
            else if (_rechargeTimer >= 0.5f)
            {
                _enemy.transform.rotation *= Quaternion.Euler(0, 45f * 3 * Time.deltaTime, 0);
            }
            else if (_rechargeTimer > 0)
            {
                _enemy.transform.rotation *= Quaternion.Euler(0, -45f * 7 * Time.deltaTime, 0);
            }
            else if (_rechargeTimer <= 0)
            {
                _enemy.GetComponent<AI>().SetAmmo(3);

                // Upon completing recharge, if player is within attack range, start attacking again, otherwise go back to patrol.
                if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 10)
                {
                    animator.SetInteger("aiState", 3);
                }
                else
                {
                    animator.SetInteger("aiState", 0);
                    _enemy.GetComponent<AI>().SetPlayerSeen(false);
                }
            }
        }
    }
}
