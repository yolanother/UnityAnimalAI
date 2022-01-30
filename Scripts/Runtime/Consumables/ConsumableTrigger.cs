using System;
using System.Collections;
using UnityEngine;

namespace DoubTech.AnimalAI.Consumables
{
    public class ConsumableTrigger : MonoBehaviour, IConsumable
    {
        [Header("Consumable Properties")]
        [SerializeField] private string consumableType;
        [SerializeField] private float quantity = 10;
        [SerializeField] private float consumptionRate = 1;
        
        [Header("Respawn")]
        [SerializeField] private bool respawn = true;
        [SerializeField] private float respawnSeconds = 60;

        [Header("Models")]
        [SerializeField] private GameObject[] consumableModels;

        private float startQuantity;
        private Coroutine respawnCoroutine;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(consumableType)) throw new Exception($"{name} must have a consumable type.");
        }

        public string ConsumableType => consumableType;
        public bool IsConsumed => quantity <= 0;

        private void Awake()
        {
            startQuantity = quantity;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!IsConsumed)
            {
                var consumer = other.GetComponent<IConsumer>();
                if (null != consumer && consumer.CanConsume(this))
                {
                    StartCoroutine(Consume());
                }
            }
        }

        private IEnumerator Respawn()
        {
            yield return new WaitForSeconds(respawnSeconds);
            quantity = startQuantity;
            respawnCoroutine = null;

            foreach (var model in consumableModels)
            {
                model.SetActive(true);
            }
        }

        private IEnumerator Consume()
        {
            while (!IsConsumed)
            {
                quantity -= consumptionRate * Time.deltaTime;
                yield return null;
            }

            foreach (var model in consumableModels)
            {
                model.SetActive(false);
            }

            if (respawn)
            {
                if (null != respawnCoroutine) StopCoroutine(respawnCoroutine);
                respawnCoroutine = StartCoroutine(Respawn());
            }
        }
    }
}
