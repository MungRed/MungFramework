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
                    Debug.LogError(name + "��Ҫ�����ӹ�����");
                }
                return res;
            }
        }
#endif

        [InfoBox("��Ҫ�ϵ��ӹ�������", "isBind", InfoMessageType = InfoMessageType.Error)]
        [SerializeField]
        [Required("��Ҫ��ק����")]
        protected SoundDataManagerAbstract soundDataManager;


        [ReadOnly]
        [SerializeField]
        protected List<SoundSource> SoundSourceList;

        [ReadOnly]
        [SerializeField]
        //Ĭ�ϵ�����Դ
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
            //����Ĭ������Դ
            yield return InitSoundSource();
        }

        public override void OnGameUpdate(GameManagerAbstract parentManager)
        {
            base.OnGameUpdate(parentManager);

            //����ÿ����ƵԴ��λ��
            foreach (var soundSource in SoundSourceList)
            {
                soundSource.Source.transform.position = soundSource.Follow.position + soundSource.LocalPosition;
                soundSource.Volume = soundDataManager.GetVolumeData(soundSource.VolumeType)/100f;
            }
        }



        /// <summary>
        /// ��ʼ������Դ
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
        /// �������Դ
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual SoundManagerAbstract AddSoundSource(string id, SoundDataManagerAbstract.VolumeTypeEnum volumeType)
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

        protected virtual SoundSource GetSoundSource(string id)
        {
            return SoundSourceList.Find(x => x.id == id);
        }

    }
}