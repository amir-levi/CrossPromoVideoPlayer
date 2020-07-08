using System;
using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using CrossPromo.VideoPlayer;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;
using UnityEngine.UI;

namespace CrossPromo.Views
{
    public class CrossPromotionVideoPlayerView : MonoBehaviour
    {
       public string ServerUrl;
        
        [SerializeField] private int InstanceId;
        private IVideoPlayer _videoPlayer;

        [SerializeField] private Button PlayButton;
        [SerializeField] private Button NextButton;
        [SerializeField] private Button PreviousButton;
        [SerializeField] private Sprite PlaySprite;
        [SerializeField] private Sprite PauseSprite;
        [SerializeField] private VideoPlayerScreen Screen;

        public Action<int, CrossPromoVideoInfo> VideoClicked;
        
        [SerializeField] private List<CrossPromoVideoInfo> _videosInfo = new List<CrossPromoVideoInfo>();
        
        public void Init(List<CrossPromoVideoInfo> videosInfo)
        {
            _videosInfo = videosInfo;
            Screen.CreateListener();
            _videoPlayer = new CrossPromoVideoPlayer(videosInfo,Screen,transform);
           // _videoPlayer = (CrossPromotionVideoPlayer) gameObject.AddComponent(videoPlayerType);
           // _videoPlayer.Init(tracks,Screen);
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


            
       
            _videoPlayer.OnVideoClicked = id =>
            {
                Debug.Log("Moshe Cohen");
                var playListItem = videosInfo.FirstOrDefault(item => item.Id == id);
                Screen.RemoveListener();
                VideoClicked(InstanceId, playListItem);
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