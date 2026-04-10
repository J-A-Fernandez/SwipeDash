using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class VirtualDPad : MonoBehaviour
{
    [SerializeField] private RectTransform background;
    [SerializeField] private RectTransform knob;

    private Finger activeFinger;
    private float radius;

    public Vector2 MoveVector { get; private set; }
    public Finger ActiveFinger => activeFinger;

    private void Start()
    {
        radius = Mathf.Min(background.rect.width, background.rect.height) * 0.5f;
        ResetJoystick();
    }

    private void Update()
    {
        if (activeFinger == null)
            TryAcquireFinger();
        else
            UpdateFinger();
    }

    private void TryAcquireFinger()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase != TouchPhase.Began)
                continue;

            if (RectTransformUtility.RectangleContainsScreenPoint(background, touch.screenPosition, null))
            {
                activeFinger = touch.finger;
                UpdateJoystick(touch.screenPosition);
                break;
            }
        }
    }

    private void UpdateFinger()
    {
        var touch = activeFinger.currentTouch;

        if (!touch.valid || touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            ResetJoystick();
            return;
        }

        UpdateJoystick(touch.screenPosition);
    }

    private void UpdateJoystick(Vector2 screenPosition)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            screenPosition,
            null,
            out Vector2 localPoint
        );

        Vector2 clamped = Vector2.ClampMagnitude(localPoint, radius);
        knob.anchoredPosition = clamped;
        MoveVector = clamped / radius;
    }

    private void ResetJoystick()
    {
        activeFinger = null;
        MoveVector = Vector2.zero;

        if (knob != null)
            knob.anchoredPosition = Vector2.zero;
    }
}