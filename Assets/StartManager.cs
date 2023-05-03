using UnityEngine;
using Yarn.Unity;

public class StartManager : MonoBehaviour {
    DialogueRunner m_dialogueRunner;

    [SerializeField]
    private string m_startNode;

    // Start is called before the first frame update
    void Start() {
        m_dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }

    private void Update() {
        m_dialogueRunner.StartDialogue(m_startNode);
        Destroy(gameObject);
    }
}
