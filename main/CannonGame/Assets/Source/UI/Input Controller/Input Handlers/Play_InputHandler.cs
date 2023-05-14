/* © 2023 - Greg Waller.  All rights reserved. */

using UnityEngine;
using UnityEngine.InputSystem;

namespace LRG.UI
{
    using System;

    using LRG.Master;

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
            _playmodeActions.Repair.performed += _repair;
            _playmodeActions.Purchase.performed += _purchase;
        }

        public override void Dispose()
        {
            _playmodeActions.Aim.performed -= _aim_begin;
            _playmodeActions.Aim.canceled -= _aim_end;
            _playmodeActions.Fire.performed -= _fire;
            _playmodeActions.Repair.performed -= _repair;
            _playmodeActions.Purchase.performed -= _purchase;
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
            _gameController.Player.Aim(value);
        }

        private void _fire(InputAction.CallbackContext context)
        {
            _gameController.Player.Fire();
        }

        private void _repair(InputAction.CallbackContext obj)
        {
            _gameController.Player.Repair();
        }

        private void _purchase(InputAction.CallbackContext obj)
        {
            _gameController.Player.PurchaseAmmo();
        }
    }
}