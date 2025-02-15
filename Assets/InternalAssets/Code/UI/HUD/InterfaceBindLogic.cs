using System;
using System.Collections.Generic;
using System.Linq;
using ProjectOlog.Code.UI.Core;
using UnityEngine;

namespace ProjectOlog.Code.UI.HUD
{
    public class InterfaceBindLogic
    {
        private List<BaseScreen> _screens;
        private List<IViewModel> _viewModels;
        private Dictionary<Type, BaseScreen> _shownScreens;

        public InterfaceBindLogic()
        {
            _screens = new List<BaseScreen>();
            _viewModels = new List<IViewModel>();
            _shownScreens = new Dictionary<Type, BaseScreen>();
        }

        public void RegisterScreen(BaseScreen screen)
        {
            if (!_screens.Contains(screen))
            {
                _screens.Add(screen);
                screen.Close();
                // screen.gameObject.SetActive(false);
            }
        }

        public void RegisterViewModel(IViewModel viewModel)
        {
            if (!_viewModels.Contains(viewModel))
            {
                _viewModels.Add(viewModel);
            }
        }
        
        
        // Cвязывает View и ViewModel
        private void BindView<TModel>(BaseScreen screen, TModel model) where TModel : IViewModel
        {
            // Находит screen
            // Совершает бинд и запускает
            // Добавляет в список

            if (!_shownScreens.ContainsKey(typeof(TModel)))
            {
                if (screen != null)
                {
                    screen.Bind(model);
                    screen.Show();
                    _shownScreens.Add(typeof(TModel), screen);
                }
            }
        }

        // Отвязывает View от ViewModel
        private void UnbindView<TModel>() where TModel : IViewModel
        {
            if (_shownScreens.TryGetValue(typeof(TModel), out var screen))
            {
                screen.Unbind();
                screen.Close();
                _shownScreens.Remove(typeof(TModel));
            }
        }
        
        public void SwitchView<TScreen>() where TScreen : BaseScreen
        {
            TScreen screen = _screens.OfType<TScreen>().FirstOrDefault();
            if (screen == null) return;

            Type modelType = screen.ModelType;
            if (_shownScreens.TryGetValue(modelType, out BaseScreen currentScreen))
            {
                currentScreen.Close();
                currentScreen.Unbind();
                
                _shownScreens.Remove(modelType);
            }

            IViewModel viewModel = _viewModels.FirstOrDefault(vm => vm.GetType() == modelType);
            if (viewModel != null)
            {
                screen.Show();
                screen.Bind(viewModel);
                
                _shownScreens.Add(modelType, screen);
            }
            else
            {
                // Обработка ситуации, когда модель представления не найдена
                Debug.LogWarning($"ViewModel of type {typeof(IViewModel)} not found in _viewModels.");
            }
        }
        
        public void SwitchView<TModel>(BaseScreen newScreen) where TModel : IViewModel
        {
            if (_shownScreens.ContainsKey(typeof(TModel)))
            {
                UnbindView<TModel>();
            }
            
            // Ищем модель представления в списке _viewModels на основе типа TModel
            var viewModel = _viewModels.OfType<TModel>().FirstOrDefault();
    
            if (viewModel != null)
            {
                // Приводим модель представления к типу TModel
                TModel typedViewModel = viewModel;
        
                // Связываем новый экран с найденной моделью представления
                BindView(newScreen, typedViewModel);
            }
            else
            {
                // Обработка ситуации, когда модель представления не найдена
                Debug.LogWarning($"ViewModel of type {typeof(TModel)} not found in _viewModels.");
            }
        }
    }
}