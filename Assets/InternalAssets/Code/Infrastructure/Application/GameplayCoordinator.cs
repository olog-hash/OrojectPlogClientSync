using ProjectOlog.Code.Infrastructure.TimeManagement;
using ProjectOlog.Code.Infrastructure.TimeManagement.Interfaces;
using ProjectOlog.Code.Input.Controls;
using UnityEngine;

namespace ProjectOlog.Code.Infrastructure.Application
{
    public class GameplayCoordinator : IUpdate
    {
        public static GameStateType CurrentState => _currentGameState;

        private static GameStateType _currentGameState;
        private static GameStateType _previousGameState;

        public GameplayCoordinator(RuntimeHelper runtimeHelper)
        {
            //runtimeHelper.RegisterUpdate(this);

            //_previousGameState = GameStateType.Menu;
            
            //ChangeState(GameStateType.Menu);
        }

        public static void RollbackState()
        {
            ChangeState(_previousGameState);
        }
        
        public static void ChangeState(GameStateType newState)
        {
            ExitCurrentState();

            _previousGameState = _currentGameState;
            _currentGameState = newState;

            EnterNewState();
        }

        private static void ExitCurrentState()
        {
            switch (_currentGameState)
            {
                default: 
                    break;
            }
        }

        private static void UpdateCurrentState()
        {
            switch (_currentGameState)
            {
                case GameStateType.None:
                    break;
                case GameStateType.Menu:
                case GameStateType.Chat:
                case GameStateType.SelectPanel:
                    break;
                case GameStateType.Gameplay:
                    if (InputControls.GetMouseButtonDown(0) || InputControls.GetMouseButtonDown(1))
                    {
                        Cursor.lockState = CursorLockMode.Locked;
                    }
                    if (UnityEngine.Input.GetKeyDown(KeyCode.Escape))
                    {
                        Cursor.lockState = CursorLockMode.None;
                    }
                    break;
                default:
                    break;
            }
        }

        private static void EnterNewState()
        {
            switch (_currentGameState)
            {
                case GameStateType.None:
                    InputControls.IsMouseControlEnabled = false;
                    InputControls.IsKeyControlEnabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case GameStateType.Menu:
                case GameStateType.Chat:
                    InputControls.IsMouseControlEnabled = true;
                    InputControls.IsKeyControlEnabled = false;
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case GameStateType.SelectPanel:
                    InputControls.IsMouseControlEnabled = true;
                    InputControls.IsKeyControlEnabled = true;
                    Cursor.lockState = CursorLockMode.None;
                    break;
                case GameStateType.Gameplay:
                    InputControls.IsMouseControlEnabled = true;
                    InputControls.IsKeyControlEnabled = true;
                    Cursor.lockState = CursorLockMode.Locked;
                    break;
                default:
                    break;
            }
        }

        public void OnUpdate(float deltaTime)
        {
            UpdateCurrentState();
        }
    }

    public enum GameStateType
    {
        None,
        Menu,
        Gameplay,
        
        Chat,
        SelectPanel,
    }
}