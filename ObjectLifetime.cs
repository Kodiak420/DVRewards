using UnityEngine;

namespace DVRewards
{
    public class ObjectLifetime : MonoBehaviour
    {
        public void Start()
        {
            Destroy(gameObject, 3f);
        }
    }
}