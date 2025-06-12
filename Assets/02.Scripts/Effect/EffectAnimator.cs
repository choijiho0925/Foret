using UnityEngine;

public class EffectAnimator : MonoBehaviour
{
    private Animator animator;
    private static readonly int animIDPlayEffect = Animator.StringToHash("PlayEffect");

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

  //활성화 될 때마다 1회용으로 이팩트 애니메이션을 보여줌
    
    private void OnEnable()
    {
        if (animator != null)
        {
            animator.SetTrigger(animIDPlayEffect);
        }
    }
}