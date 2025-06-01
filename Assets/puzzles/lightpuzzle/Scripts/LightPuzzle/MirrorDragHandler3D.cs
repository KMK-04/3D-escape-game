using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class MirrorDragHandler3D : MonoBehaviour
{
    public Camera dragCamera;
    public LayerMask gridSlotLayer;

    //private Rigidbody rb;
    private Plane dragPlane;
    private Vector3 offset;
    private Vector3 originalPosition;

    void Awake()
    {
        //rb = GetComponent<Rigidbody>();
        originalPosition = transform.position;
    }

    void OnMouseDown()
    {
        if (dragCamera == null)
        {
            Debug.LogError("Drag Camera not assigned!");
            return;
        }

        dragPlane = new Plane(-dragCamera.transform.forward, transform.position);
        Ray ray = dragCamera.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            offset = transform.position - ray.GetPoint(enter);
        }

        //rb.isKinematic = true;
    }

    void OnMouseDrag()
    {
        Ray ray = dragCamera.ScreenPointToRay(Input.mousePosition);
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 worldPos = ray.GetPoint(enter) + offset;

            Vector3 vp = dragCamera.WorldToViewportPoint(worldPos);
            vp.x = Mathf.Clamp01(vp.x);
            vp.y = Mathf.Clamp01(vp.y);
            worldPos = dragCamera.ViewportToWorldPoint(new Vector3(vp.x, vp.y, vp.z));

            transform.position = worldPos;
        }
    }

    void OnMouseUp()
    {
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale * 0.5f, Quaternion.identity, gridSlotLayer);
        Collider closest = null;
        float minDistance = float.MaxValue;

        foreach (var col in hits)
        {
            if (!col.isTrigger) continue;

            float dist = Vector3.Distance(transform.position, col.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = col;
            }
        }

        bool snapped = false;
        if (closest != null)
        {
            transform.position = closest.transform.position;
            snapped = true;
        }
        else
        {
            transform.position = originalPosition;
        }

        if (snapped && TargetManager.Instance != null)
        {
            TargetManager.Instance.ResetTargets(false);
        }

        //rb.isKinematic = false;
    }

    public void ResetToOriginalPosition()
    {
        transform.position = originalPosition;
    }
    public void ResetLayer()
    {
        gameObject.layer = LayerMask.NameToLayer("Mirror");
    }
}
