using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MungFramework.Extension.ComponentExtension;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    public abstract class SoundManagerAbstract : SingletonGameManagerAbstract<SoundManagerAbstract>
    {
#if UNITY_EDITOR
        private bool isBind
        {
            get
            {
                bool res =  !(soundDataManager != null && subGameManagerList.Contains(soundDataManager));
                if (res == true)
                {
                    Debug.LogError(name + "需要挂载子管理器");
                }
                return res;
            }
        }
#endif

        [InfoBox("需要拖到子管理器中", "isBind", InfoMessageType = InfoMessageType.Error)]
        [SerializeField]
        [Required("需要拖拽挂载")]
        protected SoundDataManagerAbstract soundDataManager;


        [ReadOnly]
        [SerializeField]
        protected List<SoundSource> SoundSourceList;

        [ReadOnly]
        [SerializeField]
        //默认的声音源
        protected  List<(string, SoundDataManagerAbstract.VolumeTypeEnum)> DefaultSoundSourceList = new List<(string, SoundDataManagerAbstract.VolumeTypeEnum)>() {
            ("music",SoundDataManagerAbstract.VolumeTypeEnum.Music),
            ("effect",SoundDataManagerAbstract.VolumeTypeEnum.Effect),
            ("voice",SoundDataManagerAbstract.VolumeTypeEnum.Voice)
        };

        public virtual void SetSoundVolume(SoundDataManagerAbstract.VolumeTypeEnum volumeType, int val)
        {
            soundDataManager.SetVolumeData(volumeType, val);
        }



        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
            //加载默认声音源
            yield return InitSoundSource();
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);

            //设置每个音频源的位置
            foreach (var soundSource in SoundSourceList)
            {
                soundSource.Source.transform.position = soundSource.Follow.position + soundSource.LocalPosition;
                soundSource.Volume = soundDataManager.GetVolumeData(soundSource.VolumeType)/100f;
            }
        }



        /// <summary>
        /// 初始化声音源
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator InitSoundSource()
        {
            foreach (var defaultSoundSource in DefaultSoundSourceList)
            {
                AddSoundSource(defaultSoundSource.Item1, defaultSoundSource.Item2);
                yield return new WaitForEndOfFrame();
            }
        }


        /// <summary>
        /// 添加声音源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SoundManagerAbstract AddSoundSource(string id, SoundDataManagerAbstract.VolumeTypeEnum volumeType)
        {
            //检查id是否存在
            if (GetSoundSource(id) != null)
            {
                Debug.LogError("声音id已存在");
                return this;
            }

            float volume = soundDataManager.GetVolumeData(volumeType) / 100f;
            //添加声音源游戏对象
            GameObject soundSourceObj = AddSoundSourceObj(id, transform, Vector3.zero, volume);

            //注册声音源
            SoundSource soundSource = new()
            {
                id = id,
                Follow = transform,
                LocalPosition = Vector3.zero,

                Source = soundSourceObj.GetComponent<AudioSource>(),
                VolumeType = volumeType,
                Volume = volume
            };
            SoundSourceList.Add(soundSource);

            return this;
        }

        /// <summary>
        /// 设置声音源跟随
        /// </summary>
        public virtual SoundManagerAbstract SetSoundSourceFollow(string id, Transform follow)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                return this;
            }

            soundSource.Follow = follow;
            return this;
        }

        /// <summary>
        /// 设置声音源相对位置
        /// </summary>
        public virtual SoundManagerAbstract SetSoundSourceLocalPosition(string id, Vector3 localPosition)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                return this;
            }
            soundSource.LocalPosition = localPosition;
            return this;
        }
        /// <summary>
        /// 添加声音源游戏对象
        /// </summary>
        protected virtual GameObject AddSoundSourceObj(string id, Transform parent, Vector3 localPosition, float volume)
        {
            GameObject newObj = new(id, typeof(AudioSource))
            {
                transform =
                {
                    parent = parent,
                    position = parent.position+localPosition
                }
            };
            var audioSource = newObj.GetComponent<AudioSource>();
            audioSource.volume = volume;
            audioSource.playOnAwake = false;

            return newObj;
        }

        /// <summary>
        /// 移除声音源
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SoundManagerAbstract RemoveSoundSource(string id)
        {
            var target = GetSoundSource(id);

            if (target == null)
            {
                return this;
            }

            SoundSourceList.Remove(target);

            Destroy(target.Source.gameObject);
            return this;
        }



        public virtual SoundManagerAbstract PlayAudio(string id, AudioClip audioclip, bool loop = false, bool transition = false,bool replace = false)
        {
            var soundSource = GetSoundSource(id);

            if (soundSource == null)
            {
                return this;
            }

            if (soundSource.Source.clip == audioclip&&replace==false)
            {
                return this;
            }

            var audioSource = soundSource.Source;
            var audioSourceObj = audioSource.gameObject;
            var volume = soundSource.Volume;

            //如果不需要过渡
            if (transition == false)
            {
                //直接切换音频
                audioSource.clip = audioclip;
                audioSource.loop = loop;
                audioSource.volume = volume;
                audioSource.Play();
            }
            else
            {
                //过渡
                //复制旧的音频源
                var newAudioSource = audioSource.CopyTo(audioSourceObj);

                newAudioSource.clip = audioclip;
                newAudioSource.loop = loop;
                newAudioSource.volume = 0;
                newAudioSource.Play();


                //旧音频源淡出，结束后销毁
                DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, 0.8f)
                    .SetEase(Ease.InOutSine)
                    .onComplete += () =>
                    {
                        Destroy(audioSource);
                    };

                //新音频源淡入
                DOTween.To(() => newAudioSource.volume, x => newAudioSource.volume = x, volume, 1.6f)
                    .SetEase(Ease.InOutSine)
                    .onComplete += () =>
                    {
                        newAudioSource.volume = volume;
                    };

                soundSource.Source = newAudioSource;
            }
            return this;
        }


        private TweenerCore<float,float, FloatOptions> tmpTweenCore;

        public virtual IEnumerator PauseAudio(string id, bool transition = false)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                yield break;
            }
            if (tmpTweenCore != null)
            {
                if (!tmpTweenCore.IsComplete())
                {
                    tmpTweenCore.Kill();          
                }
            }
            var audioSource = soundSource.Source;
            var volume = soundSource.Volume;
            //如果不需要过渡
            if (transition == false)
            {
                //直接暂停
                audioSource.Pause();
                audioSource.volume = volume;
            }
            else
            {
                //淡出
                var dt = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, 0.8f)
                    .SetEase(Ease.InOutSine);

                dt.onComplete += () =>
                {
                    audioSource.Pause();
                    audioSource.volume = volume;
                };
                tmpTweenCore = dt;
                yield return dt.WaitForCompletion();
            }

            yield return null;
        }
        public virtual IEnumerator ResumeAudio(string id, bool transition = false)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                yield break;
            }
            if (tmpTweenCore != null)
            {
                if (!tmpTweenCore.IsComplete())
                {
                    tmpTweenCore.Kill();
                }
            }
            var audioSource = soundSource.Source;
            var volume = soundSource.Volume;

            //如果不需要过渡
            if (transition == false)
            {
                //直接暂停
                audioSource.UnPause();
                audioSource.volume = volume;
            }
            else
            {
                audioSource.volume = 0;
                audioSource.UnPause();

                //淡入
                var dt = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, volume, 0.8f)
                    .SetEase(Ease.InOutSine);

                dt.onComplete += () =>
                {
                    audioSource.volume = volume;
                };

                tmpTweenCore = dt;

                yield return dt.WaitForCompletion();

            }
            yield return null;
        }

        protected virtual SoundSource GetSoundSource(string id)
        {
            return SoundSourceList.Find(x => x.id == id);
        }

    }
}