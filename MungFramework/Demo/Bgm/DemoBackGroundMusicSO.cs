using MungFramework.ComponentExtension;
using MungFramework.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Demo
{
    //[CreateAssetMenu(fileName = "newBackGroundMusicSO", menuName = "SO/BackGroundMusic")]
    public class DemoBackGroundMusicSO : DataSO
    {
        public List<AudioClip> BackGroundMusicList;


        public AudioClip GetRandomBgm()
        {
            if (BackGroundMusicList.Empty())
            {
                return null;
            }
            return BackGroundMusicList[Random.Range(0, BackGroundMusicList.Count)];
        }
    }

}