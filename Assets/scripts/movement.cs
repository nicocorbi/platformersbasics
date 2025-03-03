using UnityEngine;
using static UnityEngine.ParticleSystem;
public class Movement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Gradient gradient;
    
    [SerializeField] private float floatVelocityLowGravityThreshold = 4f;
    [SerializeField] private float maxAccelerationGround = 15f;
    [SerializeField] private float maxAccelerationAir = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Transform groundCheckOrigin;
    [SerializeField] MovementStats stats;
    [SerializeField] private float groundCheckDistance = 0.6f;
    public ParticleSystem particlesTracking;
    public ParticleSystem particlesBlood; 
    public ParticleSystem groundParticles;
    private bool isGrounded;
    private bool onPreviousFloor;
    private int numberJumps;
    private bool wantsJump;
    private bool wantsSustainedJump;
    private bool isJumping;
    private float timeJump;
    private Vector2 inputMovement;
    private float gradientValue = 1f;
    private float timeParticles;
    private float targetSpeed;
    private float newVelocityX;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gradient.Evaluate(1);
        particlesBlood.Stop();
        groundParticles.Stop();
        onPreviousFloor = false;
        timeParticles = 0f;
        

    }

    private void Update()
    {


        // Lectura de entrada para movimiento horizontal
        inputMovement = Vector2.zero;
        if (Input.GetKey(KeyCode.D)) inputMovement += Vector2.right;
        if (Input.GetKey(KeyCode.A)) inputMovement += Vector2.left;

        // Manejo del salto
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (numberJumps < stats.jumpLimit)
            {
                wantsJump = true;
                timeJump = 0;
            }
            else
            {
                // Si no quedan más saltos, activar partículas de advertencia
                if (!particlesBlood.isPlaying)
                {
                    particlesBlood.Play();
                }
            }
        }
        else if (Input.GetKey(KeyCode.W) && isJumping)
        {
            wantsSustainedJump = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            wantsSustainedJump = false;
            isJumping = false;
        }

        // Verificar si el personaje está en el suelo
        isGrounded = Physics2D.Raycast(groundCheckOrigin.position, Vector2.down, stats.groundCheckDistance, groundLayer);

        // Manejo del aterrizaje
        if (isGrounded && !onPreviousFloor)
        {
            HandleLanding();
        }

        // Manejo del temporizador para detener las partículas del suelo
        if (timeParticles > 0)
        {
            timeParticles -= Time.deltaTime;
            if (timeParticles <= 0 && groundParticles.isPlaying)
            {
                groundParticles.Stop();
            }
        }

        // Actualizar el estado anterior del suelo
        onPreviousFloor = isGrounded;
    }
    public void UpdateStat(MovementStats newStat)
    {
        stats = newStat;
    }

    private void FixedUpdate()
    {

        ApplyMovement();
        ApplyJump();
        GravitySummit();
        ApplyJump();
    }

    private void ApplyMovement()
    {
        targetSpeed = inputMovement.x * stats.maxSpeed;
        newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.acceleration * Time.fixedDeltaTime);
        // nuevaVelocidadX = rb.linearVelocity.x + (velocidadObjetivo - rb.linearVelocity.x) * stats.aceleracion * Time.fixedDeltaTime;
        // if (nuevaVelocidadX > velocidadMaxima)
        // {
        //     nuevaVelocidadX = velocidadMaxima;
        // }           
        // else if (nuevaVelocidadX < -velocidadMaxima)
        // {
        //    nuevaVelocidadX = -velocidadMaxima;
        // }
        //  rb.linearVelocity = new Vector2(nuevaVelocidadX, rb.linearVelocity.y);

        if (isGrounded)
        {



            if (inputMovement.x == 0)
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, stats.groundFriction * Time.fixedDeltaTime);
            }
            else
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.acceleration * Time.fixedDeltaTime);
            }


            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);

            newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.groundFriction * Time.fixedDeltaTime);
            if (stats.groundAcceleration >= maxAccelerationGround)
            {

                if ((rb.linearVelocity.x < 0 ? -rb.linearVelocity.x : rb.linearVelocity.x) > maxAccelerationAir)
                {
                    rb.linearVelocity = new Vector2((rb.linearVelocity.x < 0 ? -1 : 1) * maxAccelerationAir, rb.linearVelocity.y);
                }

            }
            
        }
        else
        {
            if (inputMovement.x == 0)
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, 0, stats.airFriction * Time.fixedDeltaTime);
            }
            else
            {

                newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.acceleration * Time.fixedDeltaTime);
            }


            rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
            newVelocityX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, stats.airAcceleration * Time.fixedDeltaTime);
            if (stats.airAcceleration >= maxAccelerationAir)
            {

                if ((rb.linearVelocity.x < 0 ? -rb.linearVelocity.x : rb.linearVelocity.x) > maxAccelerationAir)
                {
                    rb.linearVelocity = new Vector2((rb.linearVelocity.x < 0 ? -1 : 1) * maxAccelerationAir, rb.linearVelocity.y);
                }

            }
        }
        rb.linearVelocity = new Vector2(newVelocityX, rb.linearVelocity.y);
    }

    private void ApplyJump()
    {
        if (wantsJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.jumpForce);
            wantsJump = false;
            isJumping = true;
            numberJumps++;


            gradientValue = Mathf.Clamp01(1f - (float)numberJumps / stats.jumpLimit);
            spriteRenderer.color = gradient.Evaluate(gradientValue);
        }

        if (isJumping && wantsSustainedJump && timeJump < stats.maxJumpDuration)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, stats.sustainedJumpForce);
            timeJump += Time.fixedDeltaTime;
        }
    }

    private void HandleLanding()
    {

        particlesBlood.Stop();

        if (!groundParticles.isPlaying)
        {
            groundParticles.Play();
            timeParticles = 1f;
        }

        numberJumps = 0;

        // Reiniciar gradiente
        gradientValue = 1f;
        spriteRenderer.color = gradient.Evaluate(gradientValue);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheckOrigin.position, 0.01f);
        Gizmos.DrawLine(groundCheckOrigin.position, groundCheckOrigin.position + Vector3.down * groundCheckDistance);
    }
    private void GravitySummit()
    {
        if (rb.linearVelocityY >= floatVelocityLowGravityThreshold)
        {
            IncreasedGravity();


        }
        else if (rb.linearVelocity.y < -floatVelocityLowGravityThreshold)
        {
            ParticleSystem.MainModule mainModule = particlesTracking.main;
            mainModule.startColor = Color.blue;
            rb.gravityScale = stats.fallingGravity;

        }
        if (rb.linearVelocityY >= -floatVelocityLowGravityThreshold && rb.linearVelocityY <= floatVelocityLowGravityThreshold)
        {
            ParticleSystem.MainModule mainModule = particlesTracking.main;
            mainModule.startColor = Color.green;
            rb.gravityScale = stats.peakGravity;

        }
    }
    private void IncreasedGravity()
    {
        ParticleSystem.MainModule mainModule = particlesTracking.main;
        mainModule.startColor = Color.yellow;
        rb.gravityScale = stats.defaultGravity;
    }

}