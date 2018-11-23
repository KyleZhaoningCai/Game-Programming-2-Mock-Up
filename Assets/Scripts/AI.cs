using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour {

    public Transform[] _waypoints;
    public int _currentWaypointIndex = 99;
    public float _fieldOfView = 110f;
    public bool _playerSeen;
    public int _ammo = 3;
    public GameObject _enemyItself;
    public Rigidbody _bullet;

    private GameObject _player;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerSeen = false;
    }

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void SetCurrentWaypointIndex(int index)
    {
        _currentWaypointIndex = index;
    }

    public void SetAmmo(int ammo)
    {
        _ammo = ammo;
    }

    public void SetPlayerSeen(bool seen)
    {
        _playerSeen = seen;
    }

    // Up on entering enemy collider, if the player is within the fieldOfView, the enemy cast a ray to check if there is a wall in between.
    // If there is no wall, the player is seen.
    void OnTriggerStay(Collider other)
    {
        Vector3 evenUpHeight = new Vector3(other.transform.position.x, this.transform.position.y, other.transform.position.z);
        Vector3 direction = evenUpHeight - this.transform.position;
        Vector3 forward = transform.forward;
        float angle = Vector3.Angle(direction, forward);
        if (angle < _fieldOfView * 0.5f)
        {
            RaycastHit hit;
            
            if (Physics.Raycast(new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z), direction.normalized, out hit, this.GetComponent<SphereCollider>().radius))
            {
                if (hit.collider.gameObject == _player)
                {
                    _playerSeen = true;
                }
            }
            
        }
    }
}
