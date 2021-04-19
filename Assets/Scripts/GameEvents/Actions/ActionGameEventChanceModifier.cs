﻿using UnityEngine;

namespace trollschmiede.CivIdle.GameEvents
{
    [CreateAssetMenu(fileName = "New Action GameEvent Chance Modifier", menuName = "Scriptable Objects/Actions/Action GameEvent Chance Modifier")]
    public class ActionGameEventChanceModifier : Action
    {
        [SerializeField] GameEvent gameEvent = null;
        [SerializeField] [Range(0f,5f)] float multiplier = 1f;

        public override int EvokeAction()
        {
            gameEvent.AddChanceMultiplier(multiplier);
            return 0;
        }
    }
}
