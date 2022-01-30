using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DoubTech.AnimalAI.Utilities
{
    public class EnableRandomChild : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            var enabledIndex = Random.Range(0, transform.childCount);
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(enabledIndex == i);
            }
        }
    }
}
