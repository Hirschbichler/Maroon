//-----------------------------------------------------------------------------
// ResetBtnScript.cs
//
// Script for reset button
//
//
// Authors: Michael Stefan Holly
//          Michael Schiller
//          Christopher Schinnerl
//-----------------------------------------------------------------------------
//

using System;
using UnityEngine;
using UnityEngine.UI;

namespace Maroon.UI.Buttons
{
    /// <summary>
    /// Script for reset button
    /// </summary>
    public class ResetBtnScript : BaseButton
    {
        public bool AllowWholeReset = false;
        public bool AllowWithButtonPress = true;

        private bool _inWholeResetMode = false;
        private CanvasRenderer _canvasRenderer;
        private Button _button;
        
        protected override void Start()
        {
            base.Start();

            SimulationController.Instance.OnReset += OnResetHandler;
            SimulationController.Instance.OnStart += OnStartHandler;

            _button = gameObject.GetComponent<Button>();
            _canvasRenderer = gameObject.GetComponent<CanvasRenderer>();

            if(!AllowWholeReset) Disable();
            else Enable();
        }

        protected void Update()
        {
            if (_inWholeResetMode && SimulationController.Instance.SimulationRunning) _inWholeResetMode = false;
        }

        private void OnStartHandler(object sender, EventArgs e)
        {
            if (AllowWholeReset) Enable();
            else _inWholeResetMode = false;
        }

        private void OnResetHandler(object sender, EventArgs e)
        {
            if (!AllowWholeReset) Disable();
            else _inWholeResetMode = true;
        }

        /// <summary>
        /// Handles the button being pressed and resets the simulation
        /// </summary>
        public void ButtonResetPressed()
        {
            if (!_inWholeResetMode)
                SimulationController.Instance.ResetSimulation();
            else
                SimulationController.Instance.ResetWholeSimulation();
        }
    }
}