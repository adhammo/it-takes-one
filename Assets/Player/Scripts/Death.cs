using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
public class Death : MonoBehaviour
{
    [Header("Stats")]
    [Tooltip("Player max health")]
    public float MaxHealth = 100.0f;

    [Header("Disable")]
    [Tooltip("Scripts to disable")]
    public MonoBehaviour[] DisabledScripts;

    [Header("Hurt")]
    [Tooltip("Hurt UI object")]
    public Hurt HurtUI;

    [Header("Animation")]
    [Tooltip("Object to fall")]
    public GameObject DeathGameObject;
    [Tooltip("Model to hide")]
    public GameObject DeathModel;

    [Header("Audios")]
    [Tooltip("Hurt audio")]
    public AudioClip HurtAudio;
    [Tooltip("Death audio")]
    public AudioClip DeathAudio;

    [SerializeField]
    [Tooltip("Player current health")]
    private float _currentHealth;

    public float CurrentHealth { get { return _currentHealth; } }

    private PlayerInput _playerInput;
    private CharacterController _controller;
    private AudioSource _audioSource;
    private Collider _deathCollider;
    private Rigidbody _deathRigidbody;

    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _controller = GetComponent<CharacterController>();
        _audioSource = GetComponent<AudioSource>();
        _deathCollider = DeathGameObject.GetComponent<Collider>();
        _deathRigidbody = DeathGameObject.GetComponent<Rigidbody>();

        _currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        _audioSource.PlayOneShot(HurtAudio);
        HurtUI.GetHurt();

        _currentHealth -= damage;
        if (_currentHealth <= 0.0f)
        {
            Die();
        }
    }

    public void Reset()
    {
        foreach (var script in DisabledScripts)
        {
            script.enabled = true;
        }

        DeathModel.SetActive(true);
        _playerInput.enabled = true;
        _controller.enabled = true;
        _deathCollider.enabled = false;
        _deathRigidbody.useGravity = false;
        _deathRigidbody.isKinematic = true;

        _currentHealth = MaxHealth;
    }

    void Die()
    {
        _audioSource.PlayOneShot(DeathAudio);

        foreach (var script in DisabledScripts)
        {
            script.enabled = false;
        }

        DeathModel.SetActive(false);
        _playerInput.enabled = false;
        _controller.enabled = false;
        _deathCollider.enabled = true;
        _deathRigidbody.useGravity = true;
        _deathRigidbody.isKinematic = false;
    }
}
