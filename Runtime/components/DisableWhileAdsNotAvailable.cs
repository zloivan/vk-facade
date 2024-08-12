using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

namespace vk_facade.Runtime.components
{
    public class DisableWhileAdsNotAvailable : MonoBehaviour
    {
        [SerializeField]
        private int _adCheckFrequencyMilliseconds = 200;


        private CanvasGroup _canvasGroup;
        private CancellationTokenSource _cancellationTokenSource;

        private void Awake()
        {
            // Ensure the CanvasGroup is attached
            if (!TryGetComponent(out _canvasGroup))
            {
                _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
        }

        private void OnEnable()
        {
            // Create a new cancellation token source
            _cancellationTokenSource = new CancellationTokenSource();

            // Start the async operation
            DisableWhileAdsNotAvailableAsync(_cancellationTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            // Cancel the operation when the object is disabled
            _cancellationTokenSource?.Cancel();
        }

        private async UniTaskVoid DisableWhileAdsNotAvailableAsync(CancellationToken cancellationToken)
        {
            if (this == null || gameObject == null || !gameObject.activeInHierarchy) return;

            _canvasGroup.interactable = false;

            try
            {
                var adsAvailable = await VkBridgeFacade.CheckNativeRewardAd();

                while (!adsAvailable && !cancellationToken.IsCancellationRequested)
                {
                    if (this == null || gameObject == null || !gameObject.activeInHierarchy)
                    {
                        return;
                    }

                    adsAvailable = await VkBridgeFacade.CheckNativeRewardAd();

                    // Delay between checks, checking cancellation token to avoid unnecessary waits
                    await UniTask.Delay(200, cancellationToken: cancellationToken);
                }
            }
            finally
            {
                if (this != null && gameObject != null && gameObject.activeSelf)
                {
                    _canvasGroup.interactable = true;
                }
            }
        }
    }
}