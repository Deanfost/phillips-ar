using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Simple script that destroys the root of the painting prefab */
public class CloseButton : MonoBehaviour {
    private void OnMouseDown()
    {
        Debug.Log("Close clicked, destroying painting...");
        Destroy(transform.parent.parent.parent.gameObject);
    }
}
