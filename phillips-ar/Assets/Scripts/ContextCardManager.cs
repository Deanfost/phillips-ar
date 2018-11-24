using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A simple manager for the Context Card prefab. It exposes a simple API
 * for other classes to manipulate the card with. 
 */
public class ContextCardManager : MonoBehaviour {
    [HideInInspector]
    public Text pieceTitle, pieceCaption, pieceParagraph;

    private GameObject contentObject;

    private void Awake() {
        // Setup references
        contentObject =
                transform
                    .GetChild(0)
                    .GetChild(0)
                    .GetChild(0)
                    .GetChild(0).gameObject;
        pieceTitle = contentObject.transform.GetChild(1).GetComponent<Text>();
        pieceCaption = contentObject.transform.GetChild(2).GetComponent<Text>();
        pieceParagraph = contentObject.transform.GetChild(3).GetComponent<Text>();
    }
}
