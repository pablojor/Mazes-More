using UnityEngine;
using UnityEngine.Advertisements;

namespace MazesAndMore
{
    public class CreateAdFullScreen : MonoBehaviour
    {
        [Tooltip("ID del proyecto")]
        public string gameId = "3972135";

        /// <summary>
        /// si mostramos anuncios reales o no
        /// </summary>
        private bool testMode = true;

        /// <summary>
        /// incializa el anuncio
        /// </summary>
        void Start()
        {
            // Initialize the Ads service:
            Advertisement.Initialize(gameId, testMode);
        }

        /// <summary>
        /// activa el anuncio
        /// </summary>
        public void ShowInterstitialAd()
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady())
            {
                Advertisement.Show();
            }
            else
            {
                Debug.Log("Interstitial ad not ready at the moment! Please try again later!");
            }
        }
    }
}