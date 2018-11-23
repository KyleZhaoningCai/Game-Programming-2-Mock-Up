using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FireStateMachine : StateMachineBehaviour {

    private GameObject _enemy;
    private NavMeshAgent _agent;
    private GameObject _player;
    private float _attackAnimationDuration;
    private Rigidbody _bullet;
    private int _speed = 100;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        _enemy = animator.gameObject;
        _agent = _enemy.GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");      
        _attackAnimationDuration = 0.0f;
        _bullet = _enemy.GetComponent<AI>()._bullet;
        
        // If enemy has no ammo to start with due to unknown reason, instantly begin fleeing or recharging.
        if (_enemy.GetComponent<AI>()._ammo <= 0)
        {
            if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 8)
            {
                animator.SetInteger("aiState", 5);
            }
            else
            {
                animator.SetInteger("aiState", 4);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent.SetDestination(_enemy.transform.position);
        _attackAnimationDuration -= Time.deltaTime;

        // Every time the attack animation plays, reduce ammo by one, and if there is no ammo left, flee or recharge.
        if (_attackAnimationDuration <= 0)
        {
            _enemy.transform.LookAt(_player.transform);
            Rigidbody bullet = Instantiate(_bullet, _enemy.transform.position + new Vector3(0, 0, 0), _enemy.transform.rotation);
            bullet.velocity = _enemy.transform.TransformDirection(0, 0, _speed);
            _enemy.GetComponent<AI>().SetAmmo(_enemy.GetComponent<AI>()._ammo - 1);
            _attackAnimationDuration = 1.0f;
        }

        if (_enemy.GetComponent<AI>()._ammo <= 0)
        {
            if (Vector3.Distance(_enemy.transform.position, _player.transform.position) <= 8)
            {
                animator.SetInteger("aiState", 5);
            }
            else
            {
                animator.SetInteger("aiState", 4);
            }
        }
        else
        {   
            // If player runs further away, chase again.
            if (Vector3.Distance(_enemy.transform.position, _player.transform.position) > 10)
            {
                animator.SetInteger("aiState", 2);
            }         
        }
    }
}
