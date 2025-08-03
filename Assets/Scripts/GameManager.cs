using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private float secondsToWaitBeforeDeathCheck = 3f;
    [SerializeField] private GameObject restartScreenObject;
    [SerializeField] private SlingShotHandler slingShotHandler;

    public int MaxNumberOfShots = 3;
    private int usedNumberOfShots;
    private IconHandler iconHandler;
    private List<Baddie> baddies = new List<Baddie>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        iconHandler = FindFirstObjectByType<IconHandler>();
        Baddie[] _baddies = FindObjectsByType<Baddie>(FindObjectsSortMode.None);
        for (int i = 0; i < _baddies.Length; i++)
        {
            baddies.Add(_baddies[i]);
        }
    }

    public void UseShot()
    {
        usedNumberOfShots++;
        iconHandler.UseShot(usedNumberOfShots);
        CheckForLastShot();
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

    public void CheckForLastShot()
    { 
        if (usedNumberOfShots == MaxNumberOfShots)
        {
            StartCoroutine(CheckAfterWaitTime());
        }
    }

    private IEnumerator CheckAfterWaitTime() 
    { 
        yield return new WaitForSeconds(secondsToWaitBeforeDeathCheck);
        if (baddies.Count == 0)
        {
            WinGame();
        }
        else
        { 
            RestartGame();
        }

    }

    public void RemoveBaddie(Baddie baddie)
    {
        baddies.Remove(baddie);
        CheckForAllDeadBaddies();
    }

    private void CheckForAllDeadBaddies()
    {
        if (baddies.Count == 0)
        {
            WinGame();
        }
    }

    #region Win/Lose

    private void WinGame()
    {
       restartScreenObject.SetActive(true);
       slingShotHandler.enabled = false;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    #endregion
}
