using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Teleportation : MonoBehaviour
{
    private GameObject objectToTeleport;
    private string currentTag = "WhiteBall";
    private List<GameObject> placesTeleported = new List<GameObject>();
    private GameObject objectToRotate;
    private bool canRotate = false;
    private bool canTeleport = true;
    private bool isAnimating = false;
    private bool isRotating = false;
    private bool isLifted = false;

    public Button rotateLeftButton;
    public Button rotateRightButton;
    public float rayCheckDuration = 0.1f;
    private Vector3 originalPosition;

    void Start()
    {
        rotateLeftButton.onClick.AddListener(() => RotateObject(-90));
        rotateRightButton.onClick.AddListener(() => RotateObject(90));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleMouseClick();
        }

        if (objectToTeleport != null)
        {
            UpdateTeleportationPosition();
        }
    }

    private void HandleMouseClick()
    {
        if (isAnimating || isRotating) return;

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;
            if (!objectToTeleport && canTeleport && !hitObject.GetComponent<TeleportFlag>() && hitObject.tag == currentTag)
            {
                objectToTeleport = hitObject;
                objectToTeleport.transform.position += new Vector3(0, 1.1f, 0.5f);
            }
            else if (objectToTeleport && canTeleport && hitObject.tag == "place" && !placesTeleported.Contains(hitObject))
            {
                CompleteTeleportation(hitObject);
            }
            else if (canRotate && hitObject.tag == "Square")
            {
                if (objectToRotate != null && objectToRotate != hitObject)
                {
                    LowerObject(objectToRotate, () => {
                        LiftObject(hitObject);
                        objectToRotate = hitObject;
                        originalPosition = objectToRotate.transform.position;
                    });
                }
                else if (objectToRotate == null)
                {
                    objectToRotate = hitObject;
                    originalPosition = objectToRotate.transform.position;
                    LiftObject(objectToRotate);
                }
                else if (objectToRotate == hitObject && !isLifted)
                {
                    LiftObject(objectToRotate);
                }
            }
        }
    }

    private void CompleteTeleportation(GameObject destination)
    {
        TeleportObject(objectToTeleport, destination);
        placesTeleported.Add(destination);
        canRotate = true;
        canTeleport = false;
        Invoke(nameof(StartRayCheck), 0.1f);
    }

    private void UpdateTeleportationPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, new Vector3(0, 1.1f, 0.6f));
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            objectToTeleport.transform.position = new Vector3(point.x, 1.1f, point.z + 0.6f);
        }
    }

    void TeleportObject(GameObject objToTeleport, GameObject destination)
    {
        if (objToTeleport != null)
        {
            objToTeleport.transform.position = destination.transform.position;
            objToTeleport.AddComponent<TeleportFlag>();
            objToTeleport.transform.parent = destination.transform.parent;
            Destroy(destination);
            objectToTeleport = null;
            currentTag = (currentTag == "WhiteBall") ? "BlackBall" : "WhiteBall";
        }
    }

    public void RotateObject(float angle)
    {
        if (canRotate && objectToRotate != null && !isRotating)
        {
            isRotating = true;
            isAnimating = true;
            Sequence rotateSequence = DOTween.Sequence();
            rotateSequence.Append(objectToRotate.transform.DORotate(new Vector3(0, objectToRotate.transform.eulerAngles.y + angle, 0), 0.5f))
                .Append(objectToRotate.transform.DOMoveY(originalPosition.y, 0.2f))
                .OnComplete(() =>
                {
                    objectToRotate.transform.eulerAngles = new Vector3(0, Mathf.Round(objectToRotate.transform.eulerAngles.y / 90) * 90, 0);
                    canRotate = false;
                    canTeleport = true;
                    objectToRotate = null;
                    isAnimating = false;
                    isRotating = false;
                    isLifted = false;
                    Invoke(nameof(StartRayCheck), 0.1f);
                    Invoke(nameof(CheckForNoWinners), 0.5f);
                });
        }
    }

    private void LiftObject(GameObject obj)
    {
        if (obj != null && !isLifted)
        {
            isLifted = true;
            obj.transform.DOMoveY(obj.transform.position.y + 0.646348f, 0.2f);
        }
    }

    private void LowerObject(GameObject obj, TweenCallback onComplete)
    {
        if (obj != null)
        {
            obj.transform.DOMoveY(originalPosition.y, 0.2f).OnComplete(() => {
                isLifted = false;
                onComplete?.Invoke();
            });
        }
    }

    void StartRayCheck()
    {
        foreach (raycast raycaster in FindObjectsOfType<raycast>())
        {
            raycaster.ActivateRayCheck(rayCheckDuration);
        }
    }

    void CheckForNoWinners()
    {
        FindObjectOfType<RaycastManager>().CheckForNoWinners();
    }
}

public class TeleportFlag : MonoBehaviour
{
}
