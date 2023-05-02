using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Material))]
public class SpriteShadow : MonoBehaviour, ISerializationCallbackReceiver
{
    [SerializeField]
    private Material m_shadowMaterial;

    public void OnAfterDeserialize() {
    }

    public void OnBeforeSerialize() {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
            spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            spriteRenderer.receiveShadows = true;
            spriteRenderer.material = m_shadowMaterial;
        }
    }

    // Start is called before the first frame update
    //void Start() {
    //    SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
    //    foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
    //        spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    //        spriteRenderer.receiveShadows = true;
    //        spriteRenderer.material = m_shadowMaterial;
    //    }
    //}
}
