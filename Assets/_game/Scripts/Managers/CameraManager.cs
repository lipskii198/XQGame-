using System.Collections;
using _game.Scripts.Utility;
using Cinemachine;
using UnityEngine;

namespace _game.Scripts.Managers
{
    public class CameraManager : LazySingletonMono<CameraManager>
    {

        [SerializeField] private CinemachineVirtualCamera[] _allVirtualCameras;

        [Header("Controls for lerping y damping during player jump\fall")]
        [SerializeField] private float _fallPanAmount = 0.25f;
        [SerializeField] private float _fallYPanTime = 0.35f;
        public float _fallSpeadYDampingChangeThreshold = -15f;

        public bool isLerpingYDamping { get; private set; }
        public bool lerpedFromPlayerFalling { get; set; }

        private Coroutine _lerpYPancoroutine;

        private CinemachineVirtualCamera currentCamera;
        private CinemachineFramingTransposer framingTransposer;

        private float normYPanAmount;

        private void Awake()
        {

            for (int i = 0; i < _allVirtualCameras.Length; i++) {
                if (_allVirtualCameras[i].enabled) {
                    currentCamera = _allVirtualCameras[i];

                    framingTransposer = currentCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
                }
            }
            normYPanAmount = framingTransposer.m_YDamping;
        }

        #region lerp y damping

        public void LerpYDamping(bool isPlayerFalling) {
            _lerpYPancoroutine = StartCoroutine(LerpYAction(isPlayerFalling));
        }

        private IEnumerator LerpYAction(bool isPlayerFalling) {
            isLerpingYDamping = true;

            //starting damp amount
            float startDampAmount = framingTransposer.m_YDamping;
            float endDampAmount = 0f;
            //end damping amount
            if (isPlayerFalling) {
                endDampAmount = _fallPanAmount;
                lerpedFromPlayerFalling = true;
            } else {
                endDampAmount = normYPanAmount;
            }

            float elapsedTime = 0f;
            while (elapsedTime < _fallYPanTime) {
            
                elapsedTime += Time.deltaTime;

                float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallYPanTime);
                framingTransposer.m_YDamping = lerpedPanAmount;

                yield return null;
            }
            isLerpingYDamping = false;

        }

        #endregion
    }
}
