using System.Collections;
using UnityEngine;

public class minigamehard: MonoBehaviour
{
    private Vector3 initialPosition;
    public GameObject[] children ;
    private Coroutine movementRoutine;
    private Coroutine activateChildrenRoutine;

     void OnEnable()
    {
        initialPosition = transform.localPosition;
        children[0].SetActive(false);
        children[1].SetActive(false);
        if (movementRoutine == null)
        {
            movementRoutine = StartCoroutine(MovementCycle());
        }
    }

    IEnumerator MovementCycle()
    {
        while (true)
        {
            transform.localPosition = initialPosition;
            SetChildrenActive(false);

            float newX = initialPosition.x + Random.Range(0f, 4f);
            transform.localPosition = new Vector3(newX, initialPosition.y, initialPosition.z);

            // 여기에서도 activateChildrenRoutine 저장
            activateChildrenRoutine = StartCoroutine(ActivateChildren());
            yield return new WaitForSeconds(3f);
        }
    }
    IEnumerator ActivateChildren()
    {
        children[0].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        children[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        children[0].SetActive(false);
        children[1].SetActive(false);
    }

    void SetChildrenActive(bool state)
    {
        foreach (var child in children)
        {
            child.SetActive(state);
        }
    }
    public void StopAllSequences()
    {
        if (movementRoutine != null)
        {
            StopCoroutine(movementRoutine);
            movementRoutine = null;
        }

        if (activateChildrenRoutine != null)
        {
            StopCoroutine(activateChildrenRoutine);
            activateChildrenRoutine = null;
        }

        SetChildrenActive(false);
        transform.localPosition = initialPosition;
    }

}
