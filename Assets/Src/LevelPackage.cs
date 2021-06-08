using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MazesAndMore
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/LevelGroup", order = 1)]
    public class LevelPackage : ScriptableObject
    {
        [Tooltip("Numero ID del paquete")]
        public int packageNumber;
        [Tooltip("Nombre del tipo de niveles (aparecera en los botones)")]
        public string name;
        [Tooltip("Color identificativo de este tipo de niveles")]
        public Color packageColor;
        [Tooltip("Imagen del boton del tipo de nivel")]
        public Sprite buttonImage;
        [Tooltip("Imagen pulsada del boton del tipo de nivel")]
        public Sprite buttonImagePressed;
        [Tooltip("Array con los niveles de este paquete")]
        public TextAsset[] levels;
    }
}