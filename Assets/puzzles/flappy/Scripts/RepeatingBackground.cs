using UnityEngine;
using UnityEngine.UI;

public class RepeatingBackground : MonoBehaviour
{
    private BoxCollider2D groundCollider;
    private float groundHorizontalLength;

    private void Start() {
        groundCollider = GetComponent<BoxCollider2D>();
        groundHorizontalLength = groundCollider.size.x;
    }

    private void Update() {
       if(transform.position.x < -groundHorizontalLength) {
            transform.position = new Vector2(groundHorizontalLength, transform.position.y);

        }
    }
}