using UnityEngine;

public class GroundItem : MonoBehaviour {
    public ItemObject item;

    private bool m_hasDialogue;

    public bool HasDialogue {
        get { return m_hasDialogue; }
    }
    
    [SerializeField]
    private string m_dialogueNode;

    public string DialogueNode {
        get { return m_dialogueNode; }
    }

    private void Awake() {
        m_hasDialogue = !string.IsNullOrEmpty(m_dialogueNode);
    }
}
