using System.Collections;
using UnityEngine;

public class PlayerSquashStretch : MonoBehaviour
{
    //References 
    [SerializeField] private Renderer[] renderers;
    [SerializeField] private PlayerMovement playerMovement;

    //Stretch settings
    public float maxStretch = 0.15f;
    public float jumpStretch = 0.2f;
    public float landSquash = -0.12f;
    public float landDuration = 0.1f;
    public float smoothSpeed = 10f;

    //Internal variables
    float currentStretch;
    float targetStretch;
    bool wasGrounded;

    void Update()
    {
        CalculateStretch();
        ApplyStretch();
    }

    //Makes all calculations for the stretch amount
    void CalculateStretch()
    {
        //Stretches by velocity
        float speed01 = Mathf.Clamp01(playerMovement.currentSpeed / playerMovement.maxSpeed);
        targetStretch = Mathf.Lerp(0f, maxStretch, speed01);

        //Stretch on air
        if (!playerMovement.grounded)
            targetStretch = Mathf.Max(targetStretch, jumpStretch);

        //Squash on landing
        bool grounded = playerMovement.grounded;
        if (!wasGrounded && grounded)
            StartCoroutine(LandSquash());

        wasGrounded = grounded;

        //Smoothness transition of stretch amount
        currentStretch = Mathf.Lerp(currentStretch, targetStretch, Time.deltaTime * smoothSpeed);
    }
    //Coroutine to handle landing squash effect
    IEnumerator LandSquash()
    {
        currentStretch = landSquash;
        yield return new WaitForSeconds(landDuration);
    }

    //Applys to all materials
    void ApplyStretch()
    {
        for (int r = 0; r < renderers.Length; r++)
        {
            Material[] mats = renderers[r].materials;

            for (int m = 0; m < mats.Length; m++)
            {
                mats[m].SetFloat("_StretchAmount", currentStretch);
            }
        }
    }

    //Is called in the PlayerMovement script when jumping
    public void OnJump()
    {
        currentStretch = jumpStretch;
    }
}
