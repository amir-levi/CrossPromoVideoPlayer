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
            _videoPlayer = gameObject.GetComponent<UnityEngine.Video.VideoPlayer>();
            if(_videoPlayer == null)
                _videoPlayer = gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
            
            
            _videoPlayer.isLooping = false;
            _videoPlayer.playOnAwake = false;
            _videoPlayer.source = VideoSource.Url;
            _videoPlayer.url = url;
            
            _audioSource = gameObject.GetComponent<AudioSource>();
            if(_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();
            
            _videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
            _videoPlayer.EnableAudioTrack(0, true);
            _videoPlayer.SetTargetAudioSource(0, _audioSource);
        }

        private void TrackFinishedPlaying(UnityEngine.Video.VideoPlayer source)
        {
            OnTrackFinish?.Invoke();
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
            _videoPlayer.loopPointReached += TrackFinishedPlaying;
            screen.SetTexture(_videoPlayer.texture);
            _videoPlayer.Play();
            _audioSource.Play();
        }

        public void Stop()
        {
            _videoPlayer.loopPointReached -= TrackFinishedPlaying;
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


        
        #if UNITY_EDITOR
        public bool Playing;
        [Range(0,1)]
        public float NormalizedTime;
        private void Update()
        {
            if(_videoPlayer == null) return;
            
            Playing = _videoPlayer.isPlaying;

            if (Playing)
                NormalizedTime = (float) (_videoPlayer.time / _videoPlayer.length);


        }
        #endif
    }
}