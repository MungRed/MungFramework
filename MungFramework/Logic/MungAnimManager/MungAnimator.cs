using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace MungFramework.Logic.Anim
{
    public class MungAnimator : MungFramework.Entity.Entity
    {
        private MungAnimatorController _animController;
        private MungAnimatorController animController
        {
            get
            {
                if (_animController == null)
                {
                    _animController = MungAnimatorController.Instance;
                }
                return _animController;
            }
        }

        [SerializeField]
        private List<SpriteRenderer> spriteRendererList = new();
        [SerializeField]
        private List<Image> imageList = new();

        [Serializable]
        public class MungAnimState
        {
            public string stateName;
            public MungAnimationAssets animAssets;
        }

        [SerializeField]
        private List<MungAnimState> animStateList;

        [SerializeField]
        [FoldoutGroup("Status")]
        private string nowStateName; //当前状态
        [SerializeField]
        [FoldoutGroup("Status")]
        private int nowFrame; //当前帧
        [SerializeField]
        [FoldoutGroup("Status")]
        private int nowFrameCount; //当前帧计数
        [SerializeField]
        [FoldoutGroup("Status")]
        private bool isPlay;
        [SerializeField]
        [FoldoutGroup("Status")]
        private float nowTime;


        [SerializeField]
        [FoldoutGroup("Setting")]
        private bool loop; //是否循环
        [SerializeField]
        [FoldoutGroup("Setting")]
        private bool playOnStart; //开始播放
        [SerializeField]
        [FoldoutGroup("Setting")]
        private bool useMungLifeCycle;  //是否使用Mung生命周期
        [SerializeField]
        [FoldoutGroup("Setting")]
        private float frame;




        public bool UseMungLifeCycle=>useMungLifeCycle;

        public void AddTime(float deltaTime)
        {
            if (isPlay)
            {
                float frameTime = 1 / frame;
                nowTime += deltaTime;
                if (nowTime > frameTime)
                {
                    nowTime -= frameTime;
                    NextFrame();
                }
            }
        }

        /// <summary>
        /// 改变状态
        /// </summary>
        public void ChangeState(string stateName)
        {
            nowStateName = stateName;
            nowFrame = 0;
            nowFrameCount = 0;
            nowTime = 0;
        }




        /// <summary>
        /// 下一帧
        /// </summary>
        private void NextFrame()
        {
            var asstes = animStateList.Find(x => x.stateName == nowStateName).animAssets;
            if (asstes == null||asstes.GetSize()==0)
            {
                return;
            }
            //如果超出总帧数，根据是否循环来判断是否重第0帧开始
            if (nowFrame >= asstes.GetSize())
            {
                if (loop)
                {
                    nowFrame = 0;
                }
                else
                {
                    isPlay = false;
                    return;
                }
            }

            //播放当前帧
            var nowframe = asstes.GetFrame(nowFrame);
            foreach (var spriteRender in spriteRendererList)
            {
                spriteRender.sprite = nowframe.Sprite;
            }
            foreach (var image in imageList)
            {
                image.sprite = nowframe.Sprite;
            }
            //当前帧计数+1
            nowFrameCount++;
            //如果当前帧计数超过当前帧持续帧数，下一次应该播放下一帧
            if (nowFrameCount > nowframe.DurationFrame)
            {
                nowFrame++;
                nowFrameCount = 0;
            }
        }






        [Button("Start")]
        private void StartPlayTest()
        {
            StartPlay(true);
        }

        public void StartPlay(bool restart)
        {
            isPlay = true;
            if (restart)
            {
                nowFrame = 0;
                nowFrameCount = 0;
                nowTime = 0;
                NextFrame();
            }
        }
        [Button("Stop")]
        private void StopPlayTest()
        {
            StopPlay(true);
        }
        public void StopPlay(bool restart)
        {
            isPlay = false;
            if (restart)
            {
                nowFrame = 0;
                nowFrameCount = 0;
                nowTime = 0;
            }
        }


        private void OnEnable()
        {
            if (playOnStart)
            {
                StartPlay(true);
            }
            animController.AddAnimator(this);
        }
        private void OnDisable()
        {
            StopPlay(true);
            animController?.RemoveAnimator(this);
        }
    }
}
