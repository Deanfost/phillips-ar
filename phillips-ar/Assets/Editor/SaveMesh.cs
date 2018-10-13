using System.IO;
using UnityEngine;
using UnityEditor;

/* Script that collects all MaskObjects in the scene (tagged with "MaskObject")
 * and saves their 3D and Quad meshes into "Assets/Meshes" with a subfolder named
 * after the name of the MaskObject.
 * 
 * NOTE: This is required to ensure that prefabs containing procedurally generated
 * meshes save properly (i.e. every PaintingPrefab).
 */
public static class SaveMesh {
    [MenuItem("Assets/Save Mask Meshes")]
    public static void SaveMeshAsset()
    {
        // Collect references to all MaskObjects in the scene
        GameObject[] maskObjects = GameObject.FindGameObjectsWithTag("MaskObject");

        // Grab meshes, save to Assets
        foreach(GameObject g in maskObjects) {
            Mesh mesh3D = g.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh;
            Mesh meshQuad = g.transform.GetChild(1).GetComponent<MeshFilter>().sharedMesh;
            string objectName = g.name;
            string mesh3DName = mesh3D.name;
            string meshQuadName = meshQuad.name;

            // Make sure the required folder exists
            string folderPath = "Assets/Meshes/" + objectName;
            if(!Directory.Exists(folderPath)) {
                Directory.CreateDirectory(folderPath);
            }

            // Save each mesh as an asset
            string filePath3D = folderPath + "/" + mesh3DName + ".asset";
            string filePathQuad = folderPath + "/" + meshQuadName + ".asset";

            AssetDatabase.CreateAsset(mesh3D, filePath3D);
            AssetDatabase.CreateAsset(meshQuad, filePathQuad);
            AssetDatabase.SaveAssets();
        }
    }
}
