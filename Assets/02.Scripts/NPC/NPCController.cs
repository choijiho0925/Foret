using DG.Tweening;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public void MoveNpc()
    {
        transform.DOMoveX(3.0f, 3.0f);
    }

    public void Attack()
    {
        
    }

    public void Heal()
    {
        
    }
}
