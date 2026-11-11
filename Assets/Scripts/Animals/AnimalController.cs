using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class AnimalController : MonoBehaviour
{
    [SerializeField] private AnimalData animalData;
    [SerializeField, Range(0.7f, 1.1f)] private float aiSpeedMultiplier = 0.92f;
    [SerializeField] private float laneSpeed = 4f;
    [SerializeField] private KeyCode skillKey = KeyCode.LeftShift;

    private Rigidbody body;
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private float horizontalInput;
    private bool jumpRequested;
    private bool grounded;
    private float speedPenalty;
    private float speedBoostTimer;
    private float speedMultiplier = 1f;
    private float jumpBoostTimer;
    private float jumpMultiplier = 1f;
    private float powerBoostTimer;
    private float powerMultiplier = 1f;
    private float cooldown;
    private float stamina = 100f;
    private float baseSpeed = 6f;
    private float baseJump = 6f;
    private float basePower = 2f;
    private SkillBase skill;
    private RaceManager raceManager;

    public string AnimalName => animalData != null ? animalData.AnimalName : gameObject.name;
    public float Progress { get; private set; }
    public float FinishDistance { get; private set; } = 60f;
    public bool IsPlayerControlled { get; private set; }
    public bool IsFinished { get; private set; }
    public bool IsRaceActive { get; private set; }
    public float SkillCooldown => Mathf.Max(0f, cooldown);
    public string SkillName => skill != null ? skill.SkillName : "No skill";

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        body.constraints |= RigidbodyConstraints.FreezeRotation;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        if (animalData != null) SetAnimalData(animalData);
    }

    public void SetAnimalData(AnimalData data)
    {
        animalData = data;
        if (data == null) return;
        baseSpeed = data.RunSpeed;
        baseJump = data.JumpForce;
        basePower = data.Power;
        stamina = data.Stamina;
        skill = data.Skill;
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
        speedPenalty = 0f;
        speedMultiplier = jumpMultiplier = powerMultiplier = 1f;
        speedBoostTimer = jumpBoostTimer = powerBoostTimer = cooldown = 0f;
        stamina = animalData != null ? animalData.Stamina : 100f;
        if (body != null)
        {
            body.velocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
        }
        transform.SetPositionAndRotation(spawnPosition, spawnRotation);
        raceManager = FindObjectOfType<RaceManager>();
    }

    public void SetRaceActive(bool active) => IsRaceActive = active;

    private void Update()
    {
        if (!IsRaceActive || IsFinished) return;
        float dt = Time.deltaTime;
        cooldown = Mathf.Max(0f, cooldown - dt);
        speedBoostTimer = TickBoost(speedBoostTimer, ref speedMultiplier);
        jumpBoostTimer = TickBoost(jumpBoostTimer, ref jumpMultiplier);
        powerBoostTimer = TickBoost(powerBoostTimer, ref powerMultiplier);

        if (IsPlayerControlled)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (Input.GetKeyDown(KeyCode.Space) && grounded) jumpRequested = true;
            if (Input.GetKeyDown(skillKey) && cooldown <= 0f && skill != null)
            {
                skill.Activate(this);
                cooldown = skill.Cooldown;
            }
        }
        else
        {
            horizontalInput = 0f;
            TryAiJump();
            if (cooldown <= 0f && skill != null && Random.value < 0.004f)
            {
                skill.Activate(this);
                cooldown = skill.Cooldown;
            }
        }

        Progress = Mathf.Max(0f, transform.position.z - spawnPosition.z);
        if (Progress >= FinishDistance)
        {
            Progress = FinishDistance;
            IsFinished = true;
            IsRaceActive = false;
            body.velocity = Vector3.zero;
            if (raceManager != null) raceManager.RegisterFinish(this);
        }
    }

    private float TickBoost(float timer, ref float multiplier)
    {
        if (timer <= 0f) return 0f;
        timer -= Time.deltaTime;
        if (timer <= 0f) multiplier = 1f;
        return Mathf.Max(0f, timer);
    }

    private void FixedUpdate()
    {
        if (!IsRaceActive || IsFinished) return;
        float speed = baseSpeed * (IsPlayerControlled ? 1f : aiSpeedMultiplier) * speedMultiplier;
        speed = Mathf.Max(1f, speed - speedPenalty);
        speedPenalty = Mathf.MoveTowards(speedPenalty, 0f, Time.fixedDeltaTime * 0.8f);
        Vector3 velocity = body.velocity;
        velocity.z = speed;
        velocity.x = IsPlayerControlled ? horizontalInput * laneSpeed : 0f;
        body.velocity = velocity;

        if (jumpRequested && grounded)
        {
            body.AddForce(Vector3.up * baseJump * jumpMultiplier, ForceMode.VelocityChange);
            grounded = false;
        }
        jumpRequested = false;
    }

    private void TryAiJump()
    {
        if (!grounded) return;
        if (Physics.Raycast(transform.position + Vector3.up * 0.35f, Vector3.forward, out RaycastHit hit, 3f))
        {
            if (hit.collider.GetComponent<Obstacle>() != null) jumpRequested = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Ground")) grounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Ground")) grounded = false;
    }

    public void ApplySpeedBoost(float multiplier, float duration)
    {
        speedMultiplier = Mathf.Max(1f, multiplier);
        speedBoostTimer = Mathf.Max(0.1f, duration);
    }

    public void ApplyJumpBoost(float multiplier, float duration)
    {
        jumpMultiplier = Mathf.Max(1f, multiplier);
        jumpBoostTimer = Mathf.Max(0.1f, duration);
    }

    public void ApplyPowerBoost(float multiplier, float duration)
    {
        powerMultiplier = Mathf.Max(1f, multiplier);
        powerBoostTimer = Mathf.Max(0.1f, duration);
    }

    public void RecoverStamina(float amount) => stamina = Mathf.Clamp(stamina + amount, 0f, 100f);
    public void ApplySpeedPenalty(float amount) => speedPenalty = Mathf.Clamp(speedPenalty + amount, 0f, baseSpeed * 0.8f);
    public float GetPullForce() => basePower * powerMultiplier * (0.5f + stamina / 200f);
    public void ConsumeStamina(float amount) => stamina = Mathf.Clamp(stamina - amount, 0f, 100f);
}
