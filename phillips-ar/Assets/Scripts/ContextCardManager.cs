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
}
