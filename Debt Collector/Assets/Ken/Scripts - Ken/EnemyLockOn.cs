using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLockOn : MonoBehaviour
{
    private Transform currentTarget; // transform of locked on target
    private List<Transform> targetsInRange = new List<Transform>();
    private int currentTargetIndex;
    // Animator anim;

    [SerializeField] LayerMask targetLayers;
    [SerializeField] Transform enemyTarget_Locator;
    [SerializeField] Animator cinemachineAnimator;
    

    [Header("Settings")]
    [SerializeField] bool zeroVert_Look;
    [SerializeField] float noticeZone = 10;
    [SerializeField] float lookAtSmoothing = 2;
    [Tooltip("Angle_Degree")] [SerializeField] float maxNoticeAngle = 60;
    // [SerializeField] float crossHair_Scale = 0.1f;
    
    [Header("Character Animation")]
    public Animator characterAnimator;
    private String lockedOnField = "LockedOn";
    
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
        // camFollow.lockedTarget = enemyLocked;
        // defMovement.lockMovement = enemyLocked;
        if (Input.GetKeyDown(KeyCode.Tab)){
            if (currentTarget != null){
                //If there is already a target, Reset.
                ResetTarget();
                return;
            }
            
            if (currentTarget = ScanNearBy()) FoundTarget(); else ResetTarget();
        }

        if (Input.GetKeyDown(KeyCode.F)){
            Debug.Log(currentTargetIndex);
            if(targetsInRange.Count > 0){
                currentTarget = targetsInRange[(currentTargetIndex + 1) % targetsInRange.Count];
                currentTargetIndex++;
            }
        }

        if (enemyLocked) {
            LookAtTarget();
        }
        characterAnimator.SetBool(lockedOnField, enemyLocked);
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
        Collider[] nearbyTargets = Physics.OverlapSphere(transform.position, noticeZone, targetLayers);
        float closestAngle = maxNoticeAngle;
        Transform closestTarget = null;
        if (nearbyTargets.Length <= 0) return null;

        // clear the list or array
        targetsInRange.Clear();
        for (int i = 0; i < nearbyTargets.Length; i++)
        {
            Vector3 dir = nearbyTargets[i].transform.position - cam.position;
            dir.y = 0;
            float _angle = Vector3.Angle(cam.forward, dir);
            
            // add the near nearbyTargets[i].transform to the list 
            targetsInRange.Add(nearbyTargets[i].transform);

            if (_angle < closestAngle)
            {
                closestTarget = nearbyTargets[i].transform;
                // mark the currentTargetIndex as i
                currentTargetIndex = i;
                closestAngle = _angle;      
            }
        }

        if (!closestTarget ) return null;
        float h1 = closestTarget.GetComponent<CapsuleCollider>().height;
        float h2 = closestTarget.localScale.y;
        float h = h1 * h2;
        float half_h = (h / 2) / 2;
        currentYOffset = h - half_h;
        if(zeroVert_Look && currentYOffset > 1.6f && currentYOffset < 1.6f * 3) currentYOffset = 1.6f;
        Vector3 tarPos = closestTarget.position + new Vector3(0, currentYOffset, 0);
        if(Blocked(tarPos)) return null;
        // Debug.Log(closestTarget.position);

        // return the transform of the closest target
        return closestTarget;
    }

    bool Blocked(Vector3 t){
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
        pos = currentTarget.position + new Vector3(0, currentYOffset - 1, 0);
        // lockOnCanvas.position = pos;
        // lockOnCanvas.localScale = Vector3.one * ((cam.position - pos).magnitude * crossHair_Scale);
        // Debug.Log("pos: " + pos);
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