using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PinchZoom : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private TMP_Text zoomText;
    [SerializeField] private float zoomSpeed = 0.01f;
    [SerializeField] private float minOrthoSize = 3f;
    [SerializeField] private float maxOrthoSize = 8f;

    private bool pinching;
    private float previousDistance;

    private void Update()
    {
        if (Touch.activeTouches.Count < 2)
        {
            pinching = false;
        }
        else
        {
            var firstTouch = Touch.activeTouches[0];
            var secondTouch = Touch.activeTouches[1];

            float currentDistance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);

            if (pinching)
            {
                float delta = currentDistance - previousDistance;

                targetCamera.orthographicSize = Mathf.Clamp(
                    targetCamera.orthographicSize - delta * zoomSpeed,
                    minOrthoSize,
                    maxOrthoSize
                );
            }

            previousDistance = currentDistance;
            pinching = true;
        }

        if (zoomText != null)
            zoomText.text = $"Zoom: {targetCamera.orthographicSize:0.0}";
    }
}