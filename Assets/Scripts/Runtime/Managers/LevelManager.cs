using Runtime.Commands.Level;
using Runtime.Data.UnityObjects;
using Runtime.Data.ValueObjects;
using Runtime.Enums;
using Runtime.Signals;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Managers
{
    public class LevelManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private Transform levelHolder;
        [SerializeField] private byte totalLevelCount;

        #endregion

        #region Private Variables
 
        private OnLevelCommand _onLevelCommand;

        private short _currentLevel;
        private LevelData _levelData;

        #endregion

        #endregion

        private void Awake()
        {
            _levelData = GetLevelData();
            _currentLevel = GetActiveLevel();

            Init();
        }

        private void Init()
        {
         
            _onLevelCommand = new OnLevelCommand(levelHolder);
        }

        private LevelData GetLevelData()
        {
            return Resources.Load<CD_Level>("Data/CD_Level").Levels[_currentLevel];
        }

        private byte GetActiveLevel()
        {
            return (byte)_currentLevel;
        }

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize += _onLevelCommand.Execute;
            CoreGameSignals.Instance.onClearActiveLevel += _onLevelCommand.Undo;
            CoreGameSignals.Instance.onGetLevelValue += OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel += OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel += OnRestartLevel;
        }

        [Button]
        private void OnNextLevel()
        { 
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            _currentLevel++;
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
            
        }

        [Button]
        private void OnRestartLevel()
        {
            CoreGameSignals.Instance.onClearActiveLevel?.Invoke();
            CoreGameSignals.Instance.onReset?.Invoke();
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0);
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
        }

        private byte OnGetLevelValue()
        {
            return (byte)((byte)_currentLevel % totalLevelCount);
        }

        private void UnSubscribeEvents()
        {
            CoreGameSignals.Instance.onLevelInitialize -= _onLevelCommand.Execute;
            CoreGameSignals.Instance.onClearActiveLevel -= _onLevelCommand.Undo;
            CoreGameSignals.Instance.onGetLevelValue -= OnGetLevelValue;
            CoreGameSignals.Instance.onNextLevel -= OnNextLevel;
            CoreGameSignals.Instance.onRestartLevel -= OnRestartLevel;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }

        private void Start()
        {
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Level, 0);
            CoreGameSignals.Instance.onLevelInitialize?.Invoke((byte)(_currentLevel % totalLevelCount));
            CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Start, 1);
            
        }
    }
}