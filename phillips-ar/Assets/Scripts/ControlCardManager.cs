using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A simple manager for the Bio Card prefab. It exposes a simple API
 * for other classes to manipulate the card with. 
 */
public class ControlCardManager : MonoBehaviour {
    [HideInInspector]
    public Text paintingName, paintingArist;

    private GameObject canvas;

	void Start () {
        canvas = transform.GetChild(0).GetChild(0).gameObject;
        paintingName = canvas.transform.GetChild(0).GetComponent<Text>();
        paintingArist = canvas.transform.GetChild(1).GetComponent<Text>();
	}
}
