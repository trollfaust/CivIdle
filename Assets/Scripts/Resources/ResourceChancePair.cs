﻿using UnityEngine;
namespace trollschmiede.CivIdle.ResourceSys
{
    [System.Serializable]
    public class ResourceChancePair
    {
        public Resource resource;
        public int minValue;
        public int maxValue;
        [Range(0,100)]
        public int chance;
    }
}
