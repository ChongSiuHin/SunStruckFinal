using UnityEngine;

public class ImageAnimator : MonoBehaviour
{
    private Animator animator;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        PlayAnimation();
        animator.SetBool("PlayPause",true);
    }

    public void PlayAnimation()
    {
        animator.SetTrigger("PlayAnimation");
    }

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
