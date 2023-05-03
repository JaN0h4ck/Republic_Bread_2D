using UnityEngine;
using UnityEngine.AI;
using Yarn.Unity;

public class SceneDoor : MonoBehaviour {
    public GameObject m_connectedDoor;

    private Camera m_SceneCamera;
    private SceneDoor m_connectedSceneDoor;

    //Wird benötigt um Yarn Variablen aus dem Storage zu lesen.
    private InMemoryVariableStorage m_variableStorage;

    private bool m_DialogueEmpty;
    private DialogueRunner m_DialogueRunner;

    private CameraFollow m_CameraFollow;
    private bool m_hasCameraFollow;

    [SerializeField]
    [Tooltip("Auf \"true\" setzen, wenn nach einem Szenenwechsel Dialog abgespielt werden soll.")]
    private bool m_StartDialogue;

    [SerializeField]
    [Tooltip("Auf \"true\" setzen, wenn der Dialog nur ein mal abgespielt werden soll")]
    private bool m_DialogueOnce;

    [SerializeField]
    [Tooltip("Hier muss der Node Name des Dialogs rein, der nach einem Szenenwechsel gespielt werden soll")]
    private string m_DialogueName;

    [SerializeField]
    [Tooltip("Auf True setzen, wenn das freischalten der Tür von einer Yarn Varbiable abhängig ist")]
    private bool m_blockDoorByDefault = false;

    [SerializeField]
    [Tooltip("Name der Variable aus dem Yarn script, die die Tür freischaltet, \"$\" am Anfang nicht vergessen")]
    private string m_yarnVariableName;

    [SerializeField]
    [Tooltip("Wert der Variable, der zum freischalten benötigt wird, funktioniert nur mit integern")]
    private int m_variabaleValueToUnlock = 1;

    [SerializeField]
    private string m_doorFailDialogueName;

    [SerializeField]
    [Tooltip("Name des Yarn Nodes für den inspect")]
    private string m_inspectName;

    private bool m_hasInspect = false;

    private void Start() {
        //Sanity Check
        if (m_blockDoorByDefault && string.IsNullOrEmpty(m_yarnVariableName)) {
            Debug.LogError("SceneDoor: m_blockDoorByDefault ist auf true gesetzt, aber m_yarnVariableName ist leer!");
            m_blockDoorByDefault = false;
        }

        if (m_blockDoorByDefault) {
            char[] yarnVariableChar = m_yarnVariableName.ToCharArray();

            if (yarnVariableChar[0] != '$') {
                Debug.LogError("SceneDoor: m_yarnVariableName muss mit \"$\" beginnen!");
                m_blockDoorByDefault = false;
            }
        }

        if (!string.IsNullOrEmpty(m_inspectName))
            m_hasInspect = true;

        m_variableStorage = FindObjectOfType<InMemoryVariableStorage>();

        if (m_connectedDoor)
            m_connectedSceneDoor = m_connectedDoor.GetComponent<SceneDoor>();

        m_SceneCamera = FindSceneCamera(gameObject.transform.parent);


        m_CameraFollow = m_SceneCamera.GetComponent<CameraFollow>();
        if (m_CameraFollow != null)
            m_hasCameraFollow = true;

        m_DialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
        m_DialogueEmpty = string.IsNullOrEmpty(m_DialogueName);
    }

    // TODO: ummünzuen auf neues Input
    //private void OnMouseOver() {
    //    if (Input.GetMouseButtonDown(1) && m_hasInspect && !m_DialogueRunner.IsDialogueRunning) {
    //        GameObject.Find("brotagonist").GetComponent<NavMeshAgent>().ResetPath();
    //        m_DialogueRunner.StartDialogue(m_inspectName);
    //    }
    //}

    public void LocationChangeCurrent(NavMeshAgent agent) {
        if (!m_blockDoorByDefault) {
            ChangeCurrent(agent);
        } else {
            m_variableStorage.TryGetValue(m_yarnVariableName, out float value);
            if ((int)value == m_variabaleValueToUnlock) {
                ChangeCurrent(agent);
            } else if (string.IsNullOrEmpty(m_doorFailDialogueName))
                Debug.LogError("SceneDoor: m_blockDoorByDefault ist auf true gesetzt, aber m_doorFailDialogueName ist leer!");
            else {
                m_DialogueRunner.StartDialogue(m_doorFailDialogueName);
            }
        }
    }


    private void ChangeCurrent(NavMeshAgent agent) {
        m_SceneCamera.gameObject.SetActive(false);
        m_SceneCamera.GetComponent<AudioListener>().enabled = false;
        m_connectedSceneDoor.LocationChangeNext(agent);
        if (!m_DialogueEmpty && m_StartDialogue && !m_DialogueOnce)
            m_DialogueRunner.StartDialogue(m_DialogueName);
        else if (!m_DialogueEmpty && m_StartDialogue && m_DialogueOnce) {
            m_DialogueRunner.StartDialogue(m_DialogueName);
            m_StartDialogue = false;
        }
    }

    public void LocationChangeNext(NavMeshAgent agent) {
        agent.Warp(gameObject.transform.GetChild(0).position);
        if (m_hasCameraFollow)
            m_CameraFollow.ResetCamera();
        m_SceneCamera.gameObject.SetActive(true);
        m_SceneCamera.GetComponent<AudioListener>().enabled = true;
    }

    /// <summary>
    /// Rekursive Suche, die die Hierarchie hinauf geht, um die Camera zu finden, die zum SceneDoor gehört
    /// </summary>
    /// <param name="_parent"> Nächst höheres Parent </param>
    private Camera FindSceneCamera(Transform _parent) {
        Camera _cam;

        _cam = _parent.GetComponentInChildren<Camera>(true);
        if (_cam != null)
            return _cam;
        else
            return FindSceneCamera(_parent.parent);
    }
}
