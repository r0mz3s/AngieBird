using UnityEngine;
using UnityEngine.InputSystem;

public class SlingShotArea : MonoBehaviour
{

    [SerializeField] private LayerMask slingshotLayerMask;

    public bool IsWithinSlingshotArea() 
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (Physics2D.OverlapPoint(worldPosition, slingshotLayerMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
