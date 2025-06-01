using UnityEngine;

public class LineController : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Material lineMaterial;
    public Transform startPoint;
    public float maxLength = 100f;
    public int maxReflections = 10;

    public enum Direction
    {
        Up,    // Z+
        Down,  // Z-
        Left,  // X-
        Right  // X+
    }

    public Direction initialDirection;

    public LayerMask raycastMask;

    // 새로 추가된 변수: 검사할 타겟만 지정
    public LaserTarget target;

    public Vector3 GetDirectionVector(Direction dir)
    {
        switch (dir)
        {
            case Direction.Up: return Vector3.forward;
            case Direction.Down: return Vector3.back;
            case Direction.Left: return Vector3.left;
            case Direction.Right: return Vector3.right;
            default: return Vector3.right;
        }
    }

    void Start()
    {
        if (lineRenderer != null && lineMaterial != null)
        {
            lineRenderer.material = lineMaterial;
        }

        Vector3 direction = GetDirectionVector(initialDirection);
        CastLaser(startPoint.position, direction);
    }

    public void CastLaser(Vector3 startPos, Vector3 direction)
    {
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, startPos);

        Vector3 currentPosition = startPos;
        Vector3 currentDirection = direction.normalized;

        for (int i = 0; i < maxReflections; i++)
        {
            Ray ray = new Ray(currentPosition, currentDirection);
            if (Physics.Raycast(ray, out RaycastHit hit, maxLength, raycastMask, QueryTriggerInteraction.Collide))
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i + 1, hit.point);

                // 맞은 콜라이더가 지정한 타겟과 같을 때만 처리
                LaserTarget hitTarget = hit.collider.GetComponent<LaserTarget>();
                if (hitTarget != null && hitTarget == target)
                {
                    Debug.Log(target.name);
                    target.OnLaserHit();
                    break;
                }

                if (hit.collider.CompareTag("Mirror"))
                {
                    currentPosition = hit.point;
                    currentDirection = Vector3.Reflect(currentDirection, hit.normal);
                    currentDirection.y = 0;
                    currentDirection.Normalize();
                }
                else
                {
                    break;
                }
            }
            else
            {
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(i + 1, currentPosition + currentDirection * maxLength);
                break;
            }
        }
    }
}
