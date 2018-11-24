using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Responsible for managing individual pieces of the PaintingPrefab. Handles
 * input response and invokes callbacks to PaintingManager.cs.
 */
public class PieceManager : MonoBehaviour {
    [HideInInspector]
    public PaintingManager paintingManager; // Assiged during prefab generation
    [HideInInspector]
    public GameObject contextCard; // Assigned during prefab generatiom
    [HideInInspector]
    public ContextCardManager contextManager; // Assigned during prefab generation

    private bool floatForward = true;
    private float moveStep = .025f;
    private readonly float scaleX = .35f;
    private readonly float scaleY = .4f;
    private readonly float horiMargin = .02f;

    private MeshRenderer pieceRenderer;
    private GameObject parent3D;
    private GameObject textureQuad;
    private GameObject targetPositionNode;

    private string JSONFilePath;

    private void Start() {
        // Setup references
        parent3D = transform.GetChild(0).gameObject;
        textureQuad = transform.GetChild(1).gameObject;
        pieceRenderer = GetComponent<MeshRenderer>();

        // Setup the target position node
        targetPositionNode = new GameObject();
        targetPositionNode.transform.position = transform.position;
        targetPositionNode.transform.parent = transform.parent;
        targetPositionNode.transform.rotation = targetPositionNode.transform.parent.rotation;
    }

    private void Update() {
        // Constantly move the piece towards the target position node
        transform.position = Vector3.MoveTowards(transform.position, targetPositionNode.transform.position,moveStep);
    }

    // Catch touch input
    private void OnMouseDown() {
        // Toggle levitation if no other pieces are floating (and we're not the cropped background)
        if (name != "[crop]") {
            if (paintingManager.CheckLevitatePrivileges() && floatForward) {
                // Move the target node forward to floating position
                targetPositionNode.transform.Translate(0f, 0f, -paintingManager.floatFactor);
                paintingManager.ToggleLevitatePrivileges();
                floatForward = false;
                StartCoroutine(DelayContextCardDisplay());
            }
            else if (!paintingManager.CheckLevitatePrivileges() && !floatForward) {
                // Move the target position node back
                paintingManager.ToggleLevitatePrivileges();
                floatForward = true;
                targetPositionNode.transform.Translate(0f, 0f, paintingManager.floatFactor);
                contextCard.SetActive(false);
            }
        }
    }

    // Initializes Context Card with information from JSON object
    public void InitContextCard(PaintingJSONContainer parsedData, int index) {
        // Set title, caption, and description
        string title = parsedData.pieces[index].pieceName;
        string caption = parsedData.pieces[index].caption;
        string desc = parsedData.pieces[index].description;
        contextManager.pieceTitle.text = title;
        contextManager.pieceCaption.text = caption;
        contextManager.pieceParagraph.text = desc;
    }

    // Wait for the piece to be in position before displaying the card
    private IEnumerator DelayContextCardDisplay() {
        yield return new WaitForSeconds(.5f);
        if (!floatForward)
            contextCard.SetActive(true);
    }
}