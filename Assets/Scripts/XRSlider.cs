using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

namespace Unity.XRContent.Interaction
{
    /// <summary>
    /// An interactable that follows the position of the interactor on a single axis
    /// </summary>
    public class XRSlider : XRBaseInteractable
    {
        public delegate void DifficultyChangeDelegate(string difficulty);
        public DifficultyChangeDelegate onDifficultyChanged;

        [SerializeField]
        [Tooltip("The object that is visually grabbed and manipulated")]
        public Transform handle;

        [SerializeField]
        [Tooltip("The value of the slider")]
        [Range(0.0f, 1.0f)]
        float m_Value = 0.5f;

        [SerializeField]
        [Tooltip("The offset of the slider at value '1'")]
        float m_MaxPosition = 0.5f;

        [SerializeField]
        [Tooltip("The offset of the slider at value '0'")]
        float m_MinPosition = -0.5f;

        IXRSelectInteractor m_Interactor;

        /// <summary>
        /// The value of the slider
        /// </summary>
        public float Value
        {
            get { return m_Value; }
            set
            {
                SetValue(value);
                SetSliderPosition(value);
            }
        }

        void Start()
        {
            SetValue(m_Value);
            SetSliderPosition(m_Value);
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            selectEntered.AddListener(StartGrab);
            selectExited.AddListener(EndGrab);
        }

        protected override void OnDisable()
        {
            selectEntered.RemoveListener(StartGrab);
            selectExited.RemoveListener(EndGrab);
            base.OnDisable();
        }

        void StartGrab(SelectEnterEventArgs args)
        {
            m_Interactor = args.interactorObject;
            UpdateSliderPosition();
        }

        void EndGrab(SelectExitEventArgs args)
        {
            m_Interactor = null;
        }

        public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
        {
            base.ProcessInteractable(updatePhase);

            if (updatePhase == XRInteractionUpdateOrder.UpdatePhase.Dynamic)
            {
                if (isSelected)
                {
                    UpdateSliderPosition();
                }
            }
        }

        void UpdateSliderPosition()
        {
            // Put anchor position into slider space
            var localPosition = transform.InverseTransformPoint(m_Interactor.GetAttachTransform(this).position);
            var sliderValue = Mathf.Clamp01((localPosition.x - m_MinPosition) / (m_MaxPosition - m_MinPosition));
            SetValue(sliderValue);
            SetSliderPosition(sliderValue);
        }

        void SetSliderPosition(float value)
        {
            if (handle == null)
                return;

            var handlePos = handle.localPosition;
            handlePos.x = Mathf.Lerp(m_MinPosition, m_MaxPosition, value);
            handle.localPosition = handlePos;
        }

        void SetValue(float value)
        {
            m_Value = value;
            //m_OnValueChange.Invoke(m_Value);
        }

        public void ChangeDifficulty(string difficulty)
        {
            onDifficultyChanged.Invoke(difficulty);
        }

        void OnDrawGizmosSelected()
        {
            var sliderMinPoint = transform.TransformPoint(new Vector3(0.0f, 0.0f, m_MinPosition));
            var sliderMaxPoint = transform.TransformPoint(new Vector3(0.0f, 0.0f, m_MaxPosition));

            Gizmos.color = Color.green;
            Gizmos.DrawLine(sliderMinPoint, sliderMaxPoint);
        }

        void OnValidate()
        {
            SetSliderPosition(m_Value);
        }
    }
}
