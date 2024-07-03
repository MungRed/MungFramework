using System.Collections;
using UnityEngine;
using MungFramework.Logic.Input;
using MungFramework.Logic;

namespace MungFramework.Demo
{
    public class BGMExecutor : GameExecutorAbstract
    {
        public BackGroundMusicSO BgmList;
        public SoundManager SoundManager;


        public override IEnumerator OnGamePause(GameManagerAbstract parentManager)
        {
            yield return StartCoroutine(base.OnGamePause(parentManager));
            
            yield return StartCoroutine(SoundManager.PauseAudio("music",transition:true));
        }
        public override IEnumerator OnGameResume(GameManagerAbstract parentManager)
        {
            yield return StartCoroutine(base.OnGameResume(parentManager));

            yield return StartCoroutine(SoundManager.ResumeAudio("music",transition:true));
        }

        [ContextMenu("PlayNextBgm")]
        public void PlayNextBgm()
        {
            var nextBgm = BgmList.GetRandomBgm();
            if (nextBgm == null)
            {
                return;
            }
            SoundManager.PlayAudio("music", nextBgm, transition: true, loop: true);
        }

        [ContextMenu("PauseBgm")]
        public void PauseBgm()
        {
            GameApplicationAbstract.Instance.DOGamePause();
        }

        [ContextMenu("ResumeBgm")]
        public void ResumeBgm()
        {
            GameApplicationAbstract.Instance.DOGameResume();
        }
    }
}