using UnityEngine;
using UnityEngine.Advertisements;

namespace MazesAndMore
{
    public class CreateAdVideo : MonoBehaviour, IUnityAdsListener
    {
        [Tooltip("ID del proyecto")]
        public string gameId = "3972135";

        /// <summary>
        /// ID del tipo de anuncio que es
        /// </summary>
        private string myPlacementId = "rewardedVideo";
        /// <summary>
        /// si son anuncios reales o no
        /// </summary>
        private bool testMode = true;

        /// <summary>
        /// incializar el anuncio
        /// </summary>
        void Start()
        {
            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId, testMode);
        }

        /// <summary>
        /// mostrar el anuncio
        /// </summary>
        public void ShowRewardedVideo()
        {
            // Check if UnityAds ready before calling Show method:
            if (Advertisement.IsReady(myPlacementId))
            {
                Advertisement.Show(myPlacementId);
            }
            else
            {
                Debug.Log("Rewarded video is not ready at the moment! Please try again later!");
            }
        }

        /// <summary>
        /// se le llama cuando el anuncio a terminado
        /// </summary>
        /// <param name="placementId">ID del tipo de anuncio</param>
        /// <param name="showResult">resultado</param>
        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            // Define conditional logic for each ad completion status:
            if (showResult == ShowResult.Finished)
            {
                ProgessManager.Save save = GameManager.GetInstance().GetProgess();
                save.h++;
                GameManager.GetInstance().UpdateProgess(save);
            }
            else if (showResult == ShowResult.Skipped)
            {
                // Do not reward the user for skipping the ad.
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }

        /// <summary>
        /// cuando el video se destruye, destruye el listener
        /// </summary>
        public void OnDestroy()
        {
            Advertisement.RemoveListener(this);
        }

        /// <summary>
        /// metodos que hacen cosas que no usamos pero deben estar porque heredan.........
        /// </summary>
        public void OnUnityAdsReady(string placementId)
        {
            // If the ready Placement is rewarded, show the ad:
            if (placementId == myPlacementId)
            {
                // Optional actions to take when the placement becomes ready(For example, enable the rewarded ads button)
            }
        }
        public void OnUnityAdsDidError(string message)
        {
            // Log the error.
        }
        public void OnUnityAdsDidStart(string placementId)
        {
            // Optional actions to take when the end-users triggers an ad.
        }
    }
}