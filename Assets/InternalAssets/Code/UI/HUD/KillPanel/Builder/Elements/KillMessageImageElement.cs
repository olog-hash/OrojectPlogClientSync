using UnityEngine;
using UnityEngine.UI;

namespace ProjectOlog.Code.UI.HUD.KillPanel.Builder.Elements
{
    public class KillMessageImageElement : KillMessageElement
    {
        private Sprite _sprite;

        public KillMessageImageElement(Sprite sprite)
        {
            _sprite = sprite;
        }

        public void Display(GameObject parentObject)
        {
            var imageObject = new GameObject("Image");
            imageObject.transform.SetParent(parentObject.transform, false);

            var image = imageObject.AddComponent<Image>();
            image.sprite = _sprite;
        }
    }
}