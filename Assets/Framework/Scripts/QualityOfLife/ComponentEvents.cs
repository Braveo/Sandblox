using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Braveo.QOL {

    public class ComponentEvents : MonoBehaviour
    {
        [System.Serializable]
        public class StartEvents {

            public UnityEvent onAwake = new UnityEvent();
            public UnityEvent onStart = new UnityEvent();

        }
        [System.Serializable]
        public class EnableEvents {

            public UnityEvent onEnable = new UnityEvent();
            public UnityEvent onDisable = new UnityEvent();

        }
        [System.Serializable]
        public class UpdateEvents {

            public UnityEvent<float> onUpdate = new UnityEvent<float>();
            public UnityEvent<float> onFixed = new UnityEvent<float>();
            public UnityEvent<float> onLate = new UnityEvent<float>();

        }

        public StartEvents startEvents = new StartEvents();
        public EnableEvents enableEvents = new EnableEvents();
        public UpdateEvents updateEvents = new UpdateEvents();

        private void Awake() {
            startEvents.onAwake.Invoke();
        }

        private void Start() {
            startEvents.onStart.Invoke();
        }

        private void OnEnable() {
            enableEvents.onEnable.Invoke();
        }

        private void OnDisable() {
            enableEvents.onDisable.Invoke();
        }

        private void Update() {
            updateEvents.onUpdate.Invoke(Time.deltaTime);
        }

        private void FixedUpdate() {
            updateEvents.onFixed.Invoke(Time.fixedDeltaTime);
        }

        private void LateUpdate() {
            updateEvents.onLate.Invoke(Time.deltaTime);
        }
        
    }

}