using UnityEngine;

public class NPCController : MonoBehaviour
{
    protected virtual void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            //UI에 상호작용 키 뜨는 함수
        }
    }
    
    
}
