using UniverseLib.Input;
using UniverseLib.UI;
using UniverseLib.UI.Models;
using static HalfbyteMedia.OnARoll.MakioClient.Mod.ModPatches;

namespace HalfbyteMedia.OnARoll.MakioClient.UI.Panels
{
    internal class FreeCamPanel : Panel
    {
        public FreeCamPanel(UIBase owner) : base(owner)
        {
        }

        public override string Name => "Freecam";
        public override UIManager.Panels PanelType => UIManager.Panels.Freecam;
        public override int MinWidth => 400;
        public override int MinHeight => 320;
        public override Vector2 DefaultAnchorMin => new(0.4f, 0.4f);
        public override Vector2 DefaultAnchorMax => new(0.6f, 0.6f);
        public override bool NavButtonWanted => true;
        public override bool ShouldSaveActiveState => true;

        internal static bool inFreeCamMode;
        internal static bool enablePlayerInput;
        internal static bool usingGameCamera;

        internal static Camera ourCamera;
        internal static Camera lastMainCamera;
        internal static FreeCamBehaviour freeCamScript;

        internal static float desiredMoveSpeed = 10f;
        internal static float scrollScale = 1.5f;

        internal static float originalCameraFov;
        internal static Vector3 originalCameraPosition;
        internal static Quaternion originalCameraRotation;

        internal static float? currentUserCameraFov;
        internal static Vector3? currentUserCameraPosition;
        internal static Quaternion? currentUserCameraRotation;

        internal static Vector3 previousMousePosition;

        internal static Vector3 lastSetCameraPosition;

        static ButtonRef startStopButton;
        static Toggle useGameCameraToggle;
        static Toggle enablePlayerInputToggle;
        static InputFieldRef positionInput;
        static InputFieldRef moveSpeedInput;

        internal static void BeginFreecam()
        {
            inFreeCamMode = true;

            Patch_BladeController.bladeController._globalsContainer.Inputs.inputActive = enablePlayerInput;

            previousMousePosition = Input.mousePosition;

            CacheMainCamera();
            SetupFreeCamera();
        }

        static void CacheMainCamera()
        {
            Camera currentMain = Camera.main;
            if (currentMain)
            {
                lastMainCamera = currentMain;
                originalCameraPosition = currentMain.transform.position;
                originalCameraRotation = currentMain.transform.rotation;
                originalCameraFov = currentMain.fieldOfView;

                if (currentUserCameraPosition == null)
                {
                    currentUserCameraPosition = currentMain.transform.position;
                    currentUserCameraRotation = currentMain.transform.rotation;
                }

                if (currentUserCameraFov == null)
                {
                    currentUserCameraFov = currentMain.fieldOfView;
                }
            }
            else
                originalCameraRotation = Quaternion.identity;
        }

        static void SetupFreeCamera()
        {
            if (useGameCameraToggle.isOn)
            {
                if (!lastMainCamera)
                {
                    MakioClient.LogWarning($"There is no previous Camera found, reverting to default Free Cam.");
                    useGameCameraToggle.isOn = false;
                }
                else
                {
                    usingGameCamera = true;
                    ourCamera = lastMainCamera;
                }
            }

            if (!useGameCameraToggle.isOn)
            {
                usingGameCamera = false;

                if (lastMainCamera)
                    lastMainCamera.enabled = false;
            }


            if (!ourCamera)
            {

                ourCamera = new GameObject("Freecam").AddComponent<Camera>();
                ourCamera.gameObject.tag = "MainCamera";
                GameObject.DontDestroyOnLoad(ourCamera.gameObject);
                ourCamera.gameObject.hideFlags = HideFlags.HideAndDontSave;

                //ourCamera.gameObject.AddComponent<MorePPEffects.RadialBlur>();
                //ourCamera.gameObject.AddComponent<DepthOfField>();
                //ourCamera.gameObject.AddComponent<PostProcessingBehaviour>();
            }

            if (!freeCamScript)
                freeCamScript = ourCamera.gameObject.AddComponent<FreeCamBehaviour>();

            ourCamera.transform.position = (Vector3)currentUserCameraPosition;
            ourCamera.transform.rotation = (Quaternion)currentUserCameraRotation;
            ourCamera.fieldOfView = (float)currentUserCameraFov;

            ourCamera.gameObject.SetActive(true);
            ourCamera.enabled = true;
        }

        internal static void EndFreecam()
        {
            inFreeCamMode = false;
            Patch_BladeController.bladeController._globalsContainer.Inputs.inputActive = true;

            if (usingGameCamera)
            {
                ourCamera = null;

                if (lastMainCamera)
                {
                    lastMainCamera.transform.position = originalCameraPosition;
                    lastMainCamera.transform.rotation = originalCameraRotation;
                }
            }

            if (ourCamera)
                ourCamera.gameObject.SetActive(false);

            if (freeCamScript)
            {
                GameObject.Destroy(freeCamScript);
                freeCamScript = null;
            }

            if (lastMainCamera)
                lastMainCamera.enabled = true;
        }

        static void SetCameraPosition(Vector3 pos)
        {
            if (!ourCamera || lastSetCameraPosition == pos)
                return;

            ourCamera.transform.position = pos;
            lastSetCameraPosition = pos;
        }

        internal static void UpdatePositionInput()
        {
            if (!ourCamera)
                return;

            if (positionInput.Component.isFocused)
                return;

            lastSetCameraPosition = ourCamera.transform.position;
            positionInput.Text = ParseUtility.ToStringForInput<Vector3>(lastSetCameraPosition);
        }

        // ~~~~~~~~ UI construction / callbacks ~~~~~~~~

        protected override void ConstructPanelContent()
        {
            startStopButton = UIFactory.CreateButton(ContentRoot, "ToggleButton", "Freecam");
            UIFactory.SetLayoutElement(startStopButton.GameObject, minWidth: 150, minHeight: 25, flexibleWidth: 9999);
            startStopButton.OnClick += StartStopButton_OnClick;
            SetToggleButtonState();

            AddSpacer(5);

            GameObject toggleObj = UIFactory.CreateToggle(ContentRoot, "UseGameCameraToggle", out useGameCameraToggle, out Text toggleText);
            UIFactory.SetLayoutElement(toggleObj, minHeight: 25, flexibleWidth: 9999);
            useGameCameraToggle.onValueChanged.AddListener(OnUseGameCameraToggled);
            useGameCameraToggle.isOn = false;
            toggleText.text = "Use Game Camera";

            AddSpacer(5);

            GameObject disableInputToggleObj = UIFactory.CreateToggle(ContentRoot, "EnablePlayerInput", out enablePlayerInputToggle, out Text enableInputToggleText);
            UIFactory.SetLayoutElement(disableInputToggleObj, minHeight: 25, flexibleWidth: 9999);
            enablePlayerInputToggle.onValueChanged.AddListener(OnDisablePlayerInputToggled);
            enablePlayerInputToggle.isOn = false;
            enableInputToggleText.text = "Enable Player Input";

            AddSpacer(5);

            GameObject posRow = AddInputField("Position", "Freecam Pos:", "eg. 0 0 0", out positionInput, PositionInput_OnEndEdit);

            ButtonRef resetPosButton = UIFactory.CreateButton(posRow, "ResetButton", "Reset");
            UIFactory.SetLayoutElement(resetPosButton.GameObject, minWidth: 70, minHeight: 25);
            resetPosButton.OnClick += OnResetPosButtonClicked;

            AddSpacer(5);

            AddInputField("MoveSpeed", "Move Speed:", "Default: 1", out moveSpeedInput, MoveSpeedInput_OnEndEdit);
            moveSpeedInput.Text = desiredMoveSpeed.ToString();

            AddSpacer(5);

            Text instructionsText = UIFactory.CreateLabel(ContentRoot, "Instructions", Properties.Resources.FreeCamControls, TextAnchor.UpperLeft);
            UIFactory.SetLayoutElement(instructionsText.gameObject, flexibleWidth: 9999, flexibleHeight: 9999);

        }


        void AddSpacer(int height)
        {
            GameObject obj = UIFactory.CreateUIObject("Spacer", ContentRoot);
            UIFactory.SetLayoutElement(obj, minHeight: height, flexibleHeight: 0);
        }

        GameObject AddInputField(string name, string labelText, string placeHolder, out InputFieldRef inputField, Action<string> onInputEndEdit)
        {
            GameObject row = UIFactory.CreateHorizontalGroup(ContentRoot, $"{name}_Group", false, false, true, true, 3, default, new(1, 1, 1, 0));

            Text posLabel = UIFactory.CreateLabel(row, $"{name}_Label", labelText);
            UIFactory.SetLayoutElement(posLabel.gameObject, minWidth: 100, minHeight: 25);

            inputField = UIFactory.CreateInputField(row, $"{name}_Input", placeHolder);
            UIFactory.SetLayoutElement(inputField.GameObject, minWidth: 125, minHeight: 25, flexibleWidth: 9999);
            inputField.Component.GetOnEndEdit().AddListener(onInputEndEdit);

            return row;
        }

        void StartStopButton_OnClick()
        {
            EventSystemHelper.SetSelectedGameObject(null);

            if (inFreeCamMode)
                EndFreecam();
            else
                BeginFreecam();

            SetToggleButtonState();
        }

        void SetToggleButtonState()
        {
            if (inFreeCamMode)
            {
                RuntimeHelper.SetColorBlockAuto(startStopButton.Component, new(0.4f, 0.2f, 0.2f));
                startStopButton.ButtonText.text = "End Freecam";
            }
            else
            {
                RuntimeHelper.SetColorBlockAuto(startStopButton.Component, new(0.2f, 0.4f, 0.2f));
                startStopButton.ButtonText.text = "Begin Freecam";
            }
        }

        void OnUseGameCameraToggled(bool value)
        {
            EventSystemHelper.SetSelectedGameObject(null);

            if (!inFreeCamMode)
                return;

            EndFreecam();
            BeginFreecam();
        }

        private void OnDisablePlayerInputToggled(bool value)
        {
            EventSystemHelper.SetSelectedGameObject(null);

            if (!inFreeCamMode)
                return;

            enablePlayerInput = value;
            EndFreecam();
            BeginFreecam();
        }

        void OnResetPosButtonClicked()
        {
            currentUserCameraPosition = originalCameraPosition;
            currentUserCameraRotation = originalCameraRotation;
            currentUserCameraFov = originalCameraFov;


            if (inFreeCamMode && ourCamera)
            {
                ourCamera.transform.position = (Vector3)currentUserCameraPosition;
                ourCamera.transform.rotation = (Quaternion)currentUserCameraRotation;
                ourCamera.fieldOfView = (float)currentUserCameraFov;
            }

            positionInput.Text = ParseUtility.ToStringForInput<Vector3>(originalCameraPosition);
        }

        void PositionInput_OnEndEdit(string input)
        {
            EventSystemHelper.SetSelectedGameObject(null);

            if (!ParseUtility.TryParse(input, out Vector3 parsed, out Exception parseEx))
            {
                MakioClient.LogWarning($"Could not parse position to Vector3: {parseEx.ReflectionExToString()}");
                UpdatePositionInput();
                return;
            }

            SetCameraPosition(parsed);
        }

        void MoveSpeedInput_OnEndEdit(string input)
        {
            EventSystemHelper.SetSelectedGameObject(null);

            if (!ParseUtility.TryParse(input, out float parsed, out Exception parseEx))
            {
                MakioClient.LogWarning($"Could not parse value: {parseEx.ReflectionExToString()}");
                moveSpeedInput.Text = desiredMoveSpeed.ToString();
                return;
            }

            desiredMoveSpeed = parsed;
        }
    }

    internal class FreeCamBehaviour : MonoBehaviour
    {
        internal void Update()
        {
            if (FreeCamPanel.inFreeCamMode)
            {
                if (!FreeCamPanel.ourCamera)
                {
                    FreeCamPanel.EndFreecam();
                    return;
                }

                Transform transform = FreeCamPanel.ourCamera.transform;


                FreeCamPanel.currentUserCameraPosition = transform.position;
                FreeCamPanel.currentUserCameraRotation = transform.rotation;
                FreeCamPanel.currentUserCameraFov = FreeCamPanel.ourCamera.fieldOfView;

                float zoom = Input.mouseScrollDelta.y * FreeCamPanel.scrollScale;

                if (zoom != 0)
                    FreeCamPanel.ourCamera.fieldOfView += -(Input.mouseScrollDelta.y * FreeCamPanel.scrollScale);

                float moveSpeed = FreeCamPanel.desiredMoveSpeed * Time.deltaTime;

                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                    moveSpeed *= 10f;

                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                    transform.position += transform.right * -1 * moveSpeed;

                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                    transform.position += transform.right * moveSpeed;

                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                    transform.position += transform.forward * moveSpeed;

                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                    transform.position += transform.forward * -1 * moveSpeed;

                if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.PageUp))
                    transform.position += transform.up * moveSpeed;

                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.PageDown))
                    transform.position += transform.up * -1 * moveSpeed;

                if (Input.GetMouseButton(1))
                {
                    Vector3 mouseDelta = Input.mousePosition - FreeCamPanel.previousMousePosition;

                    float newRotationX = transform.localEulerAngles.y + mouseDelta.x * 0.3f;
                    float newRotationY = transform.localEulerAngles.x - mouseDelta.y * 0.3f;
                    transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);

                }

                FreeCamPanel.UpdatePositionInput();

                FreeCamPanel.previousMousePosition = Input.mousePosition;
            }
        }
    }
}
