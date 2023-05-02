using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Utils.Events
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent m_event;
        public UnityEvent m_response;

        public void OnEventInvoked() {
            m_response.Invoke();
        }

        private void OnEnable()
        {
            m_event.RegisterListener(this); 
        }

        private void OnDisable() {
            m_event.UnregisterListener(this);
        }
    }
}
