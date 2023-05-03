using UnityEngine;

[RequireComponent(typeof(Material))]
public class SpriteShadow : MonoBehaviour, ISerializationCallbackReceiver {
    [SerializeField]
    private Material m_shadowMaterial;

    [SerializeField]
    private bool m_receiveShadow;

    [SerializeField]
    private bool m_castShadow;

    public void OnAfterDeserialize() {
    }

    public void OnBeforeSerialize() {
        SpriteRenderer[] spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        if (m_receiveShadow && m_castShadow) {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                spriteRenderer.receiveShadows = true;
                spriteRenderer.material = m_shadowMaterial;
            }
        } else if (m_receiveShadow) {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                spriteRenderer.receiveShadows = true;
                spriteRenderer.material = m_shadowMaterial;
            }
        } else if (m_castShadow) {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
                spriteRenderer.receiveShadows = false;
                spriteRenderer.material = m_shadowMaterial;
            }
        } else {
            foreach (SpriteRenderer spriteRenderer in spriteRenderers) {
                spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                spriteRenderer.receiveShadows = false;
                spriteRenderer.material = m_shadowMaterial;
            }
        }
    }
}
