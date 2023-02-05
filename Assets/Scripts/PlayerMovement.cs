using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    public string playerName;

    public float moveSpeed;
    private float boostValue = 3;
    public Rigidbody2D rb;
    private float roundedX;
    private float roundedY;
    public float rotationSpeed;

    public ParticleSystem digParticles;

    public AudioSource digSoundSource;
    public List<AudioClip> digClips = new List<AudioClip>();

    public AudioSource collectionSoundSource;
    public List<AudioClip> collectionClips = new List<AudioClip>();

    public AudioSource boostSoundSource;
    public List<AudioClip> boostClips = new List<AudioClip>();
    public AudioClip hitRootClip;

    private Vector2 moveDirection;
    private Vector2 rotateDirection;

    public RootPlacementController rootController;

    public bool isRetracting = false;

    public bool isBoosting = false;
    public bool canBoost = true;
    private float boostStart;
    private float boostDuration = 1;
    private float boostCoolDown = 5;
    private float boostEnd = -5;

    private ParticleSystem.MinMaxCurve baseDigEmissionRate;

    void Start() {
        baseDigEmissionRate = digParticles.emission.rateOverTime;
    }

    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        Move();
    }

    void ProcessInputs()
    {
        
        // Check public values and set defaults
        if (rotationSpeed == 0)
        {
            rotationSpeed = 5;
        }

        if (moveSpeed == 0)
        {
            moveSpeed = 5;
        }


        if (Input.GetButtonDown(playerName + "_Fire1"))
        {
            isRetracting = true;
        }

        if (!isRetracting)
        {
            float moveX = Input.GetAxisRaw(playerName + "_Horizontal");
            roundedX = (int)Math.Round(moveX, 0);

            float moveY = Input.GetAxisRaw(playerName + "_Vertical");
            roundedY = (int)Math.Round(moveY, 0);

            float boostCooldownLeft = (float)Math.Round(Time.time, 2) - boostEnd + 1.1f;
            canBoost = boostCoolDown < boostCooldownLeft;

            // trigger boost
            if (Input.GetButtonDown(playerName + "_Fire2"))
            {
                if (canBoost)
                {
                    isBoosting = true;
                    boostStart = (float)Math.Round(Time.time, 2);
                    if (boostSoundSource && !boostSoundSource.isPlaying) {
                        boostSoundSource.clip = boostClips[UnityEngine.Random.Range(0, boostClips.Count)];
                        boostSoundSource.Play();
                    }
                }

            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            isRetracting = rootController.RetractRoots();
            isBoosting = false;
        }

        moveDirection = new Vector2(roundedX, roundedY).normalized;

        // Using minus rotation because the sprite is facing down so everything needs to be reversed 
        rotateDirection = new Vector2(-roundedX, -roundedY).normalized;

        // Used to rotate the sprite to face where it is going
        if (moveDirection != Vector2.zero)
        {
            if (digSoundSource && !digSoundSource.isPlaying) {
                digSoundSource.clip = digClips[UnityEngine.Random.Range(0, digClips.Count)];
                digSoundSource.Play();
            }
            Quaternion toRotation = Quaternion.LookRotation(Vector3.forward, rotateDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * 300f * Time.deltaTime);
        }

    }

    public void PlayHitEffects() {
        boostSoundSource?.PlayOneShot(hitRootClip);
        GameController.screenshake.Shake(0.3f, 0.3f);
    }

    public void PlayCollectEffects()
    {
        // playcollection sound
        Debug.Log("playing collion sound");
        collectionSoundSource.PlayOneShot(collectionClips[UnityEngine.Random.Range(0, collectionClips.Count)]);
    }

    void Move()
    {
        var digParticlesMain = digParticles.main;
        var digParticlesEmission = digParticles.emission;
        if (isBoosting)
        {
            rb.velocity = moveDirection * moveSpeed * boostValue;
            if (rb.velocity.magnitude > 0) {
                digParticlesEmission.rateOverTime = new ParticleSystem.MinMaxCurve(baseDigEmissionRate.constant * 5f);
                if (!digParticles.isPlaying) {
                    digParticles.Play();
                }
            }
            float boostLength = (float)Math.Round(Time.time, 2) - boostStart;
            if (boostLength > boostDuration)
            {
                canBoost = false;
                isBoosting = false;
                boostEnd = (float)Math.Round(Time.time, 2);
            }
        } 
        else
        {
            if (rb.velocity.magnitude > 0) {
                digParticlesEmission.rateOverTime = baseDigEmissionRate;
                if (!digParticles.isPlaying) {
                    digParticles.Play();
                }
            } else {
                digParticles.Stop();
            }
            if (!isRetracting) {
                rb.velocity = moveDirection * moveSpeed;
            }
        } 
    }
}
