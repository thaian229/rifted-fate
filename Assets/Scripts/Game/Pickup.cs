using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class Pickup : MonoBehaviour
{
    [Tooltip("Rotation angle per second")] public float RotatingSpeed = 360f;
    [Tooltip("Sound played on pickup")] public AudioClip PickupSfx;
    public GameObject PickupVfx;
    public Rigidbody PickupRigidbody { get; private set; }

    Collider m_Collider;
    AudioSource m_AudioSource;
    bool m_HasPlayedFeedback;

    protected virtual void Start()
    {
        PickupRigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_AudioSource = GetComponent<AudioSource>();

        // ensure the physics setup is a kinematic rigidbody trigger
        PickupRigidbody.isKinematic = true;
        m_Collider.isTrigger = true;
    }

    void Update()
    {
        // Handle rotating
        transform.Rotate(Vector3.up, RotatingSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        PlayerMovement pickingPlayer = other.GetComponent<PlayerMovement>();

        if (pickingPlayer != null)
        {
            OnPicked(pickingPlayer);

            PickupEvent evt = Events.PickupEvent;
            evt.Pickup = gameObject;
            EventManager.Broadcast(evt);
        }
    }

    protected virtual void OnPicked(PlayerMovement playerController)
    {
        PlayPickupFeedback();
    }

    public void PlayPickupFeedback()
    {
        if (m_HasPlayedFeedback)
            return;

        if (PickupSfx != null && m_AudioSource != null)
        {
            m_AudioSource.PlayOneShot(PickupSfx);
        }

        if (PickupVfx != null)
        {
            GameObject vfx = Instantiate(PickupVfx, transform.position, transform.rotation);
            Destroy(vfx, 1.5f);
        }

        m_HasPlayedFeedback = true;
    }
}
