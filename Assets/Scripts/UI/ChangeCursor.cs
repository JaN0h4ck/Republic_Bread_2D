using UnityEngine;
using UnityEngine.EventSystems;

public class ChangeCursor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField]
    private CursorTextureObject cursorTextures;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData) {
        Cursor.SetCursor(cursorTextures.HoverCursor, cursorTextures.Hotspot, CursorMode.Auto);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData) {
        Cursor.SetCursor(cursorTextures.DefaultCursor, cursorTextures.Hotspot, CursorMode.Auto);
    }
}
