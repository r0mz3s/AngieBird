using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using DG.Tweening;

public class SlingShotHandler : MonoBehaviour
{
    [Header("Line Renderers")]
    [SerializeField] private LineRenderer leftLineRenderer;
    [SerializeField] private LineRenderer rightLineRenderer;
    [Header("Transform References")]
    [SerializeField] private Transform leftStartPosition;
    [SerializeField] private Transform rightStartPosition;
    [SerializeField] private Transform centerPosition;
    [SerializeField] private Transform idlePosition;
    [SerializeField] private Transform elasticTransform;
    [Header("Slingshot Stats")]
    [SerializeField] private float maxDistance = 7f;
    [SerializeField] private float shotForce = 8f;
    [SerializeField] private float timeBetweenBirdRespawns = 2f;
    [SerializeField] private float elasticDivider = 17f;
    [Header("Scripts")]
    [SerializeField] private SlingShotArea slingshotArea;
    [Header("Bird")]
    [SerializeField] private AngieBird angieBirdPrefab;
    [SerializeField] private float angieBirdPositionOffset = 0.275f;

    private Vector2 slingShotLinesPosition;
    private bool clickedWithinArea;
    private AngieBird spawnedAngieBird;
    private Vector2 direction;
    private Vector2 directionNormalized;
    private bool birdOnSlinghot;


    private void Awake()
    {
        leftLineRenderer.enabled = false;
        rightLineRenderer.enabled = false;

        SpawnAngieBird();
    }

    void Update()
    {
        if (InputManager.WasLeftMouseButtonPressed && slingshotArea.IsWithinSlingshotArea())
        {
            clickedWithinArea = true;
        }

        if (InputManager.IsLeftMousePressed && clickedWithinArea && birdOnSlinghot)
        {
            DrawSlingShot();
            PositionAndRotateAngieBird();
        }

        if (InputManager.WasLeftMouseButtonReleased && birdOnSlinghot && clickedWithinArea)
        {
            if (GameManager.Instance.HasEnoughShots())
            {
                clickedWithinArea = false;
                birdOnSlinghot = false;
                spawnedAngieBird.LaunchBird(direction, shotForce);
                GameManager.Instance.UseShot();
                AnimateSlingShot();
                if (GameManager.Instance.HasEnoughShots()) 
                { 
                    StartCoroutine(SpawnAngieBirdAfterTime(timeBetweenBirdRespawns));
                }
            }
            
        }   
    }

    #region ======== SlingShot Methods ========

    private void DrawSlingShot()
    {
        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(InputManager.MousePosition);
        slingShotLinesPosition = centerPosition.position + Vector3.ClampMagnitude(touchPosition - centerPosition.position, maxDistance);
        SetLines(slingShotLinesPosition);
        direction = (Vector2)centerPosition.position - slingShotLinesPosition;
        directionNormalized = direction.normalized;
    }

    private void SetLines(Vector2 position)
    {
        if (!leftLineRenderer.enabled && !rightLineRenderer.enabled)
        {
            leftLineRenderer.enabled = true;
            rightLineRenderer.enabled = true;
        }

        leftLineRenderer.SetPosition(0, position);
        leftLineRenderer.SetPosition(1, leftStartPosition.position);

        rightLineRenderer.SetPosition(0, position);
        rightLineRenderer.SetPosition(1, rightStartPosition.position);

    }

    #endregion

    #region ======== Angie Bird Methods ========

    private void SpawnAngieBird()
    { 
        SetLines(idlePosition.position);
        Vector2 dir = (centerPosition.position - idlePosition.position).normalized;
        Vector2 spawnPosition = (Vector2)idlePosition.position + dir * angieBirdPositionOffset;
        spawnedAngieBird = Instantiate(angieBirdPrefab, spawnPosition, Quaternion.identity);
        spawnedAngieBird.transform.right = dir;
        birdOnSlinghot = true;
    }
    
    private void PositionAndRotateAngieBird() 
    {
        spawnedAngieBird.transform.position = slingShotLinesPosition + directionNormalized * angieBirdPositionOffset;
        spawnedAngieBird.transform.right = directionNormalized;
    }

    private IEnumerator SpawnAngieBirdAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        SpawnAngieBird();
    }


    #endregion

    #region ======= Animate SlingShot ========

    private void AnimateSlingShot() 
    {    
        elasticTransform.position = leftLineRenderer.GetPosition(0);
        float dist = Vector2.Distance(elasticTransform.position, centerPosition.position);
        float time = dist / elasticDivider;
        elasticTransform.DOMove(centerPosition.position, time);
        StartCoroutine(AnimateSlingShotLines(elasticTransform, time));
    }

    private IEnumerator AnimateSlingShotLines(Transform trans, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.deltaTime;
            SetLines(trans.position);
            yield return null;
        }
    }

    #endregion
}
