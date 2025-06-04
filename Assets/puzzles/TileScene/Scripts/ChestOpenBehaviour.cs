using UnityEngine;

public class ChestOpenBehaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponent<ChestController>().OnChestOpened();
    }
}
