using UnityEngine;

public class Column : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.GetComponent<Bird>() != null) {
            GameControl.instance.BirdScored();
        }
    }
}
