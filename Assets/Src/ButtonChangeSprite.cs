using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazesAndMore
{
    public class ButtonChangeSprite : ButtonObject
    {
        [Tooltip("Un sprite")]
        public Sprite OffSprite;
        [Tooltip("El otro sprite")]
        public Sprite OnSprite;
        [Tooltip("Boton al que se le cambian la imagenes")]
        public Button button;

        /// <summary>
        /// metodo al que se llama en onClick()
        /// cambiar el sprite actual del boton por el otro sprite
        /// OnSprite --> OffSprite  y viceversa
        /// </summary>
        public void ChangeImage()
        {
            if (button.image.sprite == OnSprite)
            {
                SpriteState spriteState = new SpriteState();
                spriteState = button.spriteState;
                spriteState.pressedSprite = OnSprite;

                button.image.sprite = OffSprite;
                button.spriteState = spriteState;
            }
            else
            {
                SpriteState spriteState = new SpriteState();
                spriteState = button.spriteState;
                spriteState.pressedSprite = OffSprite;

                button.image.sprite = OnSprite;
                button.spriteState = spriteState;
            }
        }
    }
}