using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("SETTINGS")]
    public bool isMoving;
    [SerializeField] private float moveTime;
    [SerializeField] private float lineRendererThreshhold;
    [SerializeField] private float turnSmoothTimeMultiplayer;
    [SerializeField] private LayerMask pathLayerMask;
    private const float floorOffset = 0.1f;

    [Header("REFERENCES")]
    [SerializeField] private Player player;
    [SerializeField] private Transform endPoint;

    private LineRenderer lineRenderer;

    private List<Vector3> points = new List<Vector3>();
    private Vector3[] path;
    private Vector3 startPosition;

    private bool startOnPlayer;
    private bool firstTap;
    private bool pathSetted;
    private bool canDraw = true;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();

        startPosition = player.transform.position;
    }

    private void Update()
    {
        CreatePath();
    }

    private void CreatePath()
    {
        if (canDraw)
        {
            if (Input.GetMouseButtonDown(0))
            {
                points.Clear();
                firstTap = true;
            }

            if (Input.GetMouseButton(0) && (startOnPlayer || firstTap))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, pathLayerMask))
                {
                    if (firstTap)
                    {
                        firstTap = false;
                        if (hit.collider.transform == player.transform)
                        {
                            startOnPlayer = true;
                        }
                        else
                        {
                            startOnPlayer = false;
                            return;
                        }
                    }

                    if (DistanceToLastPoint(hit.point) > lineRendererThreshhold)
                    {
                        points.Add(new Vector3(hit.point.x, floorOffset, hit.point.z));

                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPositions(points.ToArray());
                    }

                    if (hit.collider.tag == "Finish")
                    {
                        OnPathCreated();
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0) && startOnPlayer)
            {
                OnPathCreated();
            }
        }
    }

    private void OnPathCreated()
    {
        lineRenderer.Simplify(lineRendererThreshhold);

        startOnPlayer = false;
        SetPath();
    }

    private void SetPath()
    {
        path = new Vector3[points.Count];

        for(int i = 1; i < points.Count; i++)
        {
            path[i] = new Vector3(points[i].x, 0f, points[i].z);
        }
        path[0] = player.transform.position;

        pathSetted = true;
    }

    public void ResetPath()
    {
        StopAllCoroutines();

        player.transform.position = startPosition;
        player.transform.rotation = Quaternion.identity;
        player.ResetAnimations();

        lineRenderer.positionCount = 0;

        points.Clear();
        path = new Vector3[0];

        startOnPlayer = false;
        firstTap = false;
        pathSetted = false;
        isMoving = false;
        canDraw = true;

        player.isDefeated = false;
        player.isFinished = false;
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any())
            return Mathf.Infinity;

        return Vector3.Distance(points.Last(), point);
    }

    public void StartMove()
    {
        StartCoroutine(MovePlayer());
    }

    private IEnumerator MovePlayer()
    {
        isMoving = true;
        canDraw = false;

        float value = moveTime;
        if (pathSetted)
        {
            while (value >= 0 && !player.isDefeated)
            {
                if (player.isFinished || player.isDefeated)
                {
                    break;
                }

                iTween.PutOnPath(player.transform, path, 1 - (value / moveTime));
                value -= Time.deltaTime;

                yield return new WaitForEndOfFrame();


                iTween.LookUpdate(player.gameObject, iTween.PointOnPath(path, 1 - (value / moveTime)), Time.deltaTime * turnSmoothTimeMultiplayer);
            }

            pathSetted = false;
        }

        GameController.Instance.CheckFinish();

        isMoving = false;

        yield return null;
    }

    private void OnDrawGizmos()
    {
        if(pathSetted)
            iTween.DrawPath(path, Color.white);
    }
}
