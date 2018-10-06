using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script that applies respective sprite meshes to primitives with slight 
 * modification on the z-axis for thickness.
 * Expects primitives array to be populated with instantiated MaskObject prefabs.
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
                // Create extruded version of sprite mesh
                // Convert Vector2s of sprite to Vector3s, add to mesh
                Mesh mesh3D = new Mesh();
                mesh3D.vertices = System.Array.ConvertAll(sprites[i].vertices, v => (Vector3)v);

                // Copy "frontface" of the mesh into larger array
                Vector3[] extrudedVerts = new Vector3[mesh3D.vertices.Length * 2];
                int origLength = mesh3D.vertices.Length;
                for (int k = 0; k < mesh3D.vertices.Length; k ++) {
                    extrudedVerts[i] = mesh3D.vertices[i];
                }

                // Add "backface" of the mesh by shifting z values of front-facing vertices
                for (int k = 0; k < mesh3D.vertices.Length; k ++) {
                    float x = mesh3D.vertices[k].x;
                    float y = mesh3D.vertices[k].y;
                    float z = mesh3D.vertices[k].z * -maskDepth;
                    extrudedVerts[origLength + k] = new Vector3(x, y, z);
                }

                // 

                mesh3D.uv = sprites[i].uv;
                mesh3D.triangles = System.Array.ConvertAll(sprites[i].triangles, v => (int)v);


                // Create 2D mesh from sprite
                Mesh meshQuad = new Mesh {
                    vertices = System.Array.ConvertAll(sprites[i].vertices, v => (Vector3)v),
                    uv = sprites[i].uv,
                    triangles = System.Array.ConvertAll(sprites[i].triangles, v => (int)v)
                };

                // Apply mesh to 3D parent and quad child
                MeshFilter meshFilter3D =
                    maskObjects[i].transform.GetChild(0).GetComponent<MeshFilter>();
                MeshFilter meshFilterQuad = 
                    maskObjects[i].transform.GetChild(1).GetComponent<MeshFilter>();
                meshFilter3D.mesh = mesh3D;
                meshFilterQuad.mesh = meshQuad;

            }
        }
    }
}
