using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("SETTINGS")]
    public bool isFinished;
    public bool isDefeated;

    [Header("REFERENCES")]
    [SerializeField] private Transform endPoint;
    private PlayerController parent;
    private Animator animator;

    private void Awake()
    {
        parent = GetComponentInParent<PlayerController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator)
        {
            animator.SetBool("isMoving", parent.isMoving);
            animator.SetBool("isFinished", isFinished);
            animator.SetBool("isDefeated", isDefeated);
        }
    }

    public void ResetAnimations()
    {
        if(animator)
            animator.Play("Idle");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform == endPoint && !isDefeated)
        {
            isFinished = true;
            return;
        }

        if (collision.transform.tag == "Player" || collision.transform.tag == "Obstacle")
        {
            isDefeated = true;
            isFinished = false;
        }
    }
}
