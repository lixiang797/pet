using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class AnimalController : MonoBehaviour
{
    [SerializeField] private AnimalData animalData;
    [SerializeField] private float aiSpeedMultiplier = 0.92f;
    [SerializeField] private float laneSpeed = 4f;
    [SerializeField] private string groundTag = "Ground";

    private Rigidbody body;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private float horizontalInput;
    private bool jumpRequested;
    private bool grounded;

    public string AnimalName => animalData != null ? animalData.AnimalName : gameObject.name;
    public float Progress { get; private set; }
    public float FinishDistance { get; set; } = 60f;
    public bool IsPlayerControlled { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsRaceActive { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.constraints |= RigidbodyConstraints.FreezeRotation;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
    }

    public void PrepareForRace(bool playerControlled, float finishDistance)
    {
        IsPlayerControlled = playerControlled;
        FinishDistance = Mathf.Max(1f, finishDistance);
        IsFinished = false;
        IsRaceActive = false;
        Progress = 0f;
        horizontalInput = 0f;
        jumpRequested = false;

        body.velocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(spawnPosition, spawnRotation);
    }

    public void SetRaceActive(bool active)
    {
        IsRaceActive = active;
    }

    private void Update()
    {
        if (!IsRaceActive || IsFinished)
            return;

        if (IsPlayerControlled)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
                jumpRequested = true;
        }
        else
        {
            horizontalInput = 0f;
        }

        Progress = Mathf.Max(0f, transform.position.z - spawnPosition.z);
        if (Progress >= FinishDistance)
        {
            Progress = FinishDistance;
            IsFinished = true;
            IsRaceActive = false;
            body.velocity = Vector3.zero;
            FindObjectOfType<RaceManager>()?.RegisterFinish(this);
        }
    }

    private void FixedUpdate()
    {
        if (!IsRaceActive || IsFinished)
            return;

        float speed = animalData != null ? animalData.RunSpeed : 6f;
        if (!IsPlayerControlled)
            speed *= aiSpeedMultiplier;

        Vector3 velocity = body.velocity;
        velocity.z = speed;
        velocity.x = IsPlayerControlled ? horizontalInput * laneSpeed : 0f;
        body.velocity = velocity;

        if (jumpRequested && grounded)
        {
            float jumpForce = animalData != null ? animalData.JumpForce : 6f;
            body.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            grounded = false;
        }

        jumpRequested = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag(groundTag))
            grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag(groundTag))
            grounded = false;
    }
}
