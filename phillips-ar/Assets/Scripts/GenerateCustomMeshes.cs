using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Script that applies respective sprite meshes to primitives with slight 
 * modification on the z-axis for thickness.
 * Expects instantiated MaskObject prefabs.
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
                Mesh mesh3D = ExtrudeMesh(Create2DMesh(sprites[i]), maskDepth);

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

    // Creates a 3D version of the 2D mesh
    private Mesh ExtrudeMesh(Mesh mesh, float extrusionFactor) {
        List<Vector3> extrudedVertices = new List<Vector3>();
        List<int> extrudedIndices = new List<int>();

        // Copy "frontface" edges of the mesh into list
        extrudedVertices.AddRange(mesh.vertices);

        // Create "backface" edges of the mesh into array by shifting z values of the vertices
        foreach(Vector3 v in mesh.vertices) {
            Vector3 new_vert = new Vector3(v.x, v.y, -extrusionFactor);
            extrudedVertices.Add(new_vert);
        }

        // Reassign indices for triangles of extruded mesh
        int origVertexCount = mesh.vertices.Length;
        for (int i = 0; i < origVertexCount; i ++) {
            // Creates a face relative to z-axis on edges connecting both faces
            int i1 = i;
            int i2 = (i1 + 1) % origVertexCount;
            int i3 = i1 + origVertexCount;
            int i4 = i2 + origVertexCount;

            // Create two triangles that make up the edge face
            extrudedIndices.Add(i1);
            extrudedIndices.Add(i2);
            extrudedIndices.Add(i3);

            extrudedIndices.Add(i3);
            extrudedIndices.Add(i4);
            extrudedIndices.Add(i1);
            Debug.Log("Iteration!");
        }

        // Assign new vertices and indices to mesh
        mesh.vertices = extrudedVertices.ToArray();
        mesh.triangles = extrudedIndices.ToArray();

        // Housekeeping
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        return mesh;
    }
}
