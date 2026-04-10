using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float dashDuration = 0.15f;
    [SerializeField] private float dashCooldown = 0.5f;
    [SerializeField] private VirtualDPad dPad;

    private Rigidbody2D rb;
    private bool isDashing;
    private float lastDashTime = -999f;
    private Vector2 lastMoveDirection = Vector2.right;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (isDashing)
            return;

        Vector2 moveInput = dPad != null ? dPad.MoveVector : Vector2.zero;
        rb.linearVelocity = moveInput * moveSpeed;

        if (moveInput.sqrMagnitude > 0.001f)
            lastMoveDirection = moveInput.normalized;
    }

    public bool TryDash(Vector2 dashDirection)
    {
        if (isDashing)
            return false;

        if (Time.time < lastDashTime + dashCooldown)
            return false;

        if (dashDirection.sqrMagnitude < 0.001f)
            dashDirection = lastMoveDirection;

        StartCoroutine(DashRoutine(dashDirection.normalized));
        return true;
    }

    private IEnumerator DashRoutine(Vector2 dashDirection)
    {
        isDashing = true;
        lastDashTime = Time.time;

        float timer = 0f;
        while (timer < dashDuration)
        {
            rb.linearVelocity = dashDirection * dashSpeed;
            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }
}