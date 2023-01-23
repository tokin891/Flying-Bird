using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Managers
{
    public class vMenuManager : MonoBehaviour
    {
        public static vMenuManager _instance;

        [Header(": Objects")]
        [SerializeField] CanvasGroup _transparentMenu;
        [SerializeField] float _SmoothTransparent;
        [Space]
        [SerializeField] GameObject[] _selectLogo;
        [SerializeField] float _delayLogo = 8f;
        [SerializeField] float _smoothLogo = 6f;

        private void Awake()
        {
            _instance = this;
        }
        private void Update()
        {
            if(_transparentMenu != null
            && _transparentMenu.alpha > 0)
            {
                _transparentMenu.alpha = Mathf.Lerp(1, 0, _SmoothTransparent * Time.deltaTime);
            }
        }

        public void ShowLogo()
        {
            vGameManager._instance.ChangeStats(eStatsManager.Logo);

            StartCoroutine(_preapreLogo());
        }
        private IEnumerator _preapreLogo()
        {
            yield return new WaitForSeconds(4f);

            for (int i = 0; i < _selectLogo.Length; i++)
            {
                if(i == 0)
                {
                    _selectLogo[i].SetActive(true);
                    yield return new WaitForSeconds(_delayLogo);
                }else if(i != _selectLogo.Length)
                {
                    if (_selectLogo[i - 1].activeSelf != false)
                        _selectLogo[i - 1].SetActive(false);

                    _selectLogo[i].SetActive(true);
                    yield return new WaitForSeconds(_delayLogo);

                    if(i == _selectLogo.Length-1)
                    {
                        vGameManager._instance.ChangeStats(eStatsManager.Menu);
                    }
                }
            }
        }

        public void PlaySingle()
        {
            vGameManager._instance.ChangeStats(eStatsManager.Game);
        }
        public void PlayCoop2()
        {
            vGameManager._instance.ChangeStats(eStatsManager.Game, eTypeGame.Coop2);
        }
        public void QuitApp()
        {
            Application.Quit();
        }

        public void ChangeQuality(int quality)
        {
            QualitySettings.SetQualityLevel(quality);
        }
    }
}
