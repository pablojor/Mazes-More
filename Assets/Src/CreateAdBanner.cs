using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

namespace MazesAndMore
{
    public class CreateAdBanner : MonoBehaviour
    {
        [Tooltip("ID del proyecto")]
        public string gameId = "3972135";
        [Tooltip("ID del tipo de anuncio")]
        public string placementId = "bannerPlacement";
        [Tooltip("Marca si son anuncios reales o no")]
        public bool testMode = true;

        /// <summary>
        /// incializa el banner
        /// </summary>
        void Start()
        {
            Advertisement.Initialize(gameId, testMode);
            StartCoroutine(ShowBannerWhenInitialized());
        }

        /// <summary>
        /// muestra el banner
        /// </summary>
        /// <returns></returns>
        IEnumerator ShowBannerWhenInitialized()
        {
            while (!Advertisement.isInitialized)
            {
                yield return new WaitForSeconds(0.5f);
            }
            Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
            Advertisement.Banner.Show(placementId);
        }
    }

}