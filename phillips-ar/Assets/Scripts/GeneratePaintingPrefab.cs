using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

/* Painting Prefab generation script that creates a 2D mesh from each one and extrudes
 * a 3D version as well. These meshes are then applied to a MaskObject. The
 * 2D quad is then applied with the sprite material. After all objects are fitted
 * together like a puzzle, they are then wrapped in their own containing objects
 * with UI elements added as well. 
 */
public class GeneratePaintingPrefab : MonoBehaviour {
    public string paintingName;
    public float extrusionDepth = .2f;
    public GameObject bioCard;
    public GameObject contextCard;
    public GameObject controlCard;
    public GameObject maskObject;
    public GameObject blackoutQuad;
    public Sprite[] sprites;

    private GameObject[] maskObjects;

    // Applies meshes to GameObjects from Sprites
    [ContextMenu("Generate Prefab")]
    public void GeneratePrefab() {
        maskObjects = new GameObject[sprites.Length];

        // Applies every sprite mesh to a new MaskObject prefab
        for (int i = 0; i < sprites.Length; i++)
        {
            // Create a new MaskObject
            maskObjects[i] = Instantiate(maskObject);
            MeshFilter meshFilter3D =
                maskObjects[i].transform.GetChild(0).GetComponent<MeshFilter>();
            MeshFilter meshFilterQuad =
                maskObjects[i].transform.GetChild(1).GetComponent<MeshFilter>();
            MeshCollider meshColliderQuad = 
                maskObjects[i].transform.GetChild(1).GetComponent<MeshCollider>();

            Mesh mesh3D = new Mesh();
            Mesh meshQuad = new Mesh();

            // Create extruded version of sprite mesh for 3D parent
            Matrix4x4[] matrix = {
                    new Matrix4x4(
                    new Vector4(1, 0, 0, 0),
                    new Vector4(0, 1, 0, 0),
                    new Vector4(0, 0, 1, 0),
                    new Vector4(0, 0, 0, 1)),

                    new Matrix4x4(
                    new Vector4(1, 0, 0, 0),
                    new Vector4(0, 1, 0, 0),
                    new Vector4(0, 0, 1, 0),
                    new Vector4(0, 0, extrusionDepth, 1))
                };
            MeshExtrusion.ExtrudeMesh(Create2DMesh(sprites[i]), mesh3D, matrix, false);

            // Create 2D mesh for quad
            meshQuad = Create2DMesh(sprites[i]);

            // Apply mesh to 3D parent and quad child
            meshFilter3D.mesh = mesh3D;
            meshFilterQuad.mesh = meshQuad;

            // Assign to the MeshCollider for tap recognition
            MeshCollider meshCollider = maskObjects[i].GetComponent<MeshCollider>();
            meshCollider.sharedMesh = mesh3D;

            // Add position nodes to mark edges of mesh
            Vector3 centerOfMesh = mesh3D.bounds.center;
            float meshWidthHalf = mesh3D.bounds.extents.x;
            float meshHeightHalf = mesh3D.bounds.extents.y;

            GameObject positionLeft = new GameObject();
            GameObject positionRight = new GameObject();
            GameObject positionTop = new GameObject();
            GameObject positionBottom = new GameObject();

            positionLeft.transform.parent = maskObjects[i].transform;
            positionRight.transform.parent = maskObjects[i].transform;
            positionTop.transform.parent = maskObjects[i].transform;
            positionBottom.transform.parent = maskObjects[i].transform;

            positionLeft.transform.position = meshCollider.bounds.center;
            positionRight.transform.position = meshCollider.bounds.center;
            positionTop.transform.position = meshCollider.bounds.center;
            positionBottom.transform.position = meshCollider.bounds.center;

            positionLeft.transform.rotation = maskObjects[i].transform.rotation;
            positionRight.transform.rotation = maskObjects[i].transform.rotation;
            positionTop.transform.rotation = maskObjects[i].transform.rotation;
            positionBottom.transform.rotation = maskObjects[i].transform.rotation;

            positionLeft.transform.Translate(-meshWidthHalf, 0f, 0f);
            positionRight.transform.Translate(meshWidthHalf, 0f, 0f);
            positionTop.transform.Translate(0f, meshHeightHalf, 0f);
            positionBottom.transform.Translate(0f, -meshHeightHalf, 0f);

            positionLeft.name = "PositionLeft";
            positionRight.name = "PositionRight";
            positionTop.name = "PositionTop";
            positionBottom.name = "PositionBottom";

            // Apply sprite material to sprite quad
            string filePath = "SpriteMaterials/" + paintingName + "/" + sprites[i].name;
            Material spriteMaterial = Resources.Load(filePath, typeof(Material)) as Material;

            MeshRenderer quadRenderer = maskObjects[i]
                .transform.GetChild(1).GetComponent<MeshRenderer>();
            quadRenderer.material = spriteMaterial;

            // Recalculate normals
            mesh3D.RecalculateNormals();

            // Change names
            maskObjects[i].name = sprites[i].name;
            maskObjects[i].tag = "MaskObject";

            // Add an instance of the card prefab for each piece for positioning later
            if (maskObjects[i].name != "[crop]")
            {
                GameObject contextCardInstance = Instantiate(contextCard,
                                                         centerOfMesh,
                                                         maskObjects[i].transform.rotation);
                contextCardInstance.transform.parent = maskObjects[i].transform;
                contextCardInstance.name = "ContextCard";
                maskObjects[i].GetComponent<PieceManager>().contextCard = contextCardInstance;
                contextCardInstance.SetActive(false);
            }

            string name3D = sprites[i].name + "3D";
            string nameQuad = sprites[i].name + "Quad";
            mesh3D.name = name3D;
            meshQuad.name = nameQuad;
        }

        // Wrap MaskObjects in empty painting parent
        GameObject paintingEmpty = new GameObject();
        paintingEmpty.name = "Painting";
        paintingEmpty.tag = "PaintingPrefab";
        foreach (GameObject g in maskObjects)
        {
            g.transform.parent = paintingEmpty.transform;
        }

        // Add properly sized BlackoutQuad prefab
        GameObject newBlackoutQuad =
            Instantiate(blackoutQuad,
                        new Vector3(0, 0, extrusionDepth),
                        Quaternion.identity);
        newBlackoutQuad.name = "BlackoutQuad";
        newBlackoutQuad.tag = "BlackoutQuad";
        newBlackoutQuad.transform.parent = paintingEmpty.transform;

        Bounds parentBounds = new Bounds(paintingEmpty.transform.position, Vector3.one);
        for (int i = 0; i < maskObjects.Length; i++) {
            Mesh m = maskObjects[i].transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            Vector3 bounds = m.bounds.size;
            parentBounds.Encapsulate(bounds);
        }
        newBlackoutQuad.transform.localScale = parentBounds.size;

        // Add the PaintingManager script
        paintingEmpty.AddComponent<PaintingManager>();
        PaintingManager pm = paintingEmpty.GetComponent<PaintingManager>();
        pm.bounds = parentBounds;

        // Create another empty parent for entire prefab
        GameObject rootEmpty = new GameObject();
        rootEmpty.name = paintingName;
        rootEmpty.tag = "PaintingWrapper";

        // Create an empty parent for the UI cards
        GameObject UIEmpty = new GameObject();
        UIEmpty.name = "UICards";
        UIEmpty.tag = "UI";

        // Attach the empties to the root
        paintingEmpty.transform.parent = rootEmpty.transform;
        UIEmpty.transform.parent = rootEmpty.transform;

        // Add the instances of the card prefabs for positioning later
        GameObject bioCardInstance = Instantiate(bioCard,
                                                 UIEmpty.transform.position,
                                                 UIEmpty.transform.rotation);
        bioCardInstance.transform.parent = UIEmpty.transform;
        bioCardInstance.name = "BioCard";
        pm.bioCard = bioCardInstance;

        GameObject controlCardInstance = Instantiate(controlCard,
                                                     UIEmpty.transform.position,
                                                     UIEmpty.transform.rotation);
        controlCardInstance.transform.parent = UIEmpty.transform;
        controlCardInstance.name = "ControlCard";
        pm.controlCard = controlCardInstance;

        // REMOVE THIS LATER
        //controlCard.transform.Translate(0f, -parentBounds.size.y - .05f, 0f);

        // Rotate the prefab
        float x = rootEmpty.transform.rotation.x;
        rootEmpty.transform.Rotate(new Vector3(x + 90, 0, 0));
    }

    // Create a simple 2D mesh from Sprite vertices
    private Mesh Create2DMesh(Sprite s) {
        Mesh mesh = new Mesh
        {
            vertices = System.Array.ConvertAll(s.vertices, v => (Vector3)v),
            uv = s.uv,
            triangles = System.Array.ConvertAll(s.triangles, v => (int)v)
        };
        return mesh;
    }
}
