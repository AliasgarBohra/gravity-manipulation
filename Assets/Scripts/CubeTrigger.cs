using UnityEngine;

public class CubeTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance?.AddPoint();
            gameObject.SetActive(false);
        }
    }
}