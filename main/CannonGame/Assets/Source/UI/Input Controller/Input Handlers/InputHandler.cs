/* © 2023 - Greg Waller.  All rights reserved. */

using System;

namespace LRG.UI
{
    using LRG.Master;

    public interface IInputHandler
    {
        void Enter();
        void Exit();
        void Update();
    }

    public abstract class InputHandler : IInputHandler, IDisposable
    {
        protected abstract InputMode _inputMode { get; }
        protected ActionManager _actionManager = null;
        protected GameController _gameController = null;

        public bool IsActive { get; private set; } = false;

        protected InputHandler(GameController gameController, ActionManager actionManager)
        {
            _gameController = gameController;
            _actionManager = actionManager;
        }

        public abstract void Dispose();

        public void Enter()
        {
            IsActive = true;
            _enter();
        }
        protected abstract void _enter();

        public void Exit()
        {
            IsActive = false;
            _exit();
        }
        protected abstract void _exit();

        public virtual void Update() { }
    }
}