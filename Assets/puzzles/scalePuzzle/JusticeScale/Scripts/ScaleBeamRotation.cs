using UnityEngine;

namespace JusticeScale.Scripts
{
    [RequireComponent(typeof(ScaleController))]
    public class ScaleBeamRotation : MonoBehaviour
    {
        [SerializeField] ScaleController scaleController;
        [SerializeField] Transform balanceBeam;
        [SerializeField] [Range(0,75)] float blendRotation = 15f;

        void Awake()
        {
            if (scaleController == null) 
                scaleController = GetComponent<ScaleController>();
        }

        // 버튼 클릭 시 이 함수를 호출
        public void Weigh()
        {
            // 총 질량 차이를 계산해 놓은 뒤
            float norm = scaleController.BalanceNormalized;
            float angle = Mathf.Lerp(-blendRotation, blendRotation, norm);
            balanceBeam.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}
