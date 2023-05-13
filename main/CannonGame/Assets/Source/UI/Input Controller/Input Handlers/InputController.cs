/* © 2023 - Greg Waller.  All rights reserved. */

using System.Collections.Generic;
using UnityEngine;

namespace LRG.UI
{
    using LRG.Game;

    public interface IInputController
    {
    }

    public enum InputMode
    {
        Play,
    };

    public class InputController : MonoBehaviour, IInputController
    {
        private class _modeSwitch
        {
            private readonly _modeEnterDelegate _enter = null;
            private readonly _modeExitDelegate _exit = null;

            public _modeEnterDelegate Enter => _enter;
            public _modeExitDelegate Exit => _exit;

            public _modeSwitch(_modeEnterDelegate enterCallback, _modeExitDelegate exitCallback)
            {
                _enter = enterCallback;
                _exit = exitCallback;
            }
        }

        private ActionManager _actionManager;
        private InputMode _currentInputMode = InputMode.Play;
        private delegate void _modeEnterDelegate();
        private delegate void _modeExitDelegate();
        private Dictionary<InputMode, _modeSwitch> _modeSwitchMap;
        private Dictionary<InputMode, IInputHandler> _inputHandlerMap;

        public InputMode CurrentMode => _currentInputMode;
        
        private void LateUpdate()
        {
            if (_inputHandlerMap == null)
                return;

            _inputHandlerMap[_currentInputMode].Update();
        }

        public void RegisterGameController(GameController gameController)
        {
            _actionManager = new ActionManager();

            _inputHandlerMap = new Dictionary<InputMode, IInputHandler>
            {
                { InputMode.Play, new Play_InputHandler(gameController, _actionManager) },
            };
            _modeSwitchMap = new Dictionary<InputMode, _modeSwitch>
            {
                { InputMode.Play, new _modeSwitch( _entermode_play, _exitmode_play) },
            };

            _begin_mode(_currentInputMode);
        }

        #region Input Modes

        public IInputHandler GetInputHandlerForMode(InputMode mode)
        {
            return _inputHandlerMap[mode];
        }

        public void SetMode(InputMode inputMode)
        {
            _end_mode(_currentInputMode);
            _begin_mode(inputMode);
        }

        private void _end_mode(InputMode inputMode)
        {
            _modeSwitchMap[_currentInputMode].Exit();
            _inputHandlerMap[inputMode].Exit();
        }

        private void _begin_mode(InputMode inputMode)
        {
            _currentInputMode = inputMode;
            _modeSwitchMap[_currentInputMode].Enter();
            _inputHandlerMap[_currentInputMode].Enter();
        }

        #endregion

        #region Play Mode

        private void _entermode_play()
        {
        }

        private void _exitmode_play()
        {
        }

        #endregion

        #region Cursors

        private void _lock_cursor(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }

        private void _show_cursor(bool newState)
        {
            Cursor.visible = newState;
        }

        #endregion

    }
}