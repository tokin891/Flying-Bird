using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tutorial
{
    public class LauncherTutorial : MonoBehaviour
    {
        public static LauncherTutorial _instance;
        public GameObject[] _tutorial;

        private void Awake()
        {
            _instance = this;
        }
        private void Update()
        {
            if (isTutorialOpen())
            {
                if(Time.timeScale == 1)
                {
                    Time.timeScale = 0.012f;
                }
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    CloseTutorial();
                }
            }
            if (!isTutorialOpen())
                Time.timeScale = 1f;
        }

        public void OpenTutorial(int index)
        {
            if (_tutorial[index] != null)
            {
                _tutorial[index].GetComponent<Tutorial>().ShowTutorial();
            }
        }
        public void OpenTutorialWithDelay(int index, float delay)
        {
            StartCoroutine(_OTWD(index, delay));
        }    
        public void CloseTutorial()
        {
            Time.timeScale = 1f;
            for (int i = 0; i < _tutorial.Length; i++)
            {
                _tutorial[i].GetComponent<Tutorial>().CloseTutorial();
            }
        }

        private IEnumerator _OTWD(int index, float delay)
        {
            yield return new WaitForSeconds(delay);
            OpenTutorial(index);
        }

        private bool isTutorialOpen()
        {
            for (int i = 0; i < _tutorial.Length; i++)
            {
                if (_tutorial[i].GetComponent<CanvasGroup>().alpha == 1)
                    return true;
            }

            return false;
        }
    }
}
