using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    Transform currentTarget;
    List<Transform> enemiesInRange = new List<Transform>();
    int currentTargetIndex = -1;
    bool isCycling = false;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTarget_Locator;
    [SerializeField] Animator cinemachineAnimator;

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")] [SerializeField] float maxNoticeAngle = 60;

    Transform cam;
    bool enemyLocked;
    float currentYOffset;
    Vector3 pos;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CycleOrResetTargets();
        }
        if (enemyLocked) {
            LookAtTarget();
        }
    }

    void CycleOrResetTargets()
    {
        if (!isCycling || currentTargetIndex == -1)
        {
            ScanAndUpdateTargets();
            isCycling = true;
        }
        else
        {
            CycleTargets();
        }
    }

    void ScanAndUpdateTargets()
    {
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        enemiesInRange.Clear();
        foreach (Collider target in nearbyTargets)
        {
            if (IsValidTarget(target))
            {
                enemiesInRange.Add(target.transform);
            }
        }
        if (enemiesInRange.Count > 0)
        {
            currentTargetIndex = 0;
            currentTarget = enemiesInRange[currentTargetIndex];
            FoundTarget();
        }
        else
        {
            ResetTarget();
        }
    }

    bool IsValidTarget(Collider target)
    {
        Vector3 dir = target.transform.position - cam.position;
        dir.y = 0;
        float angle = Vector3.Angle(cam.forward, dir);
        if (angle < maxNoticeAngle && !Blocked(target.transform.position))
        {
            return true;
        }
        return false;
    }

    void CycleTargets()
    {
        if (currentTargetIndex < enemiesInRange.Count - 1)
        {
            currentTargetIndex++;
            currentTarget = enemiesInRange[currentTargetIndex];
            FoundTarget();
        }
        else
        {
            ResetTarget();
            isCycling = false; // Stop cycling after one complete loop
        }
    }

    void FoundTarget()
    {
        cinemachineAnimator.Play("Target Camera");
        enemyLocked = true;
    }

    void ResetTarget()
    {
        currentTarget = null;
        enemyLocked = false;
        cinemachineAnimator.Play("Follow Camera");
    }

    bool Blocked(Vector3 targetPosition)
    {
        RaycastHit hit;
        if (Physics.Linecast(cam.position, targetPosition, out hit))
        {
            return !hit.transform.CompareTag("Enemy");
        }
        return false;
    }

    private void LookAtTarget()
    {
        if (currentTarget == null)
        {
            ResetTarget();
            return;
        }
        AdjustCameraFocus(currentTarget);
    }

    void AdjustCameraFocus(Transform target)
    {
        float height = target.GetComponent<CapsuleCollider>().height;
        float scale = target.localScale.y;
        float totalHeight = height * scale;
        float yOffset = totalHeight - (totalHeight / 4);
        if (zeroVert_Look && yOffset > 1.6f && yOffset < 1.6f * 3) 
            yOffset = 1.6f;
        pos = target.position + Vector3.up * yOffset;
        enemyTarget_Locator.position = pos;
        Vector3 dir = target.position - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * lookAtSmoothing);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, noticeZone);
    }
}
