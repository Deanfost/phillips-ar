using System;
using System.IO;
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
    [HideInInspector]
    public string paintingName;

    public GameObject bioCard;
    public GameObject controlCard;

    private bool piecesCanLevitate = true;
    private bool shouldCalcScale = true;
    private bool shouldInitBioCard = true;

    private List<GameObject> children3D = new List<GameObject>();
    private List<GameObject> childrenQuad = new List<GameObject>();

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

        paintingName = transform.parent.name;

        // Join the JSON path
        string JSONFilePath = "Assets/Resources/JSON/" + paintingName + "/" + paintingName + ".json";

        // Parse the JSON
        ParseJSON(JSONFilePath);

        // Init the Control, Bio, and Context Cards
        InitBioAndControlCard();
        foreach (GameObject g in transform) {
            if (g.name != "[crop]" && g.tag == "MaskObject") {
                // Init the Context Cards
                g.GetComponent<PieceManager>().InitContextCard();
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
    }

    // Notify class if pieces should respond to touch input
    public void ToggleLevitatePrivileges() {
        piecesCanLevitate = !piecesCanLevitate;
    } 

    // Return levitate privileges
    public bool CheckLevitatePrivileges() {
        return piecesCanLevitate;
    }

    // Initializes Bio Card with information from JSON
    private void InitBioAndControlCard() {

    }

    // Parses the JSON file and stores info inside of an object
    private void ParseJSON(string filePath) {
        if (File.Exists(filePath)) {
            string jsonFromFile = File.ReadAllText(filePath);
        }
        else {
            Debug.LogError("Invalid JSON file path!");
        }
    }

    // Animate the Control and Bio cards in
    private IEnumerator DisplayUICards() {
        return null;
    }

    // Animate the Control and Bio cards out
    private IEnumerator HideUICards() {
        return null;
    }
}
