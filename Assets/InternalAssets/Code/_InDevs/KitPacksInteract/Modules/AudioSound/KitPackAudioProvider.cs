﻿using System;
using Scellecs.Morpeh;
using UnityEngine;
using Scellecs.Morpeh.Providers;
using Unity.IL2CPP.CompilerServices;

namespace ProjectOlog.Code._InDevs.KitPacksInteract.Modules.AudioSound
{
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public sealed class KitPackAudioProvider : MonoProvider<KitPackAudio>
    {

    }

    [Serializable]
    [Il2CppSetOption(Option.NullChecks, false)]
    [Il2CppSetOption(Option.ArrayBoundsChecks, false)]
    [Il2CppSetOption(Option.DivideByZeroChecks, false)]
    public struct KitPackAudio : IComponent
    {
        [Header("AudioSource")]
        public AudioSource AudioSource;

        [Header("AudioClip")] 
        public AudioClip AudioClip;
    }
}