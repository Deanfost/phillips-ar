using System;
using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;

/* A C# object that is responsible for the management of the parent 
 * PaintingPrefab and assigned image. It handles the proper scaling of the 
 * prefab depending on the image (target) property assigned. 
 * 
 */
public class PaintingManager : MonoBehaviour {
    [HideInInspector]
    public AugmentedImage image;
    [HideInInspector]
    public Bounds bounds; // Assigned during prefab generation
    [HideInInspector]
    public float floatFactor = .2f;

    public GameObject bioCard;
    public GameObject controlCard;

    private bool piecesCanLevitate = true;
    private bool shouldCalcScale = true;
    private bool shouldInitBioCard = true;

    private List<GameObject> children3D = new List<GameObject>();
    private List<GameObject> childrenQuad = new List<GameObject>();

    private string JSONFilePath;

    private void Start() {
        // Gather all 3D and 2D painting pieces, set references
        Debug.Log("I'm here!");
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Transform t = gameObject.transform.GetChild(i);
            if (t.tag == "MaskObject")
            {
                t.GetComponent<PieceManager>().paintingManager = this;
                children3D.Add(t.GetChild(0).gameObject);
                childrenQuad.Add(t.GetChild(1).gameObject);
            }
        }
    }

    private void Update() {
        if (image == null || image.TrackingState != TrackingState.Tracking) {
            gameObject.SetActive(false);
            return;
        }

        if (shouldCalcScale) {
            // Scale the root container of the prefab relative to the painting
            float targetWidth = image.ExtentX;
            float targetHeight = image.ExtentZ;
            float currentWidth = bounds.size.x;
            float currentHeight = bounds.size.y;
            Vector3 scale = transform.localScale;
            scale.x = targetWidth * scale.x / currentWidth;
            scale.y = targetHeight * scale.y / currentHeight;
            gameObject.transform.parent.localScale = scale;
            shouldCalcScale = false;
        }

        gameObject.SetActive(true);

        // Instantiate the card if needed 
        if (shouldInitBioCard) {
            InitInfoCard();
        }
    }

    // Notify class if pieces should respond to touch input
    public void ToggleLevitatePrivileges() {
        piecesCanLevitate = !piecesCanLevitate;
    } 

    // Return levitate privileges
    public bool CheckLevitatePrivileges() {
        return piecesCanLevitate;
    }

    // Inflates a new information card next to the model
    private void InitInfoCard() {

    }

    // Deflates the information card
    private void DeflateInfoCard() {

    }

    // Inflates a new control card at the bottom of the model
    private void InflateControlCard() {

    }

    // Deflates the control card 
    private void DeflateControlCard() {

    }
}
