using System;
using System.Collections.Generic;
using System.Linq;
using CrossPromo.Models;
using CrossPromo.VideoPlayer;
using CrossPromo.VideoPlayer.Actions;
using CrossPromo.VideoPlayer.Players;
using UnityEngine;
using UnityEngine.UI;

namespace CrossPromo.Views
{
    [ExecuteInEditMode]
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

        public VideoPlayerProperties VideoPlayerProperties;
        public Vector2 ScreenResolution;
        
        
        public void Init(List<CrossPromoVideoInfo> videosInfo)
        {
            _videoPlayerParent = GetComponentsInChildren<RectTransform>().ToList()
                .FirstOrDefault(t => "VideoPlayer".Equals(t.gameObject.name));

            if (_videoPlayerParent == null)
            {
                throw new NullReferenceException("Parent GameObject in the was changed pls revert back to the defualt");
            }
            
            
            Screen.Init(_videoPlayerParent.sizeDelta);
            
            _videoPlayer = new CrossPromoVideoPlayer(videosInfo,Screen,transform);
       
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
            
            ((IVideoTrackPreparedAction)_videoPlayer).OnNextVideoTrackReady += () =>
            {
                NextButton.interactable = true;
            };
            
            ((IVideoTrackPreparedAction)_videoPlayer).OnPreviousVideoTrackReady += () =>
            {
                PreviousButton.interactable = true;
            };


            ((IVideoClickedAction)_videoPlayer).OnVideoClicked = id =>
            {
                var playListItem = videosInfo.FirstOrDefault(item => item.Id == id);
                Screen.RemoveListener();
                VideoClicked(InstanceId, playListItem);
            };
        }


       
        
        
       

        #if UNITY_EDITOR
        private Canvas _canvas;
        private RectTransform _videoPlayerParent;
        private void OnEnable()
        {
            _canvas = GetComponentInChildren<Canvas>();
            ScreenResolution = _canvas.pixelRect.size;
            _videoPlayerParent = GetComponentsInChildren<RectTransform>().ToList()
                .FirstOrDefault(t => "VideoPlayer".Equals(t.gameObject.name));
        }
        
        void Update()
        {
            if(Application.isPlaying) return;;
            if(_canvas == null) return;

            if (ScreenResolution != _canvas.pixelRect.position)
                ScreenResolution = _canvas.pixelRect.size;

            var x = VideoPlayerProperties.WidthInPercent * (ScreenResolution.x / _canvas.scaleFactor);
            var y = VideoPlayerProperties.HeightInPercent * (ScreenResolution.y / _canvas.scaleFactor);
            _videoPlayerParent.eulerAngles = VideoPlayerProperties.Pivot;

            Vector2 deltaPos = _canvas.GetComponent<RectTransform>().position;
            _videoPlayerParent.position = VideoPlayerProperties.Position + deltaPos;
            _videoPlayerParent.sizeDelta = new Vector2(x,y);
        }
        #endif
 
    }
}