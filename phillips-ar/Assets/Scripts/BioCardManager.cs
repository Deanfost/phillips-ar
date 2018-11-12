using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* A simple manager for the Bio Card prefab. It holds a reference to the paragraph
 * prefab, so that we can add paragrpahs if need be. It also exposes a simple API
 * for other classes to manipulate the card with. 
 */
public class BioCardManager : MonoBehaviour {
    // Paragraph prefab
    public GameObject paragraph;

    [HideInInspector]
    public Text bioName, bioArtist;
    [HideInInspector]
    public Image bioImage;

    private GameObject contentObject;
    private List<Text> paragraphs = new List<Text>();

    public void Start() {
        contentObject = transform
            .GetChild(0)
            .GetChild(0)
            .GetChild(0)
            .GetChild(0).gameObject;
        bioName = contentObject.transform.GetChild(1).GetComponent<Text>();
        bioArtist = contentObject.transform.GetChild(2).GetComponent<Text>();
        bioImage = contentObject.transform.GetChild(3).GetComponent<Image>();
        paragraphs.Add(contentObject.transform.GetChild(4).GetComponent<Text>());
    }

    // Returns a reference to a new text element referencing an added paragraph
    public Text AddParagraph() {
        GameObject newParagraph = Instantiate(paragraph, transform.position, transform.rotation);
        newParagraph.transform.parent = contentObject.transform;
        return newParagraph.GetComponent<Text>();
    }
}
