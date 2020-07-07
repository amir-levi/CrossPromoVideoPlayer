using System;
using System.Collections.Generic;
using CrossPromo.Models;
using CrossPromo.VideoPlayer;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;
using UnityEngine.EventSystems;
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
        [SerializeField] private VideoPlayerScreen Screen;

        public Action<int, CrossPromotionVideoPlayerTrack> VideoClicked;
        
        public void Init(Type videoPlayerType,List<CrossPromotionVideoPlayerTrack> tracks)
        {
            _videoPlayer = (CrossPromotionVideoPlayer) gameObject.AddComponent(videoPlayerType);
            Screen.CreateListener();
            _videoPlayer.Init(tracks,Screen);
            PlayButton.onClick.AddListener(() =>
            {
                if (_videoPlayer.IsPlaying())
                {
                    PlayButton.image.sprite = PauseSprite;
                    _videoPlayer.Pause();
                   
                }
                else
                {
                    PlayButton.image.sprite = PlaySprite;
                    _videoPlayer.Resume();
                    
                }
            });
            
            NextButton.onClick.AddListener(() =>
            {
                PlayButton.image.sprite = PlaySprite;
                PreviousButton.interactable = false;
                NextButton.interactable = false;
                _videoPlayer.Next();
               
            });
            
            PreviousButton.onClick.AddListener(() =>
            {
                PlayButton.image.sprite = PlaySprite;
                PreviousButton.interactable = false;
                NextButton.interactable = false;
                _videoPlayer.Previous();
                
            });

            _videoPlayer.OnNextVideoTrackReady += () =>
            {
                NextButton.interactable = true;
            };
            
            _videoPlayer.OnNextVideoTrackReady += () =>
            {
                PreviousButton.interactable = true;
            };


            _videoPlayer.OnVideoClicked = track =>
            {
                Debug.Log("Moshe Cohen");
                Screen.RemoveListener();
                VideoClicked(InstanceId, track);
            };

        }
        
   

        public void Dispose()
        {
            PlayButton.onClick.RemoveAllListeners();
            NextButton.onClick.RemoveAllListeners();
            PreviousButton.onClick.RemoveAllListeners();
        }
    }
}