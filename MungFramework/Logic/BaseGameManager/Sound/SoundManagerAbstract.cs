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


        public virtual int GetVolumeData(VolumeTypeEnum volumeType) => soundDataManager.GetVolumeData(volumeType);

        public virtual void SetSoundVolume(VolumeTypeEnum volumeType, int val)
        {
            soundDataManager.SetVolumeData(volumeType, val);
            foreach (var soundSource in SoundSourceList.Where(x => x.VolumeType == volumeType))
            {
                soundSource.Volume = val / 100f;
                soundSource.Source.volume = soundSource.Volume;
            }
        }

        public virtual void DefaultVolumeData()
        {
            soundDataManager.DefaultVolumeData();
            foreach (var soundSource in SoundSourceList)
            {
                soundSource.Volume = GetVolumeData(soundSource.VolumeType) / 100f;
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
                soundSource.Volume = soundDataManager.GetVolumeData(soundSource.VolumeType) / 100f;
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

            float volume = soundDataManager.GetVolumeData(volumeType) / 100f;
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

        public virtual SoundManagerAbstract PlayAudio(PlayAudioData playAudioData)
        {
            if (playAudioData.OneShot)
            {
                if (playAudioData.UseDefaultSoundSource)
                {
                    PlayAudioOneShot(playAudioData.VolumeType, playAudioData.AudioClip);
                }
                else
                {
                    PlayAudioOneShot(playAudioData.SoundSourceId, playAudioData.AudioClip);
                }
            }
            else
            {
                if (playAudioData.UseDefaultSoundSource)
                {
                    PlayAudio(playAudioData.VolumeType, playAudioData.AudioClip, playAudioData.Loop, playAudioData.Transition, playAudioData.ForceReplace);
                }
                else
                {
                    PlayAudio(playAudioData.SoundSourceId, playAudioData.AudioClip, playAudioData.Loop,playAudioData.Transition, playAudioData.ForceReplace);
                }
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

            soundSource.Source.PlayOneShot(audioClip, soundSource.Source.volume);
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

            var audioSource = soundSource.Source;
            var audioSourceObj = audioSource.gameObject;
            var volume = soundSource.Volume;

            //�������Ҫ����
            if (transition == false)
            {
                //ֱ���л���Ƶ
                audioSource.clip = audioclip;
                audioSource.loop = loop;
                audioSource.volume = volume;
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

        public virtual SoundManagerAbstract PlayAudio(VolumeTypeEnum defaultType, AudioClip audioclip, bool loop = false, bool transition = false, bool forceReplace = false) => PlayAudio(DefaultSoundSourceList[defaultType], audioclip, loop, transition, forceReplace);

        private TweenerCore<float, float, FloatOptions> tmpTweenCore;

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
            //�������Ҫ����
            if (transition == false)
            {
                //ֱ����ͣ
                audioSource.Pause();
                audioSource.volume = volume;
            }
            else
            {
                //����
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
        public virtual IEnumerator PauseAudio(VolumeTypeEnum defaultType, bool transition = false)
        {
            return PauseAudio(DefaultSoundSourceList[defaultType], transition);
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

            //�������Ҫ����
            if (transition == false)
            {
                //ֱ����ͣ
                audioSource.UnPause();
                audioSource.volume = volume;
            }
            else
            {
                audioSource.volume = 0;
                audioSource.UnPause();

                //����
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
        public virtual IEnumerator ResumeAudio(VolumeTypeEnum defaultType, bool transition = false)
        {
            return ResumeAudio(DefaultSoundSourceList[defaultType], transition);
        }

        protected virtual SoundSource GetSoundSource(string id)
        {
            return SoundSourceList.Find(x => x.Id == id);
        }

    }
}