using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class SwipeDash : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private VirtualDPad dPad;
    [SerializeField] private float minSwipeDistancePixels = 60f;
    [SerializeField] private float maxSwipeTime = 0.5f;
    [SerializeField] private float rightSideStartNormalized = 0.5f;

    private Finger activeFinger;
    private Vector2 startScreenPos;
    private float startTime;

    public Finger ActiveFinger => activeFinger;

    private void Update()
    {
        if (activeFinger == null)
            TryBeginSwipe();
        else
            UpdateSwipe();
    }

    private void TryBeginSwipe()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase != TouchPhase.Began)
                continue;

            if (dPad != null && touch.finger == dPad.ActiveFinger)
                continue;

            if (touch.screenPosition.x < Screen.width * rightSideStartNormalized)
                continue;

            activeFinger = touch.finger;
            startScreenPos = touch.screenPosition;
            startTime = Time.time;
            Debug.Log("Swipe started");
            break;
        }
    }

    private void UpdateSwipe()
    {
        var touch = activeFinger.currentTouch;

        if (!touch.valid)
        {
            activeFinger = null;
            return;
        }

        if (touch.phase == TouchPhase.Ended)
        {
            Vector2 delta = touch.screenPosition - startScreenPos;
            float duration = Time.time - startTime;

            Debug.Log("Swipe delta: " + delta + " duration: " + duration);

            if (delta.magnitude >= minSwipeDistancePixels && duration <= maxSwipeTime)
            {
                bool dashed = player.TryDash(delta.normalized);
                Debug.Log("Dash triggered: " + dashed);
            }

            activeFinger = null;
        }
        else if (touch.phase == TouchPhase.Canceled)
        {
            activeFinger = null;
        }
    }
}