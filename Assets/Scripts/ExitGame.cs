using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ExitGame : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {   
        // On reaching the top of the map, exit editor playing mode.
        if (other.tag == "Player")
        {
            EditorApplication.isPlaying = false;
        }
    }
}
