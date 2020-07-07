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
        
        void Awake()
        {
            _eventTrigger = GetComponent<EventTrigger>();
        }


        public void CreateListener()
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
            Debug.Log( (entry == null) );
            if (entry != null)
                _eventTrigger.triggers.Remove(entry);
        }

        public void SetTexture(Texture texture)
        {
            Image.texture = texture;
        }

     
      

       
    }
}