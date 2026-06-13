using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class FurniturePlacement : MonoBehaviour
{
    [Header("Furniture Prefabs")]
    public GameObject[] furniturePrefabs;

    [Header("AR References")]
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;

    [Header("Settings")]
    public float spawnDistance = 1.5f;  // jarak dari kamera
    public float spawnHeight = -0.5f;   // tinggi (negatif = di bawah kamera)
    public float minScale = 0.2f;
    public float maxScale = 4f;
    public float rotateSpeed = 0.5f;
    public float doubleTapTime = 0.3f;
    public bool useARPlane = false;     // false = spawn di depan kamera

    private GameObject spawnedObject;
    private int currentIndex = 0;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    // Pinch & rotate
    private float prevPinchDist = 0f;
    private float prevRotAngle = 0f;
    private bool twoFingerActive = false;

    // Double tap
    private float lastTapTime = 0f;

    void Update()
    {
        if (Input.touchCount == 0)
        {
            twoFingerActive = false;
            return;
        }

        // ===== 1 JARI =====
        if (Input.touchCount == 1)
        {
            twoFingerActive = false;
            Touch touch = Input.GetTouch(0);

            if (IsPointerOverUI(touch.fingerId)) return;

            if (touch.phase == TouchPhase.Began)
            {
                float now = Time.time;
                bool isDoubleTap = (now - lastTapTime) < doubleTapTime;
                lastTapTime = now;

                if (spawnedObject == null)
                {
                    // SPAWN FURNITURE
                    SpawnFurniture();
                }
                else if (isDoubleTap)
                {
                    // DOUBLE TAP = pindah posisi
                    MoveFurniture();
                }
            }
        }

        // ===== 2 JARI — PINCH + ROTATE =====
        else if (Input.touchCount == 2)
        {
            if (spawnedObject == null) return;

            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float curDist = Vector2.Distance(t0.position, t1.position);
            float curAngle = Mathf.Atan2(t1.position.y - t0.position.y,
                                         t1.position.x - t0.position.x) * Mathf.Rad2Deg;

            if (!twoFingerActive)
            {
                prevPinchDist = curDist;
                prevRotAngle = curAngle;
                twoFingerActive = true;
                return;
            }

            // SCALE
            if (prevPinchDist > 0)
            {
                float factor = curDist / prevPinchDist;
                float s = spawnedObject.transform.localScale.x * factor;
                s = Mathf.Clamp(s, minScale, maxScale);
                spawnedObject.transform.localScale = new Vector3(s, s, s);
            }

            // ROTATE
            float angleDelta = Mathf.DeltaAngle(prevRotAngle, curAngle);
            spawnedObject.transform.Rotate(0f, -angleDelta * rotateSpeed, 0f, Space.World);

            prevPinchDist = curDist;
            prevRotAngle = curAngle;
        }
    }

    void SpawnFurniture()
    {
        if (useARPlane)
        {
            // Mode AR Plane (butuh deteksi lantai)
            Touch touch = Input.GetTouch(0);
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = hits[0].pose;
                spawnedObject = Instantiate(furniturePrefabs[currentIndex],
                    pose.position, pose.rotation);
            }
        }
        else
        {
            // Mode depan kamera (langsung muncul)
            Camera cam = Camera.main;
            Vector3 spawnPos = cam.transform.position
                + cam.transform.forward * spawnDistance;
            spawnPos.y += spawnHeight;

            Quaternion spawnRot = Quaternion.Euler(0,
                cam.transform.eulerAngles.y, 0);

            spawnedObject = Instantiate(furniturePrefabs[currentIndex],
                spawnPos, spawnRot);
        }
    }

    void MoveFurniture()
    {
        if (useARPlane)
        {
            Touch touch = Input.GetTouch(0);
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                spawnedObject.transform.position = hits[0].pose.position;
            }
        }
        else
        {
            // Pindah ke depan kamera
            Camera cam = Camera.main;
            Vector3 newPos = cam.transform.position
                + cam.transform.forward * spawnDistance;
            newPos.y += spawnHeight;
            spawnedObject.transform.position = newPos;
        }
    }

    public void ChangeFurniture(int index)
    {
        if (index < 0 || index >= furniturePrefabs.Length) return;
        currentIndex = index;

        if (spawnedObject != null)
        {
            Vector3 pos = spawnedObject.transform.position;
            Quaternion rot = spawnedObject.transform.rotation;
            Vector3 scale = spawnedObject.transform.localScale;
            Destroy(spawnedObject);
            spawnedObject = Instantiate(furniturePrefabs[currentIndex], pos, rot);
            spawnedObject.transform.localScale = scale;
        }
    }

    public void RotateFurniture()
    {
        if (spawnedObject != null)
            spawnedObject.transform.Rotate(0, 45, 0, Space.World);
    }

    public void DeleteFurniture()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }

    public void ResetScale()
    {
        if (spawnedObject != null)
            spawnedObject.transform.localScale = Vector3.one;
    }

    bool IsPointerOverUI(int fingerId)
    {
        return EventSystem.current != null &&
               EventSystem.current.IsPointerOverGameObject(fingerId);
    }
}