using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeLoadManager : MonoBehaviour
{
    #region Singleton
    public static SafeLoadManager instance;
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

    #region Setup
    bool isSetup = false;
    public bool Setup()
    {
        //TODO: Save/Load Stuff on Start Game
        isSetup = true;
        return isSetup;
    }
    #endregion

    #region Update Tick
    public void Tick()
    {
        //TODO: Save/Load Stuff on every Tick
    }
    #endregion
}
