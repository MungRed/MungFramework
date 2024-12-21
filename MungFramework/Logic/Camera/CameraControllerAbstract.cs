using Cinemachine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MungFramework.Logic.TimeCounter;
using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace MungFramework.Logic.Camera
{
    public abstract class CameraControllerAbstract : GameManagerAbstract
    {
        [Required("需要挂载摄像机")]
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        private CinemachineBasicMultiChannelPerlin  multiChannelPerlin;

        private CinemachineBasicMultiChannelPerlin MultiChannelPerlin
        {
            get
            {
                if (multiChannelPerlin == null)
                {
                    multiChannelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                }
                return multiChannelPerlin;
            }
        }

        [SerializeField]
        [Required("需要挂载跟随和注释Transform")]
        private Transform follow_Pos, lookAt_Pos;

        [SerializeField]
        [ReadOnly]
        private Transform follow_Bind, lookAt_Bind;

        [SerializeField]
        [ReadOnly]
        private bool follow_isBinding, lookAt_isBinding;

        [SerializeField]
        private bool isPause;

        public CameraSource GetCameraSource()
        {
            return new CameraSource(follow_Bind, lookAt_Bind);
        }

        public void Update()
        {
            if (follow_isBinding && follow_Bind != null)
            {
                follow_Pos.position = follow_Bind.position;
            }

            if (lookAt_isBinding && lookAt_Bind != null)
            {
                lookAt_Pos.position = lookAt_Bind.position;
            }
        }

        private TweenerCore<float, float, FloatOptions> setFovTweenCore;


        [Button]
        public void SetNoise(float value)
        {
            MultiChannelPerlin.m_AmplitudeGain = value;
        }

        [Button]
        public void SetNoise(float value, float time,UnityAction endCallback = null)
        {
            float nowValue = MultiChannelPerlin.m_AmplitudeGain;
            MultiChannelPerlin.m_AmplitudeGain = value;
            TimeCounterManager.StartTimeCounter(time, 0, null, () => { MultiChannelPerlin.m_AmplitudeGain = nowValue; endCallback?.Invoke(); });
        }

        [Button]
        public void SetNoise(float init, float target, float duration,UnityAction endCallback = null)
        {
            MultiChannelPerlin.m_AmplitudeGain = init;
            DOTween.To(() => MultiChannelPerlin.m_AmplitudeGain, x => MultiChannelPerlin.m_AmplitudeGain = x, target, duration).onComplete+= () => endCallback?.Invoke();
        }

        [Button]
        public void ResetNoise()
        {
            MultiChannelPerlin.m_AmplitudeGain = 0;
        }

        [Button]
        public void SetFov(int val, float time)
        {
            if (setFovTweenCore != null)
            {
                setFovTweenCore.Kill();
            }

            setFovTweenCore = DOTween
                .To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, val, time)
                .OnComplete(() => setFovTweenCore = null);
        }

        [Button]
        public void ResetFov(float time)
        {
            if (setFovTweenCore != null)
            {
                setFovTweenCore.Kill();
            }

            setFovTweenCore = DOTween
                .To(() => virtualCamera.m_Lens.FieldOfView, x => virtualCamera.m_Lens.FieldOfView = x, 60, time)
                .OnComplete(() => setFovTweenCore = null);
        }

        public override void OnGamePause(GameManagerAbstract parentManager)
        {
            base.OnGamePause(parentManager);
            if (setFovTweenCore != null)
            {
                setFovTweenCore.Pause();
            }
            follow_Pos.DOPause();
            lookAt_Pos.DOPause();
            isPause = true;
        }

        public override void OnGameResume(GameManagerAbstract parentManager)
        {
            base.OnGameResume(parentManager);
            if (setFovTweenCore != null)
            {
                setFovTweenCore.Play();
            }
            follow_Pos.DOPlay();
            lookAt_Pos.DOPlay();
            isPause = false;
        }

        public IEnumerator ChangeCameraSource(CameraSource cameraSource, float time)
        {
            var changeFollow = StartCoroutine(ChangeBindFollow(cameraSource.Follow, time));
            var changeLookAt = StartCoroutine(ChangeBindLookAt(cameraSource.LookAt, time));

            yield return changeFollow;
            yield return changeLookAt;
        }


        Coroutine bindfollow;
        public IEnumerator ChangeBindFollow(Transform aim, float time)
        {
            if (bindfollow != null)
            {
                StopCoroutine(bindfollow);
            }

            follow_isBinding = false;
            follow_Bind = aim;
            follow_Pos.DOKill();

            if (time <= Mathf.Epsilon)
            {
                follow_isBinding = true;
                yield break;
            }


            Coroutine thisCor;
            thisCor = bindfollow = StartCoroutine(MoveFollow(aim, time));
            yield return thisCor;
        }

        Coroutine bindlookat;
        public IEnumerator ChangeBindLookAt(Transform aim, float time)
        {
            //如果上一个还没结束，就停止
            if (bindlookat != null)
            {
                StopCoroutine(bindlookat);
            }

            lookAt_isBinding = false;
            lookAt_Bind = aim;
            lookAt_Pos.DOKill();

            if (time <= Mathf.Epsilon)
            {
                lookAt_isBinding = true;
                yield break;
            }

            Coroutine thisCor;
            thisCor = bindlookat = StartCoroutine(MoveLookAt(aim, time));
            yield return thisCor;
        }

        private IEnumerator MoveFollow(Transform aim, float time)
        {
            float nowTime = 0;
            var wait = new WaitForFixedUpdate();
            while (nowTime < time)
            {
                if (!isPause)
                {
                    float t = nowTime / time;
                    float smoothTime = Mathf.SmoothStep(0, 1, t);
                    follow_Pos.transform.position = Vector3.Lerp(follow_Pos.position, aim.position, smoothTime);
                    nowTime += Time.fixedDeltaTime;
                }
                yield return wait;
            }
            follow_isBinding = true;
        }

        private IEnumerator MoveLookAt(Transform aim, float time)
        {
            float nowTime = 0;
            var wait = new WaitForFixedUpdate();
            while (nowTime < time)
            {
                if (!isPause)
                {
                    float t = nowTime / time;
                    float smoothT = Mathf.SmoothStep(0, 1, t);
                    lookAt_Pos.transform.position = Vector3.Lerp(lookAt_Pos.position, aim.position, smoothT);
                    nowTime += Time.fixedDeltaTime;
                }
                yield return wait;
            }
            lookAt_isBinding = true;
        }
    }
}