using System.Collections;
using UnityEngine;
using MungFramework.Logic.Input;
using MungFramework.Logic;

namespace MungFramework.Demo
{
    public class DemoBGMExecutor : GameControllerAbstract
    {
        public DemoBackGroundMusicSO BgmList;
        public DemoSoundManager SoundManager;


        public override void OnGamePause(GameManagerAbstract parentManager)
        {
            base.OnGamePause(parentManager);
            
            SoundManager.PauseAudio("music", transition: true);
        }
        public override void OnGameResume(GameManagerAbstract parentManager)
        {
            base.OnGameResume(parentManager);

            SoundManager.ResumeAudio("music", transition: true);
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