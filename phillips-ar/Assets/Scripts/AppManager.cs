using System.Collections.Generic;
using GoogleARCore;
using GoogleARCore.Examples.AugmentedImage;
using UnityEngine;
using UnityEngine.UI;

/* Application manager for ARCore Augmented Images. 
 * Controls motion tracking, session updating, anchor spawning/tracking, and 
 * interaction with the current PaintingPrefab.
 */
public class AppManager : MonoBehaviour {
    public GameObject[] paintingPrefabs;
    //public AugmentedImageVisualizer augmentedImageVisualizerPrefab;
    public GameObject hintOverlay;

    // Currently active visualizers
    private Dictionary<int, GameObject> activeVisualizers =
        new Dictionary<int, GameObject>();
    private List<AugmentedImage> currentAugmentedImages = new List<AugmentedImage>();

    public void Update() {
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        // Don't do anything if we aren't motion tracking
        if (Session.Status != SessionStatus.Tracking) {
            return;
        }

        // Get the current trackables for this frame
        Session.GetTrackables<AugmentedImage>(currentAugmentedImages, TrackableQueryFilter.Updated);

        // Create anchors and visualizers for trackables that do not have one already, and 
        // remomve any for stopped images
        foreach (AugmentedImage image in currentAugmentedImages) {
            GameObject visualizer = null;
            activeVisualizers.TryGetValue(image.DatabaseIndex, out visualizer);
            if (image.TrackingState == TrackingState.Tracking && visualizer == null) {
                // We are tracking the image, but do not have a PaintingPrefab, add it, and add anchor
                Anchor anchor = image.CreateAnchor(image.CenterPose);
                visualizer = Instantiate(paintingPrefabs[image.DatabaseIndex], anchor.transform);
                visualizer.transform.GetChild(0).GetComponent<PaintingManager>().image = image;
                activeVisualizers.Add(image.DatabaseIndex, visualizer);
                Debug.Log("Adding visualizer...");
            }
            else if (image.TrackingState == TrackingState.Stopped && visualizer != null) {
                // We are no longer tracking the image, remove the visualizer
                Debug.Log("Removing visualizer...");
                activeVisualizers.Remove(image.DatabaseIndex);
                Destroy(visualizer.gameObject);
            }
        }

        // Show the hint overlay if there are no tracked images
        foreach (var visualizer in activeVisualizers.Values) {
            if (visualizer.transform.GetChild(0)
                .GetComponent<PaintingManager>().image.TrackingState == TrackingState.Tracking) {
                hintOverlay.SetActive(false);
                return;
            }
        }
        hintOverlay.SetActive(true);
    }
}