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
 * 
 * NOTE: Since operations in SaveMesh() must be carried out synchronously, depending
 * on the amount of sprite/meshes being fed into the script, the editor may hang  
 * for a period of time.
 * 
 * Expects references of instantiated MaskObject prefabs.
 */
public class GeneratePaintingPrefab : MonoBehaviour {
    public string paintingName;
    public float extrusionDepth = 1f;

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

                string name3D = sprites[i].name + "3D";
                string nameQuad = sprites[i].name + "Quad";
                mesh3D.name = name3D;
                mesh3D.name = nameQuad;

                // Save generated meshes as mesh assets
                SaveMesh(mesh3D, sprites[i].name, name3D);
                SaveMesh(meshQuad, sprites[i].name, nameQuad);

                // Wrap MaskObjects in empty parent


                // Add properly sized BlackoutQuad prefab


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

    // Save the given mesh as an asset in a folder named "Meshes", with the
    // given subfolder name.
    private void SaveMesh(Mesh mesh, string folderName, string fileName) {
        string folderPath = Application.dataPath + "/Meshes" + "/" + folderName;

        // Make sure the required folder exists, if not, create it
        Directory.CreateDirectory(folderPath);

        // Save the mesh as an asset
        string filePath = folderPath + "/" + fileName;
        filePath = FileUtil.GetProjectRelativePath(filePath);

        AssetDatabase.CreateAsset(mesh, filePath);
        AssetDatabase.SaveAssets();
    }
}
