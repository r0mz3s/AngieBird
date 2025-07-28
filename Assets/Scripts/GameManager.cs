using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int MaxNumberOfShots = 3;
    private int usedNumberOfShots;
    private IconHandler iconHandler;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        iconHandler = FindFirstObjectByType<IconHandler>();
    }

    public void UseShot()
    {
        usedNumberOfShots++;
        iconHandler.UseShot(usedNumberOfShots);
    }

    public bool HasEnoughShots()
    {
        if (usedNumberOfShots < MaxNumberOfShots)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
