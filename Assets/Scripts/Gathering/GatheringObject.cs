using UnityEngine;

namespace trollschmiede.CivIdle.Resources
{
    [CreateAssetMenu(fileName = "New Gathering Object", menuName = "Scriptable Objects/Gathering/Gathering Object")]
    public class GatheringObject : ScriptableObject
    {
        public string buttonName = "";
        [Tooltip("Values should be Negative for Substraction")]
        public ResourceChancePair[] craftingMaterials = new ResourceChancePair[0];
        public ResourceChancePair[] resourcesPairs = new ResourceChancePair[0];
        public float timeBetweenAutoGathering = 5f;
        public int peopleNeededToWork = 1;
        public bool isManuelGatherable = true;
        public float buttonCooldownTime = 5f;
    }
}