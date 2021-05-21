using System.Collections.Generic;
using UnityEngine;

namespace trollschmiede.CivIdle.Util
{
    public enum KeyCodeNames { pauseKey, playKey, fastKey, lockKey, unlockKey }

    [CreateAssetMenu(fileName = "New Key Settings", menuName = "Settings/Key Settings")]
    public class KeySettings : ScriptableObject
    {
        [Header("Time Control Keys")]
        public KeyCode pauseKey = KeyCode.B;
        public KeyCode playKey = KeyCode.N;
        public KeyCode fastKey = KeyCode.M;

        [Header("Tooltip Control Keys")]
        public KeyCode lockKey = KeyCode.Mouse1;
        public KeyCode unlockKey = KeyCode.Mouse0;

        public Dictionary<KeyCodeNames, KeyCode> keyPairs;

        public Dictionary<KeyCodeNames, KeyCode> GetKeys()
        {
            keyPairs = new Dictionary<KeyCodeNames, KeyCode>();
            keyPairs.Add(KeyCodeNames.pauseKey, pauseKey);
            keyPairs.Add(KeyCodeNames.playKey, playKey);
            keyPairs.Add(KeyCodeNames.fastKey, fastKey);
            keyPairs.Add(KeyCodeNames.lockKey, lockKey);
            keyPairs.Add(KeyCodeNames.unlockKey, unlockKey);

            return keyPairs;
        }

        public void ChangeKey(KeyValuePair<KeyCodeNames, KeyCode> pair)
        {
            switch (pair.Key)
            {
                case KeyCodeNames.pauseKey:
                    pauseKey = pair.Value;
                    break;
                case KeyCodeNames.playKey:
                    playKey = pair.Value;
                    break;
                case KeyCodeNames.fastKey:
                    fastKey = pair.Value;
                    break;
                case KeyCodeNames.lockKey:
                    lockKey = pair.Value;
                    break;
                case KeyCodeNames.unlockKey:
                    unlockKey = pair.Value;
                    break;
                default:
                    break;
            }
        }
    }
}
