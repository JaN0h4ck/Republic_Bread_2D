using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SceneLadder : SceneDoor
{
    [SerializeField]
    private GameObject m_ladderTop;

    [SerializeField]
    private GameObject m_spritePrefab;
    public override void LocationChangeCurrent(NavMeshAgent agent) {
        StartCoroutine(ChangeLocation(agent));
    }

    private IEnumerator ChangeLocation(NavMeshAgent agent) {
        InputContainer.Instance.DisableActions();
        GameObject climbSprite = Instantiate(m_spritePrefab, agent.transform.position, Quaternion.identity, transform);
        climbSprite.transform.position = transform.position;
        agent.gameObject.SetActive(false);
        while (climbSprite.transform.position.y < m_ladderTop.transform.position.y) {
            climbSprite.transform.position = Vector3.MoveTowards(climbSprite.transform.position, m_ladderTop.transform.position, .5f) * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(climbSprite);
        agent.gameObject.SetActive(true);
        InputContainer.Instance.EnableActions();
        base.LocationChangeCurrent(agent);
    }
}
