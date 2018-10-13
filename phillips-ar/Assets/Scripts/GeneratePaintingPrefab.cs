using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/* Script that applies respective sprite meshes to primitives with slight 
 * modification on the z-axis for thickness. Modifies each MaskObject in the 
 * first array for each sprite in second array, containting a container parent,
 * the generated 3D mesh, and a 2D mesh with sprite texture applied. Wraps all 
 * modifed MaskObjects into a name empty object, and adds a BlackoutQuad prefab.
 * 
 * Also saves corresponding generated meshes into a "Meshes" folder in the project 
 * heirarchy. 
 */
public class GeneratePaintingPrefab : MonoBehaviour {
    public string paintingName;
    public float extrusionDepth = 1f;
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
            GameObject newMaskObject = Instantiate(maskObject);
            maskObjects[i] = newMaskObject;
            MeshFilter meshFilter3D =
                maskObjects[i].transform.GetChild(0).GetComponent<MeshFilter>();
            MeshFilter meshFilterQuad =
                maskObjects[i].transform.GetChild(1).GetComponent<MeshFilter>();

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

            // Apply sprite material to sprite quad
            string filePath = "SpriteMaterials/" + sprites[i].name;
            Material spriteMaterial = Resources.Load(filePath, typeof(Material)) as Material;

            MeshRenderer quadRenderer = maskObjects[i]
                .transform.GetChild(1).GetComponent<MeshRenderer>();
            quadRenderer.material = spriteMaterial;

            // Change names
            maskObjects[i].name = sprites[i].name;
            maskObjects[i].tag = "MaskObject";

            string name3D = sprites[i].name + "3D";
            string nameQuad = sprites[i].name + "Quad";
            mesh3D.name = name3D;
            meshQuad.name = nameQuad;
        }

        // Wrap MaskObjects in empty parent
        GameObject empty = new GameObject();
        empty.name = paintingName;
        empty.tag = "PaintingPrefab";
        foreach (GameObject g in maskObjects)
        {
            g.transform.parent = empty.transform;
        }

        // Add properly sized BlackoutQuad prefab
        GameObject newBlackoutQuad = 
            Instantiate(blackoutQuad, 
                        new Vector3(0, 0, extrusionDepth), 
                        Quaternion.identity) as GameObject;
        newBlackoutQuad.name = "BlackoutQuad";
        newBlackoutQuad.transform.parent = empty.transform;

        Bounds parentBounds = new Bounds(empty.transform.position, Vector3.one);
        for (int i = 0; i < maskObjects.Length; i++) {
            Mesh m = maskObjects[i].transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            Vector3 bounds = m.bounds.size;
            parentBounds.Encapsulate(bounds);
        }
        newBlackoutQuad.transform.localScale = parentBounds.size;
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
