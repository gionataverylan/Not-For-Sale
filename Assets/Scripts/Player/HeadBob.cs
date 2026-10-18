using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [SerializeField] private float bobSmoothSpeed = 8f;
    [SerializeField] private float bobFrequency = 8f;
    [SerializeField] private float rollAmplitude = 1f;
    [SerializeField] private float pitchAmplitude = 1f;
    [SerializeField] private float sprintBobMultiplier = 1.5f;

    [SerializeField] private Transform bobPivot;
    
    private PlayerMovement playerMovement;
    private PlayerInputReader playerInputReader;

    private float timer;
    private float currentRoll;
    private float currentBobPitch;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInputReader = GetComponent<PlayerInputReader>();
    }

    private void LateUpdate()
    {
            BobHead();
    }

    private void BobHead()
    {
        float targetRoll = 0f;
        float targetBobPitch = 0f;

        bool isMoving = playerMovement.IsGrounded && playerInputReader.Move.magnitude > 0.01f;
        float multiBobHead = playerMovement.IsSprinting ? sprintBobMultiplier : 1f;

        if (isMoving)
        {
            timer += Time.deltaTime * bobFrequency * multiBobHead;

            targetRoll = Mathf.Sin(timer) * rollAmplitude * multiBobHead;
            targetBobPitch = Mathf.Cos(timer * 2) * pitchAmplitude * multiBobHead;
        }

        currentRoll = Mathf.Lerp(currentRoll, targetRoll, Time.deltaTime * bobSmoothSpeed);
        currentBobPitch = Mathf.Lerp(currentBobPitch, targetBobPitch, Time.deltaTime * bobSmoothSpeed);

        bobPivot.localEulerAngles = new Vector3(currentBobPitch, 0, currentRoll);
    }
}
