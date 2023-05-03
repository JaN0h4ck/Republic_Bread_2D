using UnityEngine;

public class NPC : MonoBehaviour {
    private bool m_hasDialogue = false;
    public bool hasDialogue {
        get { return m_hasDialogue; }
    }

    [SerializeField]
    private string m_dialogueNode;

    public string dialogueNode {
        get { return m_dialogueNode; }
    }

    private bool m_hasInspect = false;
    public bool hasInspect {
        get { return m_hasInspect; }
    }

    [SerializeField]
    private string m_inspectNode;

    public string inspectNode {
        get { return m_inspectNode; }
    }

    private void Awake() {
        m_hasDialogue = !string.IsNullOrEmpty(m_dialogueNode);
        m_hasInspect = !string.IsNullOrEmpty(m_inspectNode);
    }
}
