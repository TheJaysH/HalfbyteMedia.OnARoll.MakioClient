using HalfbyteMedia.OnARoll.MakioClient.Config;
using HalfbyteMedia.OnARoll.MakioClient.UI.Panels;
using UniverseLib.UI;
using UniverseLib.UI.Models;

namespace HalfbyteMedia.OnARoll.MakioClient.UI
{
    public static class UIManager
    {
        public enum Panels
        {
            Freecam,
            Utils,
            Options,
            Engine,
            Debug,
            AutoCompleter
        }

        public enum VerticalAnchor
        {
            Top,
            Bottom
        }

        public static VerticalAnchor NavbarAnchor = VerticalAnchor.Top;

        public static bool Initializing { get; internal set; } = true;

        internal static UIBase UiBase { get; private set; }
        public static GameObject UIRoot => UiBase?.RootObject;
        public static RectTransform UIRootRect { get; private set; }
        public static Canvas UICanvas { get; private set; }

        internal static readonly Dictionary<Panels, Panel> UIPanels = new();

        public static RectTransform NavBarRect;
        public static GameObject NavbarTabButtonHolder;
        private static readonly Vector2 NAVBAR_DIMENSIONS = new(1020f, 35f);

        private static ButtonRef closeBtn;
        //private static TimeScaleWidget timeScaleWidget;

        private static int lastScreenWidth;
        private static int lastScreenHeight;

        public static bool ShowMenu
        {
            get => UiBase != null && UiBase.Enabled;
            set
            {
                if (UiBase == null || !UIRoot || UiBase.Enabled == value)
                    return;

                UniversalUI.SetUIActive(MakioClient.GUID, value);
                //UniversalUI.SetUIActive(MouseInspector.UIBaseGUID, value);
            }
        }

        // Initialization

        internal static void InitUI()
        {
            UiBase = UniversalUI.RegisterUI<UIBase>(MakioClient.GUID, Update);

            UIRootRect = UIRoot.GetComponent<RectTransform>();
            UICanvas = UIRoot.GetComponent<Canvas>();

            DisplayManager.Init();

            Display display = DisplayManager.ActiveDisplay;
            lastScreenWidth = display.renderingWidth;
            lastScreenHeight = display.renderingHeight;

            // Create UI.
            CreateTopNavBar();

            UIPanels.Add(Panels.AutoCompleter, new AutoCompleteModal(UiBase));
            UIPanels.Add(Panels.Freecam, new FreeCamPanel(UiBase));
            //UIPanels.Add(Panels.Utils, new UtilsPanel(UiBase));
            UIPanels.Add(Panels.Options, new OptionsPanel(UiBase));
            UIPanels.Add(Panels.Engine, new EnginePanel(UiBase));
            UIPanels.Add(Panels.Debug, new DebugPanel(UiBase));

            // Failsafe fix, in some games all dropdowns displayed values are blank on startup for some reason.
            foreach (Dropdown dropdown in UIRoot.GetComponentsInChildren<Dropdown>(true))
                dropdown.RefreshShownValue();

            Initializing = false;

            if (ConfigManager.Hide_On_Startup.Value)
                ShowMenu = false;
        }

        // Main UI Update loop

        public static void Update()
        {
            if (!UIRoot)
                return;


            //// Check forceUnlockMouse toggle
            //if (Input.GetKeyDown(ConfigManager.Force_Unlock_Toggle.Value))
            //    UniverseLib.Config.ConfigManager.Force_Unlock_Mouse = !UniverseLib.Config.ConfigManager.Force_Unlock_Mouse;

            // check screen dimension change
            Display display = DisplayManager.ActiveDisplay;
            if (display.renderingWidth != lastScreenWidth || display.renderingHeight != lastScreenHeight)
                OnScreenDimensionsChanged();

            foreach(var panel in UIPanels)
            {
                panel.Value.Update();
            }
        }

        // Panels

        public static Panel GetPanel(Panels panel) => UIPanels[panel];

        public static T GetPanel<T>(Panels panel) where T : Panel => (T)UIPanels[panel];

        public static void TogglePanel(Panels panel)
        {
            Panel uiPanel = GetPanel(panel);
            SetPanelActive(panel, !uiPanel.Enabled);
        }

        public static void SetPanelActive(Panels panelType, bool active)
        {
            GetPanel(panelType).SetActive(active);
        }

        public static void SetPanelActive(Panel panel, bool active)
        {
            panel.SetActive(active);
        }

        // navbar

        public static void SetNavBarAnchor()
        {
            switch (NavbarAnchor)
            {
                case VerticalAnchor.Top:
                    NavBarRect.anchorMin = new Vector2(0.5f, 1f);
                    NavBarRect.anchorMax = new Vector2(0.5f, 1f);
                    NavBarRect.anchoredPosition = new Vector2(NavBarRect.anchoredPosition.x, 0);
                    NavBarRect.sizeDelta = NAVBAR_DIMENSIONS;
                    break;

                case VerticalAnchor.Bottom:
                    NavBarRect.anchorMin = new Vector2(0.5f, 0f);
                    NavBarRect.anchorMax = new Vector2(0.5f, 0f);
                    NavBarRect.anchoredPosition = new Vector2(NavBarRect.anchoredPosition.x, 35);
                    NavBarRect.sizeDelta = NAVBAR_DIMENSIONS;
                    break;
            }
        }

        // listeners

        private static void OnScreenDimensionsChanged()
        {
            Display display = DisplayManager.ActiveDisplay;
            lastScreenWidth = display.renderingWidth;
            lastScreenHeight = display.renderingHeight;

            foreach (KeyValuePair<Panels, Panel> panel in UIPanels)
            {
                panel.Value.EnsureValidSize();
                panel.Value.EnsureValidPosition();
                panel.Value.Dragger.OnEndResize();
            }
        }

        private static void OnCloseButtonClicked()
        {
            ShowMenu = false;
        }

        private static void Master_Toggle_OnValueChanged(KeyCode val)
        {
            closeBtn.ButtonText.text = val.ToString();
        }



        // UI Construction

        private static void CreateTopNavBar()
        {
            GameObject navbarPanel = UIFactory.CreateUIObject("MainNavbar", UIRoot);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(navbarPanel, false, false, true, true, 5, 4, 4, 4, 4, TextAnchor.MiddleCenter);
            navbarPanel.AddComponent<Image>().color = new Color(0.1f, 0.1f, 0.1f);
            NavBarRect = navbarPanel.GetComponent<RectTransform>();
            NavBarRect.pivot = new Vector2(0.5f, 1f);

            NavbarAnchor = VerticalAnchor.Top;
            SetNavBarAnchor();
            //ConfigManager.Main_Navbar_Anchor.OnValueChanged += (VerticalAnchor val) =>
            //{
            //    NavbarAnchor = val;
            //    SetNavBarAnchor();
            //};

            // UnityExplorer title

            string titleTxt = $"{MakioClient.NAME} <i><color=grey>{MakioClient.VERSION}</color></i>";
            Text title = UIFactory.CreateLabel(navbarPanel, "Title", titleTxt, TextAnchor.MiddleCenter, default, true, 14);
            UIFactory.SetLayoutElement(title.gameObject, minWidth: 75, flexibleWidth: 0);

            // panel tabs

            NavbarTabButtonHolder = UIFactory.CreateUIObject("NavTabButtonHolder", navbarPanel);
            UIFactory.SetLayoutElement(NavbarTabButtonHolder, minHeight: 25, flexibleHeight: 999, flexibleWidth: 999);
            UIFactory.SetLayoutGroup<HorizontalLayoutGroup>(NavbarTabButtonHolder, false, true, true, true, 4, 2, 2, 2, 2);

            //spacer
            GameObject spacer = UIFactory.CreateUIObject("Spacer", navbarPanel);
            UIFactory.SetLayoutElement(spacer, minWidth: 15);

            // Hide menu button

            closeBtn = UIFactory.CreateButton(navbarPanel, "CloseButton", ConfigManager.Master_Toggle.Value.ToString());
            UIFactory.SetLayoutElement(closeBtn.Component.gameObject, minHeight: 25, minWidth: 60, flexibleWidth: 0);
            RuntimeHelper.SetColorBlock(closeBtn.Component, new Color(0.63f, 0.32f, 0.31f),
                new Color(0.81f, 0.25f, 0.2f), new Color(0.6f, 0.18f, 0.16f));

            ConfigManager.Master_Toggle.OnValueChanged += Master_Toggle_OnValueChanged;
            closeBtn.OnClick += OnCloseButtonClicked;
        }
    }
}
