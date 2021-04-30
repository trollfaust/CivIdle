using UnityEngine;
using trollschmiede.CivIdle.GameEvents;
using trollschmiede.CivIdle.Resources;
using trollschmiede.CivIdle.Science;

namespace trollschmiede.CivIdle.Generic
{
    public class GameManager : MonoBehaviour
    {
        #region Singleton
        public static GameManager instance;
        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        #endregion

        [SerializeField] float mainLoopTimeBetweenTicks = 1f;

        SafeLoadManager safeLoadManager = SafeLoadManager.instance;
        ScienceManager scienceManager = ScienceManager.instance;
        ResourceManager resourceManager = ResourceManager.instance;
        GatheringManager gatheringManager = GatheringManager.instance;
        GameEventManager gameEventManager = GameEventManager.instance;
        CultureManager cultureManager = CultureManager.instance;
        PeopleManager peopleManager = PeopleManager.instance;

        #region Setup all the Stuff
        private void Start()
        {
            bool allIsSetup = SetupManagers();
            if (allIsSetup)
                StartMainLoop();
        }

        /// <summary>
        /// Setup all Managers in Order, returns ture if all Managers are Setup correctly
        /// </summary>
        /// <returns></returns>
        private bool SetupManagers()
        {
            bool saveLoadManagerIsSetup = safeLoadManager.Setup();
            CheckLogWarning(saveLoadManagerIsSetup, "Save/Load Manager Setup Error!");

            bool resourceManagerIsSetup = resourceManager.Setup();
            CheckLogWarning(resourceManagerIsSetup, "Resource Manager Setup Error!");

            bool peopleManagerIsSetup = peopleManager.Setup();
            CheckLogWarning(peopleManagerIsSetup, "People Manager Setup Error!");

            bool cultureManagerIsSetup = cultureManager.Setup();
            CheckLogWarning(cultureManagerIsSetup, "Culture Manager Setup Error!");

            bool scienceManagerIsSetup = scienceManager.Setup();
            CheckLogWarning(scienceManagerIsSetup, "Science Manager Setup Error!");

            bool gatheringManagerIsSetup = gatheringManager.Setup();
            CheckLogWarning(gatheringManagerIsSetup, "Gathering Manager Setup Error!");

            bool gameEventManagerIsSetup = gameEventManager.Setup();
            CheckLogWarning(gameEventManagerIsSetup, "GameEvent Manager Setup Error!");

            // Check if all is Setup
            if (saveLoadManagerIsSetup && scienceManagerIsSetup && resourceManagerIsSetup && gatheringManagerIsSetup && gameEventManagerIsSetup && cultureManagerIsSetup && peopleManagerIsSetup)
                return true;
            
            return false;
        }
        #endregion

        #region MainLoop
        float timeStamp;
        bool isLooping = false;

        /// <summary>
        /// Starts the Main Game Loop, Ticks every [mainLoopTimeBetweenTicks] Secounds
        /// </summary>
        private void StartMainLoop()
        {
            timeStamp = Time.time;
            isLooping = true;
        }

        /// <summary>
        /// Stops the Main Game Loop
        /// </summary>
        private void StopMainLoop()
        {
            isLooping = false;
        }

        private void Update()
        {
            if (isLooping == false)
                return;

            if (timeStamp + mainLoopTimeBetweenTicks < Time.time)
            {
                timeStamp = Time.time;

                gatheringManager.Tick();

                resourceManager.Tick();
                peopleManager.Tick();
                cultureManager.Tick();
                scienceManager.Tick();

                gameEventManager.Tick();

                safeLoadManager.Tick();
            }
        }
        #endregion

        #region Public Helper Functions
        /// <summary>
        /// Gives a Debug Warning with text if check is false
        /// </summary>
        /// <param name="check"></param>
        /// <param name="text"></param>
        public void CheckLogWarning(bool check, string text)
        {
            if (check == false)
            {
                Debug.LogWarning(text);
            }
        }
        /// <summary>
        /// Gives a Debug Warning with text
        /// </summary>
        /// <param name="text"></param>
        public void CheckLogWarning(string text)
        {
            Debug.LogWarning(text);
        }
        /// <summary>
        /// Gives a generic Debug Warning if check is false
        /// </summary>
        /// <param name="check"></param>
        public void CheckLogWarning(bool check)
        {
            if (check == false)
            {
                Debug.LogWarning("Generic Warning!");
            }
        }
        #endregion
    }
}
