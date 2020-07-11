using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

namespace CrossPromo.VideoPlayer
{
    public class CrossPromoVideoTrack : MonoBehaviour
    {
        public int Id;
        private UnityEngine.Video.VideoPlayer _videoPlayer;
        private AudioSource _audioSource;
        public Action OnTrackPrepared;
        public Action OnTrackFinish;
        public void Init(int id, string url)
        {
            Id = id;
            _videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            
            _videoPlayer.isLooping = false;
            _videoPlayer.playOnAwake = false;
            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = url;
            
            _audioSource = gameObject.AddComponent<AudioSource>();
            _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayer.EnableAudioTrack(0, true);
            _videoPlayer.SetTargetAudioSource(0, _audioSource);

            _videoPlayer.loopPointReached += source =>
            {
                OnTrackFinish?.Invoke();
            };
        }


        public void Prepare()
        {
            StartCoroutine(_prepare());
        }
        
        private IEnumerator _prepare()
        {
            if (_videoPlayer.isPrepared)
            {
                OnTrackPrepared?.Invoke();
                yield break;
            }
            
            _videoPlayer.Prepare();
         
            while (!_videoPlayer.isPrepared)
            {
                yield return null;
            }

            OnTrackPrepared?.Invoke();
        }

        public void Play(VideoPlayerScreen screen)
        {
            screen.SetTexture(_videoPlayer.texture);
            _videoPlayer.Play();
            _audioSource.Play();
        }

        public void Stop()
        {
            _videoPlayer.Stop();
            _audioSource.Stop();
        }
        
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

        public bool IsPlaying()
        {
            return _videoPlayer.isPlaying;
        }

    }
}