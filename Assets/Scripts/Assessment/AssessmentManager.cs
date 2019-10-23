﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Antares.Evaluation;
using Antares.Evaluation.Engine;
using Antares.Evaluation.Util;
using Maroon.Assessment.Handler;

namespace Maroon.Assessment
{
    [RequireComponent(typeof(AssessmentFeedbackHandler))]
    public class AssessmentManager : MonoBehaviour
    { 
        [SerializeField]
        private string amlFile;

        private Evaluator _evalService;

        private EventBuilder _eventBuilder;

        private AssessmentFeedbackHandler _feedbackHandler;

        private readonly List<AssessmentWatchValue> _assessmentValues = new List<AssessmentWatchValue>();

        public bool IsConnected { get; private set; }

        private static AssessmentManager _instance;


        public static AssessmentManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = FindObjectOfType<AssessmentManager>();
                return _instance;
            }
        }

        private void Awake()
        {
            _feedbackHandler = FindObjectOfType<AssessmentFeedbackHandler>();

            IsConnected = ConnectToAssessmentSystem();
        }

        private void Start()
        {
            Debug.Log("AssessmentManager::Send Enter Event");
            
            if(_eventBuilder == null)
                _eventBuilder = EventBuilder.Event();

            _eventBuilder.Action("enter");
        }

        private void LateUpdate()
        {
            if (_eventBuilder == null) return;

            SendGameEvent(_eventBuilder);
            _eventBuilder = null;
        }

        private bool ConnectToAssessmentSystem()
        {
            try
            {
                Debug.Log("AssessmentManager: Connecting to Assessment Service...");
                _evalService = new Evaluator();
                Debug.Log("AssessmentManager: Successfully connected to Assessment Service.");

                Debug.Log($"AssessmentManager: Loading {amlFile} into evaluation engine ...");
                _evalService.LoadAmlFile(Application.dataPath + amlFile);
                Debug.Log("AssessmentManager: Assessment model loaded.");

                _evalService.FeedbackReceived += delegate (object sender, FeedbackEventArgs args)
                {
                  _feedbackHandler.HandleFeedback(args);
                };
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"AssessmentManager: An error occurred while connecting to the Assessment service.: {ex.Message} {ex.StackTrace}");
                return false;
            }
        }

        public void RegisterAssessmentObject(AssessmentObject assessmentObject)
        {
            Debug.Log($"AssessmentManager::RegisterAssessmentObject: {assessmentObject.ObjectID}");

            if(_eventBuilder == null)
                _eventBuilder = EventBuilder.Event();

            _eventBuilder
                .PerceiveObject(assessmentObject.ObjectID)
                .Set("class", assessmentObject.ClassType.ToString());

            foreach (var watchValue in assessmentObject.WatchValues)
                _eventBuilder.Set(watchValue.PropertyName, watchValue.GetValue());
        }

        public void DeregisterAssessmentObject(AssessmentObject assessmentObject)
        {
            Debug.Log($"AssessmentManager::DeregisterAssessmentObject: {assessmentObject.ObjectID}");

            if (_eventBuilder == null)
                _eventBuilder = EventBuilder.Event();

            _eventBuilder.UnlearnObject(assessmentObject.ObjectID);
        }

        public void SendUserAction(string actionName, string objectId=null)
        {
            Debug.Log($"AssessmentManager::SendUserAction: {objectId}.{actionName}");

            if (_eventBuilder == null)
                _eventBuilder = EventBuilder.Event();

            _eventBuilder.Action(actionName, objectId);
            foreach (var watchValue in GetComponents<AssessmentWatchValue>())
            {
                if (watchValue.IsDynamic)
                {
                    _eventBuilder.UpdateDataOf(watchValue.ObjectID)
                        .Set(watchValue.PropertyName, watchValue.GetValue());
                }
            }
        }

        public void SendDataUpdate(string objectId, string propertyName, object value)
        {
            Debug.Log($"AssessmentManager::SendDataUpdate: {objectId}.{propertyName}={value}");

            if (_eventBuilder == null)
                _eventBuilder = EventBuilder.Event();

            _eventBuilder.UpdateDataOf(objectId).Set(propertyName, value);
        }

        private void SendGameEvent(GameEvent gameEvent)
        {
            if (IsConnected)
            {
                _evalService.ProcessEvent(gameEvent);
            }
            else
            {
                Debug.LogWarning("AssessmentManager::SendGameEvent: Assessment service is not running");
            }
        }
    }
}
