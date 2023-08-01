using UnityEngine;

public class ChangeSprite : MonoBehaviour
{
    public Sprite StunGunSprite;
    public Sprite SuitSprite;
    public RuntimeAnimatorController newAnimatorController;
    public RuntimeAnimatorController secondAnimator;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public InteractionSystem interactionSystem;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("StunGun") || interactionSystem.pickUpStunGun)
        {
            spriteRenderer.sprite = StunGunSprite;
            animator.runtimeAnimatorController = newAnimatorController;
        }

        if (other.gameObject.CompareTag("Suit") || InteractionSystem.pickUpSuit)
        {
            Debug.Log("SuitUP");
            spriteRenderer.sprite = SuitSprite;
            animator.runtimeAnimatorController = secondAnimator;
        }
    }
}
