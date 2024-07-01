using MungFramework.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace MungFramework.Demo
{
    [CreateAssetMenu(fileName = "newBackGroundMusicSO", menuName = "SO/BackGroundMusic")]
    public class BackGroundMusicSO : DataSO
    {
        public List<AudioClip> BackGroundMusicList;


        public AudioClip GetNextMusic()
        {
            var nextIndex = Random.Range(0, BackGroundMusicList.Count);
            return BackGroundMusicList[nextIndex];
        }
    }

}