using UnityEngine;

namespace trollschmiede.CivIdle.Resources
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
        [SerializeField] Resource[] cultureChangingResources = new Resource[0];
        [SerializeField] float timeBetweenCultureTicks = 0f;

        private float timeStamp;

        private void Start()
        {
            timeStamp = Time.time;
        }

        private void Update()
        {
            if (timeStamp + timeBetweenCultureTicks <= Time.time)
            {
                timeStamp = Time.time;

                OnCultureTick();
            }
        }


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
