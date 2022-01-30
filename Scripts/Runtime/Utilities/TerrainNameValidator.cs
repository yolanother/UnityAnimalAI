using UnityEngine;

namespace DoubTech.AnimalAI.Utilities
{
    public class TerrainNameValidator : MonoBehaviour, ITerrainValidator
    {
        [SerializeField] public string validName;

        public bool IsValid => name == validName;
    }
    
    public interface ITerrainValidator
    {
        public bool IsValid { get; }
    }
}