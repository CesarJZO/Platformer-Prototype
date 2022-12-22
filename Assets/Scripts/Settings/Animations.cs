using System;
using System.Collections.Generic;
using UnityEngine;

namespace Settings
{
    [CreateAssetMenu(fileName = "Animations", menuName = "Settings/Animations")]
    public class Animations : ScriptableObject
    {
        [SerializeField] private AnimationClip[] clips;

        private readonly Dictionary<string, AnimationClip> _clipsDictionary = new();

        private void OnValidate()
        {
            try
            {
                _clipsDictionary.Clear();
                foreach (var clip in clips)
                    _clipsDictionary.Add(clip.name, clip);
            }
            catch (UnassignedReferenceException) { }
        }

        public float GetLength(string animationName)
        {
            if (_clipsDictionary.ContainsKey(animationName))
                return _clipsDictionary[animationName].length;
            return -1;
        }
    }
}
