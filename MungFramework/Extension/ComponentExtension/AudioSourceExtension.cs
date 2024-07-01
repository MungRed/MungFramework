using UnityEngine;

namespace MungFramework.ComponentExtension
{
    public static class AudioSourceExtension
    {
        public static AudioSource CopyTo(this AudioSource source ,GameObject target)
        {
            var newAudioSource = target.AddComponent<AudioSource>();

            newAudioSource.clip = source.clip;
            newAudioSource.volume = source.volume;
            newAudioSource.pitch = source.pitch;
            newAudioSource.loop = source.loop;
            newAudioSource.playOnAwake = source.playOnAwake;
            newAudioSource.spatialBlend = source.spatialBlend;
            newAudioSource.reverbZoneMix = source.reverbZoneMix;
            newAudioSource.dopplerLevel = source.dopplerLevel;
            newAudioSource.rolloffMode = source.rolloffMode;
            newAudioSource.minDistance = source.minDistance;
            newAudioSource.maxDistance = source.maxDistance;
            newAudioSource.panStereo = source.panStereo;
            newAudioSource.spatialize = source.spatialize;
            newAudioSource.spatializePostEffects = source.spatializePostEffects;
            newAudioSource.spread = source.spread;
            newAudioSource.rolloffMode = source.rolloffMode;
            newAudioSource.bypassEffects = source.bypassEffects;
            newAudioSource.bypassListenerEffects = source.bypassListenerEffects;
            newAudioSource.bypassReverbZones = source.bypassReverbZones;
            newAudioSource.priority = source.priority;
            newAudioSource.mute = source.mute;
            newAudioSource.playOnAwake = source.playOnAwake;
            newAudioSource.ignoreListenerPause = source.ignoreListenerPause;
            newAudioSource.ignoreListenerVolume = source.ignoreListenerVolume;
            newAudioSource.outputAudioMixerGroup = source.outputAudioMixerGroup;

            return newAudioSource;
        }
    }
}
