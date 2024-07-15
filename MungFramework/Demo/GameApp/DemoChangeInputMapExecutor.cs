using MungFramework.ComponentExtension;
using MungFramework.Logic;
using MungFramework.Logic.Input;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MungFramework.Demo
{
    public class DemoChangeInputMapExecutor : GameControllerAbstract
    {
        [SerializeField]
        private DemoInputManager InputManager;

        public override IEnumerator OnSceneLoad(GameManagerAbstract parentManager)
        {
            yield return base.OnSceneLoad(parentManager);
            InputManager = parentManager as DemoInputManager;
        }


        Coroutine testCor = null;

        public void ChanageKeyMapTest()
        {
            if (testCor == null)
            {
                testCor = StartCoroutine(changeKeyMapTest());
            }
        }

        public IEnumerator changeKeyMapTest()
        {
            var value = InputValueEnum.RIGTH_PAGE;

            var oldBind = InputManager.GetCurrentBind(value).ToList();
            if (oldBind.Count == 0)
            {
                Debug.Log("�ɰ��������ڣ������λ");
                yield return StartCoroutine(InputManager.ChangeKeyBind("KEYMAP_LAYER_Ui",InputKeyEnum.ANYKEY, value));
            }
            else
            {
                Debug.Log("�밴����� �ɰ���Ϊ��"+oldBind.First());
                yield return StartCoroutine(InputManager.ChangeKeyBind("KEYMAP_LAYER_Ui", oldBind.First(), value));
                var newBind = InputManager.GetCurrentBind(value).ToList();
                Debug.Log("�󶨳ɹ� �°���Ϊ��" + newBind.First());
            }

            testCor = null;
        }
    }
}

