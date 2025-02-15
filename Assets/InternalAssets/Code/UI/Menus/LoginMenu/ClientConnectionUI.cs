using ProjectOlog.Code._InDevs.Data.Sessions;
using ProjectOlog.Code.Infrastructure.Application.StateMachine;
using ProjectOlog.Code.Infrastructure.Application.StateMachine.States;
using ProjectOlog.Code.Networking.Client;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace ProjectOlog.Code.UI.Menus.LoginMenu
{
    public class ClientConnectionUI : MonoBehaviour
    {
        [SerializeField]
        private InputField _usernameIF;

        [SerializeField]
        private Text _disconnectInfoField;

        private GameObject _uiObject;
        private NetworkClient _clientLogic;
        private ApplicationStateMachine _applicationStateMachine;

        [Inject]
        public void Construct(NetworkClient clientLogic, ApplicationStateMachine applicationStateMachine)
        {
            _uiObject = gameObject;
            _clientLogic = clientLogic;
            _applicationStateMachine = applicationStateMachine;

            Initialize();
        }

        private void Initialize()
        {
            _clientLogic.OnConnected += OnConnected;
            _clientLogic.OnDisconnected += OnDisconnected;
        }

        public void OnConnect()
        {
            if (_usernameIF != null && !string.IsNullOrWhiteSpace(_usernameIF.text))
            {
                LocalData.LocalName = _usernameIF.text;

                _clientLogic.ConnectToServer("localhost"); // 85.159.231.90 // localhost
                _uiObject.SetActive(false);
            }
        }

        private void OnConnected()
        {
            _applicationStateMachine.Enter<BattleLevelState>();
        }

        private void OnDisconnected(string info)
        {
            _uiObject.SetActive(true);
            _disconnectInfoField.text = info.ToString();
        }

        public void OnDestroy()
        {
            _clientLogic.OnConnected -= OnConnected;
            _clientLogic.OnDisconnected -= OnDisconnected;
        }
    }
}