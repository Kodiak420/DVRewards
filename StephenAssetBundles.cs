using CommandTerminal;
using System.Collections;
using UnityEngine;

namespace DVRewards
{
    public class StephenAssetBundles : MonoBehaviour
    {
        public AssetBundle loadedAssetBundle;

        public void LoadAssets(string filePath)
        {
            AssetBundle.UnloadAllAssetBundles(true);

            Terminal.Log("DVRewards: Attempting to load asset bundle...");
            loadedAssetBundle = AssetBundle.LoadFromFile(filePath);

            if (loadedAssetBundle != null) { Terminal.Log($"DVRewards: Loaded asset bundle. {filePath}"); }
            else { Terminal.Log($"DVRewards: Loaded asset bundle is null! {filePath}"); }
        }
        
        public GameObject InstantiateAssetFromBundle(string assetName, string gameObjectName, bool outputLog)
        {
            if (loadedAssetBundle != null)
            {
                GameObject prefab = loadedAssetBundle.LoadAsset(assetName) as GameObject;
                prefab.name = gameObjectName;
                Instantiate(prefab);

                if (outputLog) { Terminal.Log($"DVRewards: Loaded {assetName} from asset bundle."); }
                return prefab;
            }
            else
            {
                Terminal.Log($"DVRewards: {assetName} could not be loaded from the asset bundle as the loaded asset bundle is null!");
                return null;
            }
        }

        public AudioClip InstantiateAudioFromBundle(string assetName, bool outputLog)
        {
            if (loadedAssetBundle != null)
            {
                AudioClip clip = loadedAssetBundle.LoadAsset(assetName) as AudioClip;

                if (outputLog) { Terminal.Log($"DVRewards: Loaded {assetName} from asset bundle."); }
                return clip;
            }
            else
            {
                Terminal.Log($"DVRewards: {assetName} could not be loaded from the asset bundle as the loaded asset bundle is null!");
                return null;
            }
        }

        public static IEnumerator HideRewardUI()
        {
            if (!Main.showUI) { yield return null; }

            yield return new WaitForSeconds(3f);
            Main.showUI = false;
        }

        public void GrabAllStations()
        {
            Terminal.Log($"DVRewards: Grabbing all stations.");

            Main.stations = FindObjectsOfType<StationController>();
            for (int i = 0; i < Main.stations.Length; i++)
            {
                Main.stations[i].logicStation.JobAddedToStation += ()=>
                {
                    Invoke("DoDiamonds", 15f);
                };
            }
        }

        public void DoDiamonds()
        {
            CreateDiamondPickups(1);
        }

        public void CreateDiamondPickups(int count)
        {   
            for (int i = 0; i < count; i++)
            {
                int r = Random.Range(0,5);
                if(r >= 3)
                {
                    float randX = Random.Range(PlayerManager.PlayerTransform.position.x - 500f, PlayerManager.PlayerTransform.position.x + 500f);
                    float randZ = Random.Range(PlayerManager.PlayerTransform.position.z - 500f, PlayerManager.PlayerTransform.position.z + 500f);
                    float yVal = PlayerManager.PlayerTransform.position.y + 500f;

                    GameObject diamond = Main.StephenAssetBundle.InstantiateAssetFromBundle("Diamond", "Diamond", false);
                    diamond.AddComponent<DiamondScript>();

                    RaycastHit hit;
                    if (Physics.Raycast(new Vector3(randX, yVal, randZ), Vector3.down, out hit, Mathf.Infinity))
                        yVal = hit.point.y + 0.75f;

                    Vector3 finalPos = new Vector3(randX, yVal, randZ);
                    diamond.transform.position = finalPos;
                    WorldMover.Instance.AddObjectToMove(diamond.transform);

                    Terminal.Log($"DVRewards: Spawned a diamond at {finalPos}.");
                }
            }
        }
    }
}
