
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool pressed = false;

    public bool GetPress() { return pressed; }

    public void OnPointerDown(PointerEventData eventData) {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData) {
        pressed = false;
    }
}
