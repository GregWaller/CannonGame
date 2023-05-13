﻿/* © 2023 - Greg Waller.  All rights reserved. */

using LRG.Game;

using UnityEngine;
using UnityEngine.InputSystem;

namespace LRG.UI
{
    public class Play_InputHandler : InputHandler
    {
        private readonly @ActionManager.PlayActions _playmodeActions;

        protected override InputMode _inputMode => InputMode.Play;

        public Play_InputHandler(GameController gameController, @ActionManager actionManager)
            : base(gameController, actionManager)
        {
            _playmodeActions = _actionManager.Play;
            _playmodeActions.Disable();

            _playmodeActions.Aim.performed += _aim_begin;
            _playmodeActions.Aim.canceled += _aim_end;
            _playmodeActions.Fire.performed += _fire;
        }

        public override void Dispose()
        {
            _playmodeActions.Aim.performed -= _aim_begin;
            _playmodeActions.Aim.canceled -= _aim_end;
            _playmodeActions.Fire.performed -= _fire;
        }

        protected override void _enter()
        {
            _playmodeActions.Enable();
        }
        protected override void _exit()
        {
            _playmodeActions.Disable();
        }

        private void _aim_begin(InputAction.CallbackContext context)
        {
            _aim(context);
        }
        private void _aim_end(InputAction.CallbackContext context)
        {
            _aim(context);
        }
        private void _aim(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            _gameController.Cannon.Aim(value);
        }

        private void _fire(InputAction.CallbackContext context)
        {
            Debug.Log("Play_InputHandler::_fire");
        }
    }
}