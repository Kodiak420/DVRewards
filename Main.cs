using System.Reflection;
using UnityEngine;
using UnityModManagerNet;
using HarmonyLib;
using CommandTerminal;
using DV.ServicePenalty;

namespace DVRewards
{
    public class Main
    {
        public static GameObject StephenAssetContainer;
        public static StephenAssetBundles StephenAssetBundle;
        public static UnityModManager.ModEntry localModEntry;
        public static int playerCash = 0;
        public static bool showUI = false;
        public static StationController[] stations;

        private static bool gameInitialized = false;

        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            modEntry.OnUpdate = OnUpdate;
            localModEntry = modEntry;
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            return true;
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float value)
        {
            if (StephenAssetContainer == null)
            {
                StephenAssetContainer = new GameObject("Diamond");
                StephenAssetBundle = StephenAssetContainer.AddComponent<StephenAssetBundles>();
                StephenAssetBundle.LoadAssets(localModEntry.Path + "diamondbundle");
            }

            if (!gameInitialized)
            {
                if (!LoadingScreenManager.IsLoading || WorldStreamingInit.IsLoaded || InventoryStartingItems.itemsLoaded && SaveLoadController.carsAndJobsLoadingFinished)
                {
                    StephenAssetBundle.GrabAllStations();
                    gameInitialized = true;
                }
            }

            if (showUI)
                StephenAssetBundle.StartCoroutine(StephenAssetBundles.HideRewardUI());
        }        

        public static void GiveRandomReward()
        {
            AudioClip clip = StephenAssetBundle.InstantiateAudioFromBundle("DiamondAudio", false);
            if (clip != null)
            {
                GameObject _audio = new GameObject();
                AudioSource audio = _audio.AddComponent<AudioSource>();
                _audio.AddComponent<ObjectLifetime>();
                audio.playOnAwake = false;
                audio.PlayOneShot(clip);
            }

            int r = Random.Range(0, 101);
            if(r > 80)
            {
                if (JobDebtController.existingJoblessCarDebts.NumberOfDebts > 0)
                {
                    JobDebtController.existingJoblessCarDebts.Pay();
                    Terminal.Log($"Diamond collected. Removed a jobless car debt worth ${JobDebtController.existingJoblessCarDebts.GetTotalPrice()}.");
                }
                else
                {
                    playerCash = Random.Range(500, 5001);
                    SingletonBehaviour<Inventory>.Instance.AddMoney(playerCash);
                    Terminal.Log($"Diamond collected. Added ${playerCash} to player wallet.");
                }
            }
            else if (r > 60 && r <= 80)
            {
                if (JobDebtController.stagedJobsDebts.Count > 0)
                {
                    JobDebtController.stagedJobsDebts[Random.Range(0, JobDebtController.stagedJobsDebts.Count - 1)].Pay();
                    Terminal.Log($"Diamond collected. Removed a staged job debt worth ${JobDebtController.stagedJobsDebts[Random.Range(0, JobDebtController.stagedJobsDebts.Count - 1)].GetTotalPrice()}.");
                }
                else
                {
                    playerCash = Random.Range(500, 5001);
                    SingletonBehaviour<Inventory>.Instance.AddMoney(playerCash);
                    Terminal.Log($"Diamond collected. Added ${playerCash} to player wallet.");
                }
            }
            else if (r > 40 && r <= 60)
            {
                if (JobDebtController.existingTrackedJobs.Count > 0)
                {
                    JobDebtController.existingTrackedJobs[Random.Range(0, JobDebtController.existingTrackedJobs.Count - 1)].Pay();
                    Terminal.Log($"Diamond collected. Removed a tracked job debt worth ${JobDebtController.existingTrackedJobs[Random.Range(0, JobDebtController.existingTrackedJobs.Count - 1)].GetTotalPrice()}.");
                }
                else
                {
                    playerCash = Random.Range(500, 5001);
                    SingletonBehaviour<Inventory>.Instance.AddMoney(playerCash);
                    Terminal.Log($"Diamond collected. Added ${playerCash} to player wallet.");
                }
            }
            else
            {
                playerCash = Random.Range(500, 5001);
                SingletonBehaviour<Inventory>.Instance.AddMoney(playerCash);
                Terminal.Log($"Diamond collected. Added ${playerCash} to player wallet.");
            }
        }
    }
}
