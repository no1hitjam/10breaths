using Helpers;
using Input;
using UnityEngine;

public class CameraControl : MonoBehaviour {
    private const float Sensitivity = 0.1f;
    private const float Smoothing = 2.0f;

    [SerializeField] private GameObject headPivot;

    public static CameraControl Instance;

    private Vector2 _mouseLook;
    private Vector2 _smoothLookDelta;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        if (!DebugFlags.OnlyMoveCameraWhenHoldingLeftMouse) {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update() {
        if (DebugFlags.OnlyMoveCameraWhenHoldingLeftMouse && !Inputs.PrimaryButton.IsDown) {
            return;
        }

        var mouseDelta = Inputs.LookDelta;
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(Sensitivity * Smoothing, Sensitivity * Smoothing));
        
        this._smoothLookDelta.x = Mathf.Lerp(this._smoothLookDelta.x, mouseDelta.x, 1f / Smoothing);
        this._smoothLookDelta.y = Mathf.Lerp(this._smoothLookDelta.y, mouseDelta.y, 1f / Smoothing);
        this._mouseLook += this._smoothLookDelta;

        this.headPivot.transform.localRotation = Quaternion.AngleAxis(-this._mouseLook.y, Vector3.right);
        this.transform.localRotation = Quaternion.AngleAxis(this._mouseLook.x, this.transform.up);
    }
}