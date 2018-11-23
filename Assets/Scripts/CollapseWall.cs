using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollapseWall : MonoBehaviour {

    private Vector3 endPosition = new Vector3(12.22f, -1.36f, -39.5f);
    private bool collapsed = false;

    void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.LeftControl) && !collapsed)
        {
            collapsed = true;
            this.gameObject.transform.position = endPosition;
            this.gameObject.transform.Rotate(0, 0, 90);

        }
    }
}
