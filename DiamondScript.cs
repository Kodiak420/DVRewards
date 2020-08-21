using UnityEngine;

namespace DVRewards
{
    public class DiamondScript : MonoBehaviour
    {
        private bool hasCollected = false;

        private void Start()
        {
            Destroy(gameObject, 1000);
        }

        private void Update()
        {
            CheckForCollected();
        }

        private void CheckForCollected()
        {
            if (PlayerManager.PlayerTransform == null || hasCollected) { return; }

            if (Vector3.Distance(PlayerManager.PlayerTransform.position, transform.position) < 2f)
            {
                hasCollected = true;

                Main.GiveRandomReward();
                Main.showUI = true;
                
                Destroy(gameObject);
            }
        }
    }
}
