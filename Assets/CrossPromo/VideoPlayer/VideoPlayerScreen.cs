using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CrossPromo.VideoPlayer
{
    public class VideoPlayerScreen : MonoBehaviour
    {
        [SerializeField] private RawImage Image;
        public Action OnClick;
        private EventTrigger _eventTrigger;

        private Vector2 _playerSize;
        
        public void Init(Vector2 playerSize)
        {
            _playerSize = playerSize;
            _eventTrigger = GetComponent<EventTrigger>();
            CreateListener();
        }


        private void CreateListener()
        {
            var entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            entry.callback.AddListener((data) =>
            {
                OnClick?.Invoke();
            });
            _eventTrigger.triggers.Add(entry);
        }

        public void RemoveListener()
        {
            var entry = _eventTrigger.triggers.FirstOrDefault(t => t.eventID == EventTriggerType.PointerClick);
            if (entry != null)
                _eventTrigger.triggers.Remove(entry);
        }

        public void SetTexture(Texture texture)
        {
            Image.texture = texture;
            Image.SetNativeSize();
            var aspectRatio = _playerSize.x / _playerSize.y;
            var textureAspectRatio = texture.width / texture.height;

            if (aspectRatio > 1)
            {
                Image.GetComponent<RectTransform>().sizeDelta = new Vector2(_playerSize.y * textureAspectRatio, _playerSize.y);
            }
            else
            {
                Image.GetComponent<RectTransform>().sizeDelta = new Vector2(_playerSize.x , _playerSize.x* textureAspectRatio);
            }
            
            
           
            
            Image.GetComponent<RectTransform>().localPosition = Vector3.zero;
        }

     
      

       
    }
}