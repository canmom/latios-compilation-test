using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.XR.Management;
#if UNITY_EDITOR
#endif

namespace Holonautic.Scripts.Managed
{
    public class ActivateXr : MonoBehaviour
    {
        [SerializeField]
        public bool autoStart;

        [SerializeField]
        public Key activationKey = Key.Space;
        
        [SerializeField]
        public Key breakKey = Key.Enter;
        

        [Serializable]
        public enum XRState
        {
            None,
            StartingUp,
            Running,
            ShuttingDown,
        }

        [FormerlySerializedAs("_currentState")]
        [SerializeField]
        private XRState currentState;

        public XRState CurrentState => currentState;
        
        private OVRManager _ovrManager;
        
        private void Awake()
        {
            _ovrManager = FindObjectOfType<OVRManager>();
        }

        // Start is called before the first frame update
        void Start()
        {
            #if UNITY_EDITOR

            EditorApplication.playModeStateChanged += (playModeState) =>
            {
                if (playModeState == PlayModeStateChange.ExitingPlayMode)
                {
                    if (currentState == XRState.Running)
                    {
                        StopXR();
                    }
                }
            };


            if (autoStart)
            {
                StartCoroutine(StartXRCoroutine());
            }
            else
            {
                Debug.Log($"<color=#EB7DFF>Press `{activationKey}` to start XR</color>");
            }
            #elif UNITY_ANDROID
                 StartCoroutine(StartXRCoroutine());
            #endif
        }

        // Update is called once per frame
        void Update()
        {
            if (Keyboard.current == null)
            {
                enabled = false;
                return;
            }

            if (Keyboard.current[breakKey].wasPressedThisFrame)
            {
                Debug.Break();
            }
            
            if (Keyboard.current[activationKey].wasPressedThisFrame && currentState == XRState.None)
            {
                currentState = XRState.StartingUp;
                StartCoroutine(StartXRCoroutine());
            }

            if (Keyboard.current[activationKey].wasPressedThisFrame && currentState == XRState.Running)
            {
                StopXR();
            }

            if (Keyboard.current[activationKey].wasPressedThisFrame && currentState == XRState.Running)
            {
                StopXR();
            }
        }

        public IEnumerator StartXRCoroutine()
        {
            Debug.Log("Initializing XR...");
          
            yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

            if (XRGeneralSettings.Instance.Manager.activeLoader == null)
            {
                Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
                currentState = XRState.None;
            }
            else
            {
                Debug.Log("Starting XR...");
                XRGeneralSettings.Instance.Manager.StartSubsystems();
                currentState = XRState.Running;
            }
        }

        void StopXR()
        {
            Debug.Log("Stopping XR...");
            XRGeneralSettings.Instance.Manager.StopSubsystems();
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            Debug.Log("XR stopped completely.");
            currentState = XRState.None;
        }

        private void OnDisable()
        {
            if (currentState == XRState.Running)
            {
                StopXR();
            }
        }

        private void OnDestroy()
        {
            if (currentState == XRState.Running)
            {
                StopXR();
            }
        }
    }
}