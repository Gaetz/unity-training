using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetDestructor : MonoBehaviour {

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Target")
        {
            Destroy(other.gameObject);
        }
    }
}
