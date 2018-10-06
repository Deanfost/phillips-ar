using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AppManager : MonoBehaviour {
    private GameObject[] masks;

	void Start () {
        // Load the 3D mask models
        // Would normally be called after image target reco by AR Core
        masks = LoadBundle();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    // Return loaded assets from the bundle
    GameObject[] LoadBundle() {
        // Load the asset bundle
        AssetBundle bundle = 
            AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "luncheon"));

        // Collect the assets
        if (bundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return null;
        }
        return (UnityEngine.GameObject[])bundle.LoadAllAssets();
    }
}