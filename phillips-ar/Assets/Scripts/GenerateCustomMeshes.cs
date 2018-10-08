using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script that applies respective sprite meshes to primitives with slight 
 * modification on the z-axis for thickness.
 * Expects instantiated MaskObject prefabs.
 * 
 * Uses the ExtrudeMesh() function from MeshExtrusion.cs.
 */
public class GenerateCustomMeshes : MonoBehaviour {
    public float maskDepth = 1f;

    public Sprite[] sprites;
    public GameObject[] maskObjects;

    // Applies meshes to GameObjects from Sprites
    [ContextMenu("Apply Meshes")]
    public void ApplyMeshes() {
        // Check length of arrays
        if (sprites.Length != maskObjects.Length) {
            Debug.LogError("Arrays must be same length!");
        }
        else {
            // Applies every sprite mesh to its respective MaskObject 
            for (int i = 0; i < sprites.Length; i++)
            {
                MeshFilter meshFilter3D =
                    maskObjects[i].transform.GetChild(0).GetComponent<MeshFilter>();
                MeshFilter meshFilterQuad =
                    maskObjects[i].transform.GetChild(1).GetComponent<MeshFilter>();

                // Create extruded version of sprite mesh for 3D parent
                Mesh mesh3D = new Mesh();
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
                        new Vector4(0, 0, maskDepth, 1))
                    };

                MeshExtrusion.ExtrudeMesh(Create2DMesh(sprites[i]), mesh3D, matrix, false);

                // Create 2D mesh for quad
                Mesh meshQuad = Create2DMesh(sprites[i]);

                // Apply mesh to 3D parent and quad child
                meshFilter3D.mesh = mesh3D;
                meshFilterQuad.mesh = meshQuad;

                // Apply sprite material to sprite quad
                string filePath = "SpriteMaterials/" + sprites[i].name;
                Material spriteMaterial = Resources.Load(filePath, typeof(Material)) as Material;

                MeshRenderer quadRenderer = maskObjects[i]
                    .transform.GetChild(1).GetComponent<MeshRenderer>();
                quadRenderer.material = spriteMaterial;

                // Change name
                maskObjects[i].name = sprites[i].name;
            }
        }
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
