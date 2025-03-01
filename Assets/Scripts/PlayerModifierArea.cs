using UnityEngine;

public class PlayerModifierArea : MonoBehaviour
{
    [SerializeField] private int _speedMultiplierModifier = 20;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IsometricMovementController>().SpeedMultiplier += _speedMultiplierModifier;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<IsometricMovementController>().SpeedMultiplier -= _speedMultiplierModifier;
        }
    }
}
