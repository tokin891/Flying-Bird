using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Managers
{
    using Player.Movement;
    using TMPro;
    using Tutorial;
    using Shop.Item;
    using Player.Coop;

    public class vGameManager : MonoBehaviour
    {
        public static vGameManager _instance;

        [Header("Stats")]
        public vWindowManagers _window;
        public vBirdKeyCode _codes;
        public eStatsManager _statsManager = new eStatsManager();
        public eCurrentUsePower _currentPower = new eCurrentUsePower();
        public eTypeGame _typeGame = new eTypeGame();[Space]

        [Tooltip("The same count as 'scoreSpeedUp'")]
        public float[] allSpeeds;
        [Tooltip("The same count as 'scoreSpeedUp'")]
        public float[] allDelayRespawn;
        [Tooltip("Setup border score level up")]
        public int[] scoreSpeedUp;

        private float _percentagesWallSpeed = 1f;
        private int currentScoreLevel = 0;[Space]

        [Header("Events")]
        [SerializeField] UnityEvent _onLose;
        [SerializeField] UnityEvent _onAddCoin;
        [SerializeField] BirdMovement _mainBird;

        [Space]
        [Header("Objects")]
        [SerializeField]
        eStatsManager _selectStartWindow;
        [SerializeField]
        Trap.SpawnBarrier _spawnBarrier;
        [SerializeField]
        TMP_Text _scoreText;
        [SerializeField]
        TMP_Text _timeText;
        [SerializeField]
        TMP_Text _coinText;
        [SerializeField]
        TMP_Text _scoreTextbest;
        [SerializeField]
        TMP_Text _timeTextbest;

        [Space]
        [Header("CO-OP")]
        [SerializeField] Transform[] PointsRespawn;
        [SerializeField] GameObject _birdPrefab;
        private PlayerState[] _allPState = new PlayerState[] { _playerOne , _playerTwo};
        private List<BirdMovement> allBird = new List<BirdMovement>();
        [SerializeField]
        TMP_Text _scoreTextlast;
        [SerializeField]
        TMP_Text _timeTextlast;   

        private int mScore;
        private int mTime;
        private int mCoin;
        private int lastScore;
        private int lastTime;

        protected const string _mScore = "mScore";
        protected const string _mTime = "mTime";
        protected const string _mCoin = "mCoin";

        private static PlayerOne _playerOne = new PlayerOne();
        private static PlayerTwo _playerTwo = new PlayerTwo();

        private void Awake()
        {
            _instance = this;
            ChangeStats(_selectStartWindow);
            if(_selectStartWindow == eStatsManager.Logo)
                vMenuManager._instance.ShowLogo();

            _mainBird = FindObjectOfType<BirdMovement>();
            currentScoreLevel = 0;

            _playerOne.indexPlayer = 0;
            _playerTwo.indexPlayer = 1;
        }
        private void Update()
        {
            if(_scoreText != null)
            {
                _scoreText.text = "Score: " + mScore.ToString();
                _timeText.text = "Time: " + mTime.ToString();
            }
            if(_scoreTextbest != null)
            {
                _scoreTextbest.text = "Your best core: " + PlayerPrefs.GetInt(_mScore);
                _timeTextbest.text = "Your best time: " + PlayerPrefs.GetInt(_mTime);
            }
            if(_coinText != null)
            {
                _coinText.text = "Coin: " + PlayerPrefs.GetInt(_mCoin);
            }

            if(mScore > scoreSpeedUp[currentScoreLevel])
            {
                if(currentScoreLevel + 1 < scoreSpeedUp.Length)
                {
                    currentScoreLevel++;
                    Debug.Log("Speed Up To " + currentScoreLevel);
                }
            }

            if(_statsManager == eStatsManager.Game)
            {
                RenderSettings.skybox.SetFloat("_Rotation", Time.time * 2f);
            }

            // Only Test Strefe
            if (Input.GetKeyDown(KeyCode.K))
                SpeedUpPercentagesWall(2.5f, 3f, null);
            if (Input.GetKeyDown(KeyCode.L))
                SpeedUpPercentagesWall(0.55f, 3f, null);
        }

        public void ChangeStats(eStatsManager _stats, eTypeGame _type = eTypeGame.Single)
        {
            _statsManager = _stats;

            switch (_stats)
            {
                case eStatsManager.Logo:
                    _window.OpenWindow(_window._logo);

                    break;
                case eStatsManager.Menu:
                    _window.OpenWindow(_window._menu);

                    break;
                case eStatsManager.Game:
                    _window.OpenWindow(_window._game);

                    PrepareGame(_type);
                    break;
                case eStatsManager.Dead:
                    _window.OpenWindow(_window._dead);

                    break;      
            }
        }

        public void PrepareGame(eTypeGame type)
        {
            _typeGame = type;
            if(type == eTypeGame.Single)
            {
                _mainBird.SetupPlayer(_allPState[0]);
                _mainBird.SwitchState(Player.EnumsBird.eBirdState.Fly);

                currentScoreLevel = 0;
                InvokeRepeating(nameof(AddTime), 1, 1);
                ClearScore();
                LauncherTutorial._instance.OpenTutorialWithDelay(0, 0.5f);
                LauncherTutorial._instance.OpenTutorialWithDelay(1, 3f);
            }
            if (type == eTypeGame.Coop2)
            {
                _mainBird.gameObject.SetActive(false);

                //for (int i = 0; i < 2; i++)
                //{
                //    BirdMovement objectBird = Instantiate(_birdPrefab, PointsRespawn[i].position, Quaternion.identity).GetComponentInChildren<BirdMovement>();
                //    objectBird.SetupPlayer(_allPState[i]);
                //    objectBird.SwitchState(Player.EnumsBird.eBirdState.Fly);
                //    allBird.Add(objectBird);
                //}

                currentScoreLevel = 0;
                InvokeRepeating(nameof(AddTime), 1, 1);
                ClearScore();
                LauncherTutorial._instance.OpenTutorialWithDelay(0, 0.15f);
                LauncherTutorial._instance.OpenTutorialWithDelay(1, 2f);

                BirdMovement objectBird = Instantiate(_birdPrefab, PointsRespawn[0].position, Quaternion.identity).GetComponentInChildren<BirdMovement>();
                objectBird.SetupPlayer(_allPState[0]);
                objectBird.SwitchState(Player.EnumsBird.eBirdState.Fly);
                allBird.Add(objectBird);

                BirdMovement objectBird2 = Instantiate(_birdPrefab, PointsRespawn[1].position, Quaternion.identity).GetComponentInChildren<BirdMovement>();
                objectBird2.SetupPlayer(_allPState[1]);
                objectBird2.SwitchState(Player.EnumsBird.eBirdState.Fly);
                allBird.Add(objectBird2);
            }
        }
        public void LoseGame()
        {
            switch (_typeGame)
            {
                case eTypeGame.Single: // In Single Mode
                    ChangeStats(eStatsManager.Dead);
                    _mainBird.SwitchState(Player.EnumsBird.eBirdState.Dead);

                    lastScore = mScore;
                    lastTime = mTime;
                    SaveData();
                    CancelInvoke(nameof(AddTime));

                    _scoreTextlast.text = "Your Score: " + lastScore.ToString();
                    _timeTextlast.text = "Your Time: " + lastTime.ToString();
                    _onLose?.Invoke();

                    break;
                case eTypeGame.Coop2: // In Coop2 Mode
                    ChangeStats(eStatsManager.Dead);
                    for (int i = 0; i < allBird.Count; i++)
                    {
                        allBird[i].SwitchState(Player.EnumsBird.eBirdState.Dead);
                    }

                    lastScore = mScore;
                    lastTime = mTime;

                    _scoreTextlast.text = "Yours Score: " + lastScore.ToString() + "/ 2 Players";
                    _timeTextlast.text = "Yours Time: " + lastTime.ToString();
                    CancelInvoke(nameof(AddTime));

                    _onLose?.Invoke();
                    break;
            }
        }
        public void RestartGame()
        {
            _mainBird.gameObject.SetActive(true);

            Trap.SpawnBarrier[] allBarrierSpawn = FindObjectsOfType<Trap.SpawnBarrier>();
            for (int i = 0; i < allBarrierSpawn.Length; i++)
            {
                allBarrierSpawn[i].DestroyAllBarrier();
            }
            if(FindObjectOfType<vBirdCoop>() != null)
            {
                vBirdCoop[] allB = FindObjectsOfType<vBirdCoop>();
                for (int i = 0; i < allB.Length; i++)
                {
                    Destroy(allB[i].gameObject);
                }
            }

            ChangeStats(eStatsManager.Menu);
            _mainBird.SwitchState(Player.EnumsBird.eBirdState.Idle);
            _mainBird.transform.position = new Vector3(-6.74f, 0f, 0f);
            currentScoreLevel = 0;
        }

        #region Save And Load
        private void LoadData()
        {
            if (!PlayerPrefs.HasKey(_mScore) && !PlayerPrefs.HasKey(_mTime))
                return;

            mScore = PlayerPrefs.GetInt(_mScore);
            mTime = PlayerPrefs.GetInt(_mTime);
            mCoin = PlayerPrefs.GetInt(_mCoin);
        }
        private void SaveData()
        {
            if(mScore > PlayerPrefs.GetInt(_mScore))
            {
                PlayerPrefs.SetInt(_mScore, mScore);
            }
            if(mTime > PlayerPrefs.GetInt(_mTime))
            {
                PlayerPrefs.SetInt(_mTime, mTime);
            }
            PlayerPrefs.SetInt(_mCoin, mCoin);

            PlayerPrefs.Save();
        }
        private void ClearScore()
        {
            mTime = 0;
            mScore = 0;
        }

        public void ClearData()
        {
            PlayerPrefs.DeleteAll();
            mCoin = 0;
        }
        #endregion

        public void AddScore()
        {
            mScore++;
        }
        public void AddTime()
        {
            mTime++;
        }
        public void AddCoin()
        {
            mCoin++;
            _onAddCoin?.Invoke();
        }

        public float GetSpeedWall()
        {
            return allSpeeds[currentScoreLevel] * _percentagesWallSpeed;
        }
        public float GetPercentagesWall()
        { return _percentagesWallSpeed; }
        public float GetDelaySpawn()
        {
            return allDelayRespawn[currentScoreLevel];
        }

        #region Powers
        public void UsePower(Item _item)
        {
            switch (_item._objectPower)
            {
                case TypePower.SlowTime10sec:
                    SpeedUpPercentagesWall(0.65f, 10f, _item);
                    break;
                case TypePower.DobuleLife:

                    break;
                case TypePower.TripleLife:

                    break;
                case TypePower.SpeedUp10sec:
                    SpeedUpPercentagesWall(1.35f, 10f, _item);
                    break;
            }
        }
        private void SpeedUpPercentagesWall(float percentages, float delay, Item _index)
        {
            if (_currentPower == eCurrentUsePower.Something)
                return;

            // Do SpeedUp with delay
            StartCoroutine(_speedUpDelay(percentages, delay));

            // RemoveIndex

        }

        private IEnumerator _speedUpDelay(float percentages, float delay)
        {
            _percentagesWallSpeed = percentages;
            _currentPower = eCurrentUsePower.Something;
            Debug.Log(percentages);

            yield return new WaitForSeconds(delay);

            _percentagesWallSpeed = 1f;
            _currentPower = eCurrentUsePower.Nothing;
            Debug.Log("Stop Using Power");
        }
        #endregion
    }

    #region Enums & Class
    [System.Serializable]
    public class vWindowManagers
    {
        public GameObject _logo;
        public GameObject _menu;
        public GameObject _game;
        public GameObject _dead;

        public void OpenWindow(GameObject objectWindow)
        {
            _logo.SetActive(false);
            _menu.SetActive(false);
            _game.SetActive(false);
            _dead.SetActive(false);

            GameObject[] _lists = new GameObject[] { _logo, _menu, _game, _dead };
            for (int i = 0; i < _lists.Length; i++)
            {
                if (_lists[i].name == objectWindow.name)
                    _lists[i].SetActive(true);
            }
        }
    }
    [System.Serializable]
    public class vBirdKeyCode
    {
        [Header("KeyCode For Players")]
        [Tooltip("Key Code For 3 Player. Fill index 0,1,2 with some keycode," +
            "Index0 - Player01, Index1 - Player02, Index2 - Player03")]

        public KeyCode[] _jumps;
        public KeyCode[] _usePower;
        public Material[] _materialsPerPlayer;

        public KeyCode GetPlayerCode(KeyCode[] selectCode, int PlayerState)
        {
            if (PlayerState < 3)
                return selectCode[PlayerState];

            return KeyCode.None;
        }
        public Material GetMaterial(int PlayerState)
        {
            if (PlayerState < 3)
                return _materialsPerPlayer[PlayerState];

            return null;
        }
    }
    public enum eStatsManager
    { 
        Logo,
        Menu,
        Game,
        Dead
    }
    public enum eCurrentUsePower
    {
        Nothing,
        Something
    }
    public enum eTypeGame
    {
        Single,
        Coop2
    }
    #endregion
}
