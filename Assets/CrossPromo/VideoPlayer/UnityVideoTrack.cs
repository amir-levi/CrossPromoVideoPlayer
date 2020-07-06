using System;
using System.Collections;
using CrossPromo.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace CrossPromo.VideoPlayer
{
    public class UnityVideoTrack : MonoBehaviour
    {
        private UnityEngine.Video.VideoPlayer _videoPlayer;
        private AudioSource _audioSource;
        public CrossPromotionVideoPlayerTrack CrossPromotionTrack;

        [Range(0f, 1f)]
        public float VideoNormalizedTime = 0;

        public Action OnTrackPrepared;
        public Action OnTrackFinish;
        public void Init(CrossPromotionVideoPlayerTrack track)
        {
            CrossPromotionTrack = track;
            _videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            
            _videoPlayer.isLooping = true;
            _videoPlayer.playOnAwake = false;
            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = CrossPromotionTrack.VideoUrl;
            
            _audioSource = gameObject.AddComponent<AudioSource>();
            _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayer.EnableAudioTrack(0, true);
            _videoPlayer.SetTargetAudioSource(0, _audioSource);

            _videoPlayer.loopPointReached += source =>
            {
                OnTrackFinish?.Invoke();
            };
            
        }


        public IEnumerator Prepare()
        {
            _videoPlayer.Prepare();
         
            while (!_videoPlayer.isPrepared)
            {
                Debug.Log("Preparing Index: " + CrossPromotionTrack.Id);
                yield return null;
            }

            OnTrackPrepared?.Invoke();

            yield return 0;
        }

        public void Play(RawImage screen)
        {
            screen.texture = _videoPlayer.texture;
            _videoPlayer.Play();
            _audioSource.Play();
            
            
        }

        public void Stop()
        {
            _videoPlayer.Stop();
            _audioSource.Stop();
        }
        
        #if UNITY_EDITOR
        private void Update()
        {
            if (!_videoPlayer.isPlaying) return;
            if (_videoPlayer.isPaused) return;
            VideoNormalizedTime =  (float) (_videoPlayer.time / _videoPlayer.length);
        }

#endif

        public void Pause()
        {
            if (_videoPlayer.isPlaying)
            {
                if (!_videoPlayer.isPaused)
                {
                    _videoPlayer.Pause();
                    _audioSource.Pause();
                }
            }
        }

        public void Resume()
        {
            if (_videoPlayer.isPaused)
            {
                _videoPlayer.Play();
                _audioSource.Play();
            }
        }
    }
}