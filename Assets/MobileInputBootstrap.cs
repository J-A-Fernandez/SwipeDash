using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using TouchSimulation = UnityEngine.InputSystem.EnhancedTouch.TouchSimulation;

public class MobileInputBootstrap : MonoBehaviour
{
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();

#if UNITY_EDITOR
        TouchSimulation.Enable();
#endif
    }

    private void OnDisable()
    {
#if UNITY_EDITOR
        TouchSimulation.Disable();
#endif

        EnhancedTouchSupport.Disable();
    }
}