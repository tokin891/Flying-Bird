using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Tutorial
{
    public class Tutorial : MonoBehaviour
    {
        [SerializeField] int idTutorial;
        private CanvasGroup _canvas;
        public int useles = 0;

        private void Awake()
        {
            _canvas = GetComponent<CanvasGroup>();

            LoadTutorial();
        }
        public void SaveTutorial()
        {
            PlayerPrefs.SetInt("Tutorial" + idTutorial.ToString(), useles);
            PlayerPrefs.Save();
        }
        public void LoadTutorial()
        {
            if (!PlayerPrefs.HasKey("Tutorial" + idTutorial.ToString()))
                return;

            useles = PlayerPrefs.GetInt("Tutorial" + idTutorial.ToString());
        }

        public void ShowTutorial()
        {
            if (useles == 1)
                return;

            _canvas.alpha = 1;
            _canvas.interactable = true;
            _canvas.blocksRaycasts = true;
        }
        public void CloseTutorial()
        {
            if (_canvas.alpha == 0)
                return;

            _canvas.alpha = 0;
            useles = 1;
            _canvas.interactable = false;
            _canvas.blocksRaycasts = false;

            SaveTutorial();
        }
    }
}