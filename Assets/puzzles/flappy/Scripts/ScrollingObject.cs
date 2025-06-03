using UnityEngine;

public class ScrollingObject : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.linearVelocity = new Vector2(GameControl.instance.scrollSpeed, 0);
    }

    private void Update() {
        if(GameControl.instance.isGameover == true) {
            rb2d.linearVelocity = Vector2.zero;
        }
    }
}