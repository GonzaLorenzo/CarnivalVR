using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.XRContent.Interaction
{
    internal class InteractionAnimator : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("The timeline to drive with the activation button")]
        PlayableDirector m_ToAnimate;

        bool m_Animating = false;
        XRBaseController m_Controller;

        void Start()
        {
            var interactable = GetComponent<IXRSelectInteractable>();
            if (interactable == null || interactable as Object == null)
            {
                Debug.LogWarning($"No interactable on {name} - no animation will be played.", this);
                enabled = false;
                return;
            }

            if (m_ToAnimate == null)
            {
                Debug.LogWarning($"No timeline configured on {name} - no animation will be played.", this);
                enabled = false;
                return;
            }

            interactable.selectEntered.AddListener(OnSelect);
            interactable.selectExited.AddListener(OnDeselect);
        }

        void Update()
        {
            if (m_Animating && m_Controller != null)
            {
                var floatValue = m_Controller.activateInteractionState.value;
                m_ToAnimate.time = floatValue;
            }
        }

        void OnSelect(SelectEnterEventArgs args)
        {
            var controllerInteractor = args.interactorObject as XRBaseControllerInteractor;
            if (controllerInteractor == null)
            {
                Debug.LogWarning($"Selected by {args.interactorObject.transform.name}, which is not an XRBaseControllerInteractor", this);
                return;
            }

            m_Controller = controllerInteractor.xrController;
            if (m_Controller == null)
            {
                Debug.LogWarning($"Selected by {controllerInteractor.name}, which does not have a valid XRBaseController", this);
                return;
            }

            m_ToAnimate.Play();
            m_Animating = true;
        }

        void OnDeselect(SelectExitEventArgs args)
        {
            m_Animating = false;
            m_ToAnimate.Stop();
            m_Controller = null;
        }
    }
}
