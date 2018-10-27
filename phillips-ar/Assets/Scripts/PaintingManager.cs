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
    public AugmentedImage image;
    public Vector3 boundsSize = new Vector3();
    public static bool piecesCanLevitate = true;

    private bool shouldCalcScale = true;
    private List<GameObject> children3D = new List<GameObject>();
    private List<GameObject> childrenQuad = new List<GameObject>();
    private List<InfoCard> cards = new List<InfoCard>(); 

    private void Start() {
        // Gather all 3D and 2D painting pieces
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Transform t = gameObject.transform.GetChild(i);
            if (t.tag == "MaskObject")
            {
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
            // Scale the prefab relative to the image target
            float targetWidth = image.ExtentX;
            float targetHeight = image.ExtentZ;
            float currentWidth = boundsSize.x;
            float currentHeight = boundsSize.y;
            Vector3 scale = transform.localScale;
            scale.x = targetWidth * scale.x / currentWidth;
            scale.y = targetHeight * scale.y / currentHeight;
            gameObject.transform.localScale = scale;
            shouldCalcScale = false;
        }

        gameObject.SetActive(true);
    }

    // Notify class if pieces should respond to touch input
    public static void ToggleLevitatePrivileges() {
        piecesCanLevitate = !piecesCanLevitate;
    } 

    // Instantiates and inflates a new information card 
    public static void InflateContextCard() {
     
    }
}
