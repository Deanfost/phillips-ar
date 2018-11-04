using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Responsible for managing individual pieces of the PaintingPrefab. Handles
 * input response and invokes callbacks to PaintingManager.cs.
 */
public class PieceManager : MonoBehaviour
{
    [HideInInspector]
    public GameObject UICardPrefab;
    [HideInInspector]
    public PaintingManager paintingManager;

    private bool floatForward = true;
    private float moveStep = .025f;
    private readonly float scaleX = .35f;
    private readonly float scaleY = .4f;
    //private readonly float horiMargin = .1f;
    //private readonly float zOffset = .02f;

    private MeshCollider pieceCollider;
    private GameObject parent3D;
    private GameObject textureQuad;
    private GameObject contextCard;
    private GameObject targetPositionNode;

    private void Start()
    {
        parent3D = transform.GetChild(0).gameObject;
        textureQuad = transform.GetChild(1).gameObject;
        pieceCollider = GetComponent<MeshCollider>();

        // Setup the target position node
        targetPositionNode = new GameObject();
        targetPositionNode.transform.position = transform.position;
        targetPositionNode.transform.parent = transform.parent;
        targetPositionNode.transform.rotation = targetPositionNode.transform.parent.rotation;
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
                StartCoroutine(DelayCardInflation());
            }
            else if (!paintingManager.CheckLevitatePrivileges() && !floatForward)
            {
                // Move the target node backward
                targetPositionNode.transform.Translate(0f, 0f, paintingManager.floatFactor);
                paintingManager.ToggleLevitatePrivileges();
                floatForward = true;
                DeflateContextCard();
            }
        }
    }

    // Instantiates and inflates a Context Card next to the piece containing information on that piece
    private void InflateContextCard() {
        // Instantiate the context card
        contextCard = Instantiate(UICardPrefab, pieceCollider.bounds.center, transform.rotation);
        contextCard.transform.parent = transform;

        // Scale the contextCard to a percentage of the painting size
        Vector3 boundsSize = contextCard.GetComponent<BoxCollider2D>().bounds.size;
        float imageWidth = paintingManager.image.ExtentX;
        float imageHeight = paintingManager.image.ExtentZ;
        float currentWidth = boundsSize.x;
        float currentHeight = boundsSize.y;

        Vector3 newScale = new Vector3
        {
            x = imageWidth * contextCard.transform.localScale.x * scaleX / currentWidth,
            y = imageHeight * contextCard.transform.localScale.y * scaleY / currentHeight
        };
        newScale.y = newScale.y * 2f;

        contextCard.transform.localScale = newScale;

        // Calculate new position 
        Sprite cardSprite = contextCard.GetComponent<SpriteRenderer>().sprite;
        float cardHalfWidth = cardSprite.rect.width /
                                        cardSprite.pixelsPerUnit / 2 /
                                        contextCard.transform.localScale.x;
        Debug.Log(contextCard.transform.localScale);
        float translation = pieceCollider.bounds.extents.x;

        // Position the card to the right of the piece
        contextCard.transform.Translate(translation, 0, 0);
        float difference = contextCard.transform.position.x - pieceCollider.bounds.center.x;
        Debug.Log(difference);
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