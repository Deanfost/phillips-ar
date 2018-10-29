using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Responsible for managing individual pieces of the PaintingPrefab. Handles
 * input response and invokes callbacks to PaintingManager.cs.
 */
public class PieceManager : MonoBehaviour
{
    public GameObject UICard;

    private bool floatForward = true;

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
        if (name != "[crop]")
        {
            if (PaintingManager.piecesCanLevitate && floatForward)
            {
                transform.Translate(new Vector3(0f, 0f, -PaintingManager.floatFactor));
                InflateContextCard();
                PaintingManager.ToggleLevitatePrivileges();
                floatForward = !floatForward;
            }
            else if (!PaintingManager.piecesCanLevitate && !floatForward)
            {
                transform.Translate(new Vector3(0f, 0f, PaintingManager.floatFactor));
                PaintingManager.ToggleLevitatePrivileges();
                floatForward = !floatForward;
            }
        }
    }

    // Instantiates and inflates a ContextCard next to the piece containing information on that piece
    private void InflateContextCard() {
        // Calculate scale of card (use the bounds to figure it out)
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh m = mf.mesh;

        // Calculate new position based off of piece position relative to rest of painting (left or right side?)
        Vector3 position = new Vector3(transform.position.x + transform.position.y, transform.position.z - .05f);


        GameObject card = Instantiate(UICard, position, Quaternion.identity);
    }
}