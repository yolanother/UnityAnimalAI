using System;
using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;

namespace DoubTech.AnimalAI.Consumables
{
    public class Consumer : MonoBehaviour, IConsumer
    {
        [SerializeField] private string[] consumableTypes;

        private HashSet<string> consumableTypeSet = new HashSet<string>();

        private void Awake()
        {
            consumableTypeSet.AddRange(consumableTypes);
        }

        public bool CanConsume(IConsumable consumable)
        {
            return consumableTypeSet.Contains(consumable.ConsumableType);
        }
    }
}