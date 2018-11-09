using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Responsible for managing individual pieces of the PaintingPrefab. Handles
 * input response and invokes callbacks to PaintingManager.cs.
 */
public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public PaintingManager paintingManager;

    [SerializeField]
    private GameObject contextCardInstance;
    private bool floatForward = true;
    private float moveStep = .025f;
    private readonly float scaleX = .35f;
    private readonly float scaleY = .4f;
    private readonly float horiMargin = .02f;

    private MeshRenderer pieceRenderer;
    private GameObject parent3D;
    private GameObject textureQuad;
    private GameObject contextCard;
    private GameObject targetPositionNode;

    private string JSONFilePath;

    private void Start()
    {
        // Setup references
        parent3D = transform.GetChild(0).gameObject;
        textureQuad = transform.GetChild(1).gameObject;
        pieceRenderer = GetComponent<MeshRenderer>();

        // Setup the target position node
        targetPositionNode = new GameObject();
        targetPositionNode.transform.position = transform.position;
        targetPositionNode.transform.parent = transform.parent;
        targetPositionNode.transform.rotation = targetPositionNode.transform.parent.rotation;

        // Get the reference to Context Card
        
    }

    private void Update()
    {
        // Constantly move the piece towards the target position node
        transform.position = Vector3.MoveTowards(transform.position, targetPositionNode.transform.position,moveStep);
    }

    // Catch touch input
    private void OnMouseDown()
    {
        // Toggle levitation if no other pieces are floating (and we're not the cropped background)
        if (name != "[crop]")
        {
            if (paintingManager.CheckLevitatePrivileges() && floatForward)
            {
                // Move the target node forward
                targetPositionNode.transform.Translate(0f, 0f, -paintingManager.floatFactor);
                paintingManager.ToggleLevitatePrivileges();
                floatForward = false;
                //StartCoroutine(DelayCardInflation());
            }
            else if (!paintingManager.CheckLevitatePrivileges() && !floatForward)
            {
                // Move the target node backward
                targetPositionNode.transform.Translate(0f, 0f, paintingManager.floatFactor);
                paintingManager.ToggleLevitatePrivileges();
                floatForward = true;
                //DeflateContextCard();
            }
        }
    }

    // Populates the context card with information on the piece
    private void InflateContextCard() {
        // Parse the JSON file

        // Get references to UI elements

        // Populate information
        
       
    }
     
    // Destroys the inflated Context Card
    private void DeflateContextCard() {
        Destroy(contextCard);
    }

    // Wait before inflating the Context Card
    private IEnumerator DelayCardInflation() {
        yield return new WaitForSeconds(.5f);
        // If we're in the forward position, inflate the card
        if (!floatForward) {
            InflateContextCard();
        }
    }
}