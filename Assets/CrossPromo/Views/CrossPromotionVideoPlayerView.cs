using System;
using System.Collections.Generic;
using CrossPromo.Models;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;
using UnityEngine.UI;

namespace CrossPromo.Views
{
    public class CrossPromotionVideoPlayerView : MonoBehaviour
    {
        [SerializeField] private int InstanceId;
        private CrossPromotionVideoPlayer _videoPlayer;

        [SerializeField] private Button PlayButton;
        [SerializeField] private Button NextButton;
        [SerializeField] private Button PreviousButton;
        [SerializeField] private Sprite PlaySprite;
        [SerializeField] private Sprite PauseSprite;
        [SerializeField] private RawImage Screen;
        
        public void Init(Type videoPlayerType,List<CrossPromotionVideoPlayerTrack> tracks)
        {
            _videoPlayer = (CrossPromotionVideoPlayer) gameObject.AddComponent(videoPlayerType);
            _videoPlayer.Init(tracks,Screen);
            PlayButton.onClick.AddListener(() =>
            {
                if (_videoPlayer.IsPlaying())
                {
                    _videoPlayer.Pause();
                    PlayButton.image.sprite = PauseSprite;
                }
                else
                {
                    _videoPlayer.Resume();
                    PlayButton.image.sprite = PlaySprite;
                }
            });
            
            NextButton.onClick.AddListener(() =>
            {
                _videoPlayer.Next();
            });
            
            PreviousButton.onClick.AddListener(() =>
            {
                _videoPlayer.Previous();
            });
        }

        public void Dispose()
        {
            PlayButton.onClick.RemoveAllListeners();
            NextButton.onClick.RemoveAllListeners();
            PreviousButton.onClick.RemoveAllListeners();
        }
    }
}