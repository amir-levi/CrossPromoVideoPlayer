using System;
using UnityEngine;

namespace CrossPromo.VideoPlayer
{
    [Serializable]
    public class VideoPlayerProperties
    {
        [Range(0,1)]
        public float WidthInPercent = 1;
        [Range(0,1)]
        public float HeightInPercent = 1;
        
        public Vector2 Position;
        public Vector3 Pivot;
        
    }
   
}