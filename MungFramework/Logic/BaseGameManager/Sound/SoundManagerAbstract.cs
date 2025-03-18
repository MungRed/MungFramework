using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using MungFramework.Extension.AudioSourceExtension;
using MungFramework.Extension.TransformExtension;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Logic.Sound
{
    public abstract class SoundManagerAbstract : SingletonGameManagerAbstract<SoundManagerAbstract>
    {
        [SerializeField]
        [Required("��Ҫ��ק����")]
        protected SoundDataManagerAbstract soundDataManager;


        [ShowInInspector]
        protected List<SoundSource> SoundSourceList
        {
            get;
        } = new();


        //Ĭ�ϵ�����Դ
        [ShowInInspector]
        protected Dictionary<VolumeTypeEnum, string> DefaultSoundSourceList
        {
            get;
        } = new() { { VolumeTypeEnum.Music, "Music" }, { VolumeTypeEnum.Effect, "Effect" }, { VolumeTypeEnum.Voice, "Voice" } };


        public virtual int GetVolume(VolumeTypeEnum volumeType) => soundDataManager.GetVolume(volumeType);
        public virtual void SetVolume(VolumeTypeEnum volumeType, int val)
        {
            soundDataManager.SetVolumeData(volumeType, val);
            foreach (var soundSource in SoundSourceList.Where(x => x.VolumeType == volumeType))
            {
                soundSource.Volume = val / 100f;
                soundSource.Source.volume = soundSource.Volume;
            }
        }
        public virtual void DefaultVolume()
        {
            soundDataManager.DefaultVolumeData();
            foreach (var soundSource in SoundSourceList)
            {
                soundSource.Volume = GetVolume(soundSource.VolumeType) / 100f;
                soundSource.Source.volume = soundSource.Volume;
            }
        }


        public override void OnSceneLoad(GameManagerAbstract parentManager)
        {
            base.OnSceneLoad(parentManager);
            //����Ĭ������Դ
            foreach (var defaultSoundSource in DefaultSoundSourceList)
            {
                AddSoundSource(defaultSoundSource.Value, defaultSoundSource.Key);
            }
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);

            //����ÿ����ƵԴ��λ�ú�����
            foreach (var soundSource in SoundSourceList)
            {
                soundSource.Source.transform.position = soundSource.Follow.DirectionLocalPosition(soundSource.LocalPosition);
                soundSource.Volume = soundDataManager.GetVolume(soundSource.VolumeType) / 100f;
            }
        }

        /// <summary>
        /// �������Դ
        /// </summary>
        public virtual SoundManagerAbstract AddSoundSource(string id, VolumeTypeEnum volumeType)
        {
            //���id�Ƿ����
            if (GetSoundSource(id) != null)
            {
                Debug.LogError("����id�Ѵ���");
                return this;
            }

            float volume = soundDataManager.GetVolume(volumeType) / 100f;
            //�������Դ��Ϸ����
            GameObject soundSourceObj = AddSoundSourceObj(id, transform, Vector3.zero, volume);

            //ע������Դ
            SoundSource soundSource = new(id, transform, Vector3.zero, soundSourceObj.GetComponent<AudioSource>(), volumeType, volume);
            SoundSourceList.Add(soundSource);
            return this;
        }

        /// <summary>
        /// ��������Դ����
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
        /// ��������Դ���λ��
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
        /// �������Դ��Ϸ����
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
        /// �Ƴ�����Դ
        /// </summary>
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

        public virtual SoundManagerAbstract DoSoundOperate(SoundOperateData operateData)
        {
            switch (operateData.OperateType)
            {
                case SoundOperateData.SoundOperateTypeEnum.PlayAudio:
                    if (operateData.UseDefaultSoundSource)
                    {
                        PlayAudio(DefaultSoundSourceList[operateData.VolumeType], operateData.PlayAudioData);
                    }
                    else
                    {
                        PlayAudio(operateData.SoundSourceId, operateData.PlayAudioData);
                    }
                    break;
                case SoundOperateData.SoundOperateTypeEnum.AddSoundSouce:
                    AddSoundSource(operateData.SoundSourceId, operateData.VolumeType);
                    break;
                case SoundOperateData.SoundOperateTypeEnum.RemoveSoundSource:
                    RemoveSoundSource(operateData.SoundSourceId);
                    break;
                case SoundOperateData.SoundOperateTypeEnum.PauseAudio:
                    if (operateData.UseDefaultSoundSource)
                    {
                        StartCoroutine(PauseAudio(operateData.VolumeType, true));
                    }
                    else
                    {
                        StartCoroutine(PauseAudio(operateData.SoundSourceId, true));
                    }
                    break;
                case SoundOperateData.SoundOperateTypeEnum.ResumeAudio:
                    if (operateData.UseDefaultSoundSource)
                    {
                        StartCoroutine(ResumeAudio(operateData.VolumeType, true));
                    }
                    else
                    {
                        StartCoroutine(ResumeAudio(operateData.SoundSourceId, true));
                    }
                    break;
                case SoundOperateData.SoundOperateTypeEnum.StopAudio:
                    if (operateData.UseDefaultSoundSource)
                    {
                        StartCoroutine(StopAudio(operateData.VolumeType, true));
                    }
                    else
                    {
                        StartCoroutine(StopAudio(operateData.SoundSourceId, true));
                    }
                    break;
            }
            return this;
        }

        protected virtual SoundManagerAbstract PlayAudio(string soundSourceId, PlayAudioData playAudioData)
        {
            if (playAudioData.OneShot)
            {
                PlayAudioOneShot(soundSourceId, playAudioData.AudioClip);
            }
            else
            {
                PlayAudio(soundSourceId, playAudioData.AudioClip, playAudioData.Loop, playAudioData.Transition, playAudioData.ForceReplace);
            }
            return this;
        }


        /// <summary>
        /// ��������Դ����һ����Ƶ
        /// ���Ḳ��ԭ��Ƶ
        /// </summary>
        public virtual SoundManagerAbstract PlayAudioOneShot(string id, AudioClip audioClip)
        {
            var soundSource = GetSoundSource(id);

            if (soundSource == null)
            {
                return this;
            }

            soundSource.Source.PlayOneShot(audioClip, soundSource.Volume);
            return this;
        }
        public virtual SoundManagerAbstract PlayAudioOneShot(VolumeTypeEnum defaultType, AudioClip audioClip)
        {
            return PlayAudioOneShot(DefaultSoundSourceList[defaultType], audioClip);
        }

        /// <summary>
        /// ������Ƶ��������ŵ���Ƶ�뵱ǰ��Ƶ��ͬ�������ظ����ţ����replaceΪtrue������滻��ǰ��Ƶ
        /// </summary>
        public virtual SoundManagerAbstract PlayAudio(string id, AudioClip audioclip, bool loop = false, bool transition = false, bool forceReplace = false)
        {
            var soundSource = GetSoundSource(id);

            if (soundSource == null)
            {
                return this;
            }

            if (soundSource.Source.clip == audioclip && forceReplace == false)
            {
                return this;
            }

            StopTweener(id);

            var audioSource = soundSource.Source;
            var audioSourceObj = audioSource.gameObject;
            //var volume = soundSource.Volume;

            //�������Ҫ����
            if (transition == false)
            {
                //ֱ���л���Ƶ
                audioSource.clip = audioclip;
                audioSource.loop = loop;
                audioSource.volume = soundSource.Volume;
                audioSource.Play();
            }
            else
            {
                //����
                //���ƾɵ���ƵԴ
                var newAudioSource = audioSource.CopyTo(audioSourceObj);

                newAudioSource.clip = audioclip;
                newAudioSource.loop = loop;
                newAudioSource.volume = 0;
                newAudioSource.Play();


                //����ƵԴ����������������
                DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, 0.8f)
                    .SetEase(Ease.InOutSine)
                    .onComplete += () =>
                    {
                        Destroy(audioSource);
                    };

                //����ƵԴ����
                var dt = DOTween.To(() => newAudioSource.volume, x => newAudioSource.volume = x, soundSource.Volume, 1.6f)
                    .SetEase(Ease.InOutSine);

                dt.onComplete += () =>
                {
                    newAudioSource.volume = soundSource.Volume;
                };
                AddTweener(id, dt);

                soundSource.Source = newAudioSource;
            }
            return this;
        }

        public virtual SoundManagerAbstract PlayAudio(VolumeTypeEnum defaultType, AudioClip audioclip, bool loop = false, bool transition = false, bool forceReplace = false) => PlayAudio(DefaultSoundSourceList[defaultType], audioclip, loop, transition, forceReplace);

        //private TweenerCore<float, float, FloatOptions> tmpTweenCore;

        protected virtual IEnumerator PauseAudio(string id, bool transition = false)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                yield break;
            }
            //if (tmpTweenCore != null)
            //{
            //    if (!tmpTweenCore.IsComplete())
            //    {
            //        tmpTweenCore.Kill();
            //    }
            //}

            StopTweener(id);

            var audioSource = soundSource.Source;
            //var volume = soundSource.Volume;
            //�������Ҫ����
            if (transition == false)
            {
                //ֱ����ͣ
                audioSource.Pause();
                //audioSource.volume = volume;
            }
            else
            {
                //����
                var dt = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, 0.8f)
                    .SetEase(Ease.InOutSine);

                dt.onComplete += () =>
                {
                    audioSource.Pause();
                    audioSource.volume = soundSource.Volume;
                };
                AddTweener(id, dt);
                yield return dt.WaitForCompletion();
            }

            yield return null;
        }
        protected virtual IEnumerator PauseAudio(VolumeTypeEnum defaultType, bool transition = false)
        {
            return PauseAudio(DefaultSoundSourceList[defaultType], transition);
        }

        protected virtual IEnumerator ResumeAudio(string id, bool transition = false)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                yield break;
            }
            StopTweener(id);
            //if (tmpTweenCore != null)
            //{
            //    if (!tmpTweenCore.IsComplete())
            //    {
            //        tmpTweenCore.Kill();
            //    }
            //}
            var audioSource = soundSource.Source;
            //var volume = soundSource.Volume;

            //�������Ҫ����
            if (transition == false)
            {
                audioSource.UnPause();
                //audioSource.volume = sou;
            }
            else
            {
                audioSource.volume = 0;
                audioSource.UnPause();

                //����
                var dt = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, soundSource.Volume, 0.8f)
                    .SetEase(Ease.InOutSine);

                dt.onComplete += () =>
                {
                    audioSource.volume = soundSource.Volume;
                };
                AddTweener(id, dt);
                //tmpTweenCore = dt;
                yield return dt.WaitForCompletion();
            }
            yield return null;
        }
        protected virtual IEnumerator ResumeAudio(VolumeTypeEnum defaultType, bool transition = false)
        {
            return ResumeAudio(DefaultSoundSourceList[defaultType], transition);
        }




        protected virtual IEnumerator StopAudio(string id, bool transition = false)
        {
            var soundSource = GetSoundSource(id);
            if (soundSource == null)
            {
                yield break;
            }
            StopTweener(id);

            var audioSource = soundSource.Source;
            //var volumeType = soundSource.VolumeType;

            //�������Ҫ����
            if (transition == false)
            {
                //ֱ����ͣ
                audioSource.Stop();
            }
            else
            {
                //var oldvolume = soundSource.Volume;
                //����
                var dt = DOTween.To(() => audioSource.volume, x => audioSource.volume = x, 0, 0.8f)
                    .SetEase(Ease.InOutSine);
                dt.onComplete += () =>
                {
                    audioSource.Stop();
                    audioSource.volume = soundSource.Volume;
                };
                AddTweener(id, dt);
                yield return dt.WaitForCompletion();
            }
            yield return null;
        }
        protected virtual IEnumerator StopAudio(VolumeTypeEnum defaultType, bool transition = false)
        {
            return StopAudio(DefaultSoundSourceList[defaultType], transition);
        }

        protected Dictionary<string, TweenerCore<float, float, FloatOptions>> transitionTweenDic = new();
        protected void StopTweener(string id)
        {
            if (transitionTweenDic.ContainsKey(id))
            {
                transitionTweenDic[id].Complete();
                transitionTweenDic.Remove(id);
            }
        }
        protected void AddTweener(string id, TweenerCore<float, float, FloatOptions> tweener)
        {
            StopTweener(id);
            //if (transitionTweenDic.ContainsKey(id))
            //{
            //    transitionTweenDic[id].Kill();
            //    transitionTweenDic.Remove(id);
            //}
            transitionTweenDic.Add(id, tweener);
        }

        //private TweenerCore<float, float, FloatOptions> tmpTweenCore;

        protected virtual SoundSource GetSoundSource(string id)
        {
            return SoundSourceList.Find(x => x.Id == id);
        }

    }
}