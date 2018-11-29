using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

/* A C# object that is responsible for the management of the PaintingPrefab and 
 * assigned image. It handles the proper scaling of the prefab depending on the 
 * image (target) property assigned. It also handles assignment of UI Group 
 * information.
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
    [HideInInspector]
    public PaintingJSONContainer parsedData;

    public GameObject bioCard; // Assigned during prefab generation
    public GameObject controlCard; // Assigned during prefab generation

    private BioCardManager bioManager;
    private ControlCardManager controlManager;
    private bool piecesCanLevitate = true;
    private bool shouldCalcScale = true;

    private void Start() {
        // Gather all 3D and 2D painting pieces, set references
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Transform t = gameObject.transform.GetChild(i);
            if (t.tag == "MaskObject")
            {
                t.GetComponent<PieceManager>().paintingManager = this;
            }
        }

        // Get the managers
        bioManager = bioCard.GetComponent<BioCardManager>();
        controlManager = controlCard.GetComponent<ControlCardManager>();

        // Join the JSON path
        string filePath = "JSON/" + paintingName + "/" + paintingName;

        // Load and parse the file
        TextAsset loadedJSON = Resources.Load<TextAsset>(filePath);
        if (loadedJSON != null) {
            string textFromFile = loadedJSON.text;
            JSONObject jsonObject = new JSONObject(textFromFile);
            parsedData = new PaintingJSONContainer(jsonObject);
        } 
        else {
            Debug.LogError("Invalid JSON path at " + filePath);
        }

        // Init the UI group
        InitBioCard();
        InitControlCard();

        // Init the context cards of each piece
        int index = 0;
        foreach (Transform t in transform) {
            if (t.name != "[crop]" && t.tag == "MaskObject") {
                t.GetComponent<PieceManager>().InitContextCard(parsedData, index);
                index++;
            }
        }
    }

    private void Update() {
        if (image == null || image.TrackingState != TrackingState.Tracking) {
            gameObject.SetActive(false);
            return;
        }

        if (shouldCalcScale) {
            // Scale the root container of the prefab relative to the image target
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

    // Initializes Bio card with information from the parsed JSON object
    private void InitBioCard() {
        // Set the name, artist, and image
        bioManager.bioName.text = parsedData.paintingName;
        bioManager.bioArtist.text = parsedData.artist;

        if (parsedData.imageName != "") {
            string spritePath = "InfoImages/" + paintingName + parsedData.imageName;
            Sprite loadedSprite = Resources.Load<Sprite>(spritePath);
            if (loadedSprite != null)
            {
                bioManager.bioImage.sprite = loadedSprite;
            }
            else
            {
                Debug.LogError("Invalid path for Bio Card image at " + spritePath);
            }
        }

        // Set the description
        bioManager.bioDescription.text = parsedData.description;
    }

    // Initializes Control card with information from the parsed JSON object
    private void InitControlCard() {
        controlManager.paintingName.text = parsedData.paintingName;
        controlManager.paintingArist.text = parsedData.artist;
    }
}
