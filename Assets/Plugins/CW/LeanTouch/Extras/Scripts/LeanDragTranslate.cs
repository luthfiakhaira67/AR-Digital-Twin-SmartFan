using UnityEngine;
using CW.Common;

namespace Lean.Touch
{
    /// <summary>This component allows you to translate the current GameObject relative to the camera using the finger drag gesture.</summary>
    [HelpURL(LeanTouch.HelpUrlPrefix + "LeanDragTranslate")]
    [AddComponentMenu(LeanTouch.ComponentPathPrefix + "Drag Translate")]
    public class LeanDragTranslate : MonoBehaviour
    {
        /// <summary>The method used to find fingers to use with this component. See LeanFingerFilter documentation for more information.</summary>
        public LeanFingerFilter Use = new LeanFingerFilter(true);

        /// <summary>The camera the translation will be calculated using.
        /// None/null = MainCamera.</summary>
        public Camera Camera { set { _camera = value; } get { return _camera; } }
        [SerializeField] private Camera _camera;

        /// <summary>The movement speed will be multiplied by this.
        /// -1 = Inverted Controls.</summary>
        public float Sensitivity { set { sensitivity = value; } get { return sensitivity; } }
        [SerializeField] private float sensitivity = 1.0f;

        /// <summary>If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.
        /// -1 = Instantly change.
        /// 1 = Slowly change.
        /// 10 = Quickly change.</summary>
        public float Damping { set { damping = value; } get { return damping; } }
        [SerializeField] protected float damping = -1.0f;

        /// <summary>This allows you to control how much momentum is retained when the dragging fingers are all released.
        /// NOTE: This requires <b>Dampening</b> to be above 0.</summary>
        public float Inertia { set { inertia = value; } get { return inertia; } }
        [SerializeField][Range(0.0f, 1.0f)] private float inertia;

        [SerializeField]
        private Vector3 remainingTranslation;

        public void AddFinger(LeanFinger finger)
        {
            Use.AddFinger(finger);
        }

        public void RemoveFinger(LeanFinger finger)
        {
            Use.RemoveFinger(finger);
        }

        public void RemoveAllFingers()
        {
            Use.RemoveAllFingers();
        }

#if UNITY_EDITOR
		protected virtual void Reset()
		{
			Use.UpdateRequiredSelectable(gameObject);
		}
#endif

        protected virtual void Awake()
        {
            Use.UpdateRequiredSelectable(gameObject);
        }

        protected virtual void Update()
        {
            var oldPosition = transform.localPosition;
            var fingers = Use.UpdateAndGetFingers();
            var screenDelta = LeanGesture.GetScreenDelta(fingers);

            if (screenDelta != Vector2.zero)
            {
                if (transform is RectTransform)
                {
                    TranslateUI(screenDelta);
                }
                else
                {
                    Translate(screenDelta);
                }
            }

            remainingTranslation += transform.localPosition - oldPosition;
            var factor = CwHelper.DampenFactor(Damping, Time.deltaTime);
            var newRemainingTranslation = Vector3.Lerp(remainingTranslation, Vector3.zero, factor);
            transform.localPosition = oldPosition + remainingTranslation - newRemainingTranslation;

            if (fingers.Count == 0 && inertia > 0.0f && Damping > 0.0f)
            {
                newRemainingTranslation = Vector3.Lerp(newRemainingTranslation, remainingTranslation, inertia);
            }

            remainingTranslation = newRemainingTranslation;
        }

        private void TranslateUI(Vector2 screenDelta)
        {
            // --- MODIFIKASI SAFETY CHECK BY GEMINI ---
            var finalCamera = _camera;

            // Jika kamera di inspector kosong, coba cari dari Canvas worldCamera
            if (finalCamera == null)
            {
                var canvas = transform.GetComponentInParent<Canvas>();
                if (canvas != null && canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                {
                    finalCamera = canvas.worldCamera;
                }
            }

            // Jika masih kosong, coba cari Camera.main (Pastikan ARCamera ada Tag MainCamera!)
            if (finalCamera == null)
            {
                finalCamera = Camera.main;
            }

            // JIKA KAMERA TETAP TIDAK KETEMU, JANGAN LANJUT (BIAR GAK FREEZE)
            if (finalCamera == null)
            {
                return;
            }

            // Pastikan parent ada untuk perhitungan World Point
            var parentRect = transform.parent as RectTransform;
            if (parentRect == null) return;

            // Screen position of the transform
            var screenPoint = RectTransformUtility.WorldToScreenPoint(finalCamera, transform.position);

            // Add the deltaPosition
            screenPoint += screenDelta * Sensitivity;

            // Convert back to world space
            var worldPoint = default(Vector3);

            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(parentRect, screenPoint, finalCamera, out worldPoint) == true)
            {
                transform.position = worldPoint;
            }
        }

        private void Translate(Vector2 screenDelta)
        {
            var camera = CwHelper.GetCamera(this._camera, gameObject);

            if (camera != null)
            {
                var screenPoint = camera.WorldToScreenPoint(transform.position);
                screenPoint += (Vector3)screenDelta * Sensitivity;
                transform.position = camera.ScreenToWorldPoint(screenPoint);
            }
            else
            {
                Debug.LogError("Gagal menemukan Kamera! Tag ARCamera kamu sebagai 'MainCamera' atau tarik ke Inspector.", this);
            }
        }
    }
}

#if UNITY_EDITOR
namespace Lean.Touch.Editor
{
	using UnityEditor;
	using TARGET = LeanDragTranslate;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(TARGET), true)]
	public class LeanDragTranslate_Editor : CwEditor
	{
		protected override void OnInspector()
		{
			TARGET tgt; TARGET[] tgts; GetTargets(out tgt, out tgts);

			Draw("Use");
			Draw("_camera", "The camera the translation will be calculated using.\n\nNone/null = MainCamera.");
			Draw("sensitivity", "The movement speed will be multiplied by this.\n\n-1 = Inverted Controls.");
			Draw("damping", "If you want this component to change smoothly over time, then this allows you to control how quick the changes reach their target value.\n\n-1 = Instantly change.\n\n1 = Slowly change.\n\n10 = Quickly change.");
			Draw("inertia", "This allows you to control how much momentum is retained when the dragging fingers are all released.\n\nNOTE: This requires <b>Damping</b> to be above 0.");
		}
	}
}
#endif