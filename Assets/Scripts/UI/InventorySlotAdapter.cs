using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlotAdapter : MonoBehaviour
{
    private Image backgroundImage;
    private TextMeshProUGUI itemCountText;

    [SerializeField]
    private Image itemImage;

    private Color defaultColor;

    private void Awake() {
        backgroundImage = GetComponent<Image>();
        defaultColor = backgroundImage.color;
        itemCountText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetBackgroundColor(Color color) {
        backgroundImage.color = color;
    }

    public void resetBackgroundColor() {
        backgroundImage.color = defaultColor;
    }

    public void SetItemImage(Sprite sprite) {
        itemImage.sprite = sprite;
    }

    public void ResetItemImage() {
        itemImage.sprite = null;
    }

    public void SetItemImageColor(Color color) {
        itemImage.color = color;
    }

    public void SetItemCountText(string text) {
        itemCountText.text = text;
    }
}
