using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;

/* A C# object that is responsible for the management of the parent 
 * PaintingPrefab and assigned image. It handles the proper scaling of the 
 * prefab depending on the image (target) property assigned. 
 * 
 */
public class PaintingManager : MonoBehaviour {
    public AugmentedImage image;

    private Vector3 boundsSize = new Vector3();

    private void Start()
    {
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "BlackoutQuad")
            {
                boundsSize = child.GetComponent<Renderer>().bounds.size;
                break;
            }
        }
    }

    private void Update()
    {
        if (image == null || image.TrackingState != TrackingState.Tracking) {
            gameObject.SetActive(false);
            return;
        }

        // Scale the prefab to the image target
        float imageWidth = image.ExtentX;
        float imageHeight = image.ExtentZ;
        float sizeX = boundsSize.x;
        float sizeY = boundsSize.y;
        Vector3 scale = gameObject.transform.localScale;
        scale.x = imageWidth * scale.x / sizeX;
        scale.y = imageHeight * scale.y / sizeY;
        gameObject.transform.localScale = scale;
    }
}
