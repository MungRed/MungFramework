using MungFramework.Ui;
using UnityEngine;

namespace MungFramework.Demo
{
    public class UiButtonDemo : UiButtonAbstract
    {
        public AudioClip CheckAudio;

        public override void Select()
        {
            base.Select();
            if (CheckAudio != null)
            {
                GameApplication.Instance.SoundManager.PlayAudio("effect", CheckAudio);
            }
        }

    }
}
