using System;
using UnityEngine;
using DontLetHerIn.GameLoop;

namespace DontLetHerIn.Core
{
    /// <summary>
    /// Minimal owner of the global <see cref="GameState"/>.
    /// Coordinates high-level transitions only; the actual run and threat
    /// rules live in <see cref="RunController"/> and ThreatManager.
    /// </summary>
    public sealed class GameManager : MonoBehaviour
    {
        [SerializeField] private int totalFloors = RunController.DefaultTotalFloors;

        private RunController _runController;

        /// <summary>Raised whenever <see cref="CurrentState"/> changes.</summary>
        public event Action<GameState> StateChanged;

        public GameState CurrentState { get; private set; } = GameState.Boot;

        public RunController Run => _runController;

        private void Awake()
        {
            _runController = new RunController(totalFloors);
            _runController.OnRunWon += SetRunWon;
            _runController.OnRunLost += SetRunLost;
        }

        private void OnDestroy()
        {
            if (_runController == null) return;
            _runController.OnRunWon -= SetRunWon;
            _runController.OnRunLost -= SetRunLost;
        }

        /// <summary>Begin a new run and enter the run-start state.</summary>
        public void StartRun()
        {
            _runController.StartRun();
            SetState(GameState.RunStart);
        }

        /// <summary>Restart the run from the beginning.</summary>
        public void RestartRun()
        {
            _runController.RestartRun();
            SetState(GameState.RunStart);
        }

        /// <summary>Move to the results screen after a victory.</summary>
        public void SetRunWon()
        {
            SetState(GameState.RunWon);
            SetState(GameState.Results);
        }

        /// <summary>Move to the results screen after a defeat.</summary>
        public void SetRunLost()
        {
            SetState(GameState.RunLost);
            SetState(GameState.Results);
        }

        /// <summary>Set the current game state and notify listeners if it changed.</summary>
        public void SetState(GameState next)
        {
            if (CurrentState == next) return;
            CurrentState = next;
            StateChanged?.Invoke(next);
        }
    }
}
