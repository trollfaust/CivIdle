using UnityEngine;

namespace trollschmiede.CivIdle.ResourceSys
{
    public class CultureManager : MonoBehaviour
    {
        #region Singlton
        public static CultureManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] Resource cultureResource = null;
        [SerializeField] float timeBetweenCultureTicks = 1f;

        Resource[] cultureChangingResources;
        private float timeStamp;

        #region Setup
        bool isSetup = false;
        public bool Setup()
        {
            cultureChangingResources = ResourceManager.instance.GetCultureGeneratingResources();
            if (cultureChangingResources == null || cultureChangingResources.Length == 0)
            {
                return false;
            }

            timeStamp = Time.time;
            isSetup = true;
            return isSetup;
        }
        #endregion

        #region Update Tick
        public void Tick()
        {
            if (isSetup == false)
                return;

            if (timeStamp + timeBetweenCultureTicks <= Time.time)
            {
                timeStamp = Time.time;

                OnCultureTick();
            }
        }
        #endregion

        /// <summary>
        /// Uses one of each of the Culture Generating Resources to generate Culture
        /// </summary>
        void OnCultureTick()
        {
            foreach (Resource resource in cultureChangingResources)
            {
                if (resource.amount > 0)
                {
                    resource.AmountChange(-1);
                    cultureResource.AmountChange(resource.cultureValue);
                }
            }
        }
    }
}
