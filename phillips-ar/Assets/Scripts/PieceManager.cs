using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Responsible for managing individual pieces of the PaintingPrefab. Handles
 * input response and invokes callbacks to PaintingManager.cs.
 */
public class PieceManager : MonoBehaviour {
    public float floatFactor = .005f;

    private bool floatForward = true;
    private bool reactToInput = true;
    private GameObject mesh3D;
    private GameObject meshQuad;

    private void Start()
    {
        mesh3D = transform.GetChild(0).gameObject;
        meshQuad = transform.GetChild(1).gameObject;
    }

    // Catch touch input
    private void OnMouseDown()
    {
        // Toggle levitation if no other pieces are floating (and we're not the cropped background)
        if (name != "[crop]") {
            if (reactToInput) {
                if (floatForward && PaintingManager.piecesCanLevitate)
                {
                    StartCoroutine(LevitateTransition());
                    reactToInput = false;
                    floatForward = !floatForward;
                    PaintingManager.ToggleLevitatePrivileges();
                }
                else if (!floatForward && !PaintingManager.piecesCanLevitate)
                {
                    StartCoroutine(LevitateTransition());
                    reactToInput = false;
                    floatForward = !floatForward;
                    PaintingManager.ToggleLevitatePrivileges();
                }
            }
        }
    }

    // Animates the piece back and forth
    private IEnumerator LevitateTransition() {
        if (floatForward) {
            float targetZPos = transform.position.z - floatFactor;
            while (transform.position.z > targetZPos) {
                transform.Translate(0f, 0f, -.0008f);
                yield return new WaitForSeconds(.01f);
            }

            reactToInput = true;
        }
        else  {
            float targetZPos = transform.position.z + floatFactor;
            while (transform.position.z < targetZPos) {
                transform.Translate(0f, 0f, .0008f);
                yield return new WaitForSeconds(.01f);
            }

            reactToInput = true;
        }
    }
}