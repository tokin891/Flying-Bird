using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Game.Managers
{
    public class vSoundManager : MonoBehaviour
    {
        public static vSoundManager _instance;
        [SerializeField] AudioMixer _mixer;

        private void Awake()
        {
            _instance = this;
        }

        public void SetupMainVolume(float volume)
        {
            _mixer.SetFloat("main", volume);
        }
        public void SetupSFXVolume(float volume)
        {
            _mixer.SetFloat("sfx", volume);
        }
    }
}
