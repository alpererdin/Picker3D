using UnityEngine;

namespace Runtime.Commands.Level
{
    public class OnLevelCommand
    {
        private Transform _levelHolder;
        private GameObject _loadedLevel;

        internal OnLevelCommand(Transform levelHolder)
        {
            _levelHolder = levelHolder;
        }

        internal void Execute(byte levelIndex)
        {
            if (_loadedLevel != null)
            {
                
                Undo();
            }

            
            _loadedLevel = Object.Instantiate(Resources.Load<GameObject>($"Prefabs/LevelPrefabs/level {levelIndex}"), _levelHolder, true);
        }

        internal void Undo()
        {
            if (_loadedLevel != null)
            {
                Object.Destroy(_loadedLevel);
                _loadedLevel = null;
            }
        }
    }
}