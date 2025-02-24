﻿using NetCode.Limits;

namespace ProjectOlog.Code.Networking.Profiles.Snapshots.Core
{
    public static class NetworkTransformLimits
    {
        public static readonly FloatLimit Yaw = new FloatLimit(0, 360, 0.01f);
        public static readonly FloatLimit Pitch = new FloatLimit(-90, 90, 0.01f);
        
        public static readonly FloatLimit Rotation = new FloatLimit(0, 360, 0.01f);
    }
}