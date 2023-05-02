using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Utils/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        public List<GameEventListener> m_listeners = new List<GameEventListener>();

        public void Invoke() {
            for(int i = 0; i < m_listeners.Count; i++) {
                m_listeners[i].OnEventInvoked();
            }
        }

        public void RegisterListener(GameEventListener listener) { 
            if(!m_listeners.Contains(listener)) { 
                m_listeners.Add(listener);
            }
        }

        public void UnregisterListener(GameEventListener listener) {
            if(m_listeners.Contains(listener)) { 
                m_listeners.Remove(listener);
            }
        }
    }
}
