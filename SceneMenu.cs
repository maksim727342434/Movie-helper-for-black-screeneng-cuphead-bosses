using UnityEngine;
using System.Collections;

namespace CupheadMod
{
    public class SceneMenu : MonoBehaviour
    {
        private bool menuOpen = false;
        private int selectedIndex = 0;
        private bool devilPhase2 = false;
        private bool blackAndWhite = false;
        private bool twoStrip = false;
        private bool showVersionPopup = false;
        private float originalOrtho = 5f;
        private bool camPosStored = false;
        private Camera mainCam;
        private GUIStyle boxStyle;
        private GUIStyle labelStyle;
        private GUIStyle selectedStyle;
        private GUIStyle titleStyle;
        private GUIStyle hintStyle;
        private GUIStyle buttonStyle;
        private GUIStyle buttonSelectedStyle;
        private GUIStyle popupStyle;
        private GUIStyle popupTitleStyle;
        private GUIStyle popupTextStyle;
        private bool stylesInit = false;

        private string[] levels = {
            "scene_level_veggies",
            "scene_level_slime",
            "scene_level_frogs",
            "scene_level_flower",
            "scene_level_flying_genie",
            "scene_level_pirate",
            "scene_level_mouse",
            "scene_level_robot",
            "scene_level_train",
            "scene_level_dice_palace_main",
            "scene_level_dice_palace_booze",
            "scene_level_dice_palace_chips",
            "scene_level_dice_palace_cigar",
            "scene_level_dice_palace_domino",
            "scene_level_dice_palace_rabbit",
            "scene_level_dice_palace_flying_horse",
            "scene_level_dice_palace_roulette",
            "scene_level_dice_palace_eight_ball",
            "scene_level_dice_palace_flying_memory",
        };

        private string[] names = {
            "The Root Pack",
            "Goopy Le Grande",
            "Ribby and Croaks",
            "Cagney Carnation",
            "Djimmi The Great",
            "Captain Brineybeard",
            "Werner Werman",
            "Dr. Kahl's Robot",
            "Phantom Express",
            "King Dice",
            "Tipsy Troop",
            "Chips Bettigan",
            "Mr.Whezzy",
            "Pip And Dot",
            "Hopus Pocus",
            "Phear Lap",
            "Piroulette",
            "Mangosteen",
            "Mr.Chimes",
        };

        private string[] buttons = {
            "Zoom Out",
            "Zoom In",
            "Reset Camera",
            "B&W: OFF sadly not working",
            "2-Strip: OFF",
            "Unity Version",
        };

        int TotalItems => names.Length + buttons.Length;

        void InitStyles()
        {
            Texture2D bgTex = new Texture2D(1, 1);
            bgTex.SetPixel(0, 0, new Color(0.15f, 0.02f, 0.02f, 0.95f));
            bgTex.Apply();

            Texture2D selectedTex = new Texture2D(1, 1);
            selectedTex.SetPixel(0, 0, new Color(0.6f, 0.4f, 0f, 0.6f));
            selectedTex.Apply();

            Texture2D buttonTex = new Texture2D(1, 1);
            buttonTex.SetPixel(0, 0, new Color(0.3f, 0.1f, 0.1f, 1f));
            buttonTex.Apply();

            Texture2D buttonSelTex = new Texture2D(1, 1);
            buttonSelTex.SetPixel(0, 0, new Color(0.6f, 0.4f, 0f, 1f));
            buttonSelTex.Apply();

            Texture2D popupTex = new Texture2D(1, 1);
            popupTex.SetPixel(0, 0, new Color(0.05f, 0.05f, 0.05f, 0.98f));
            popupTex.Apply();

            boxStyle = new GUIStyle(GUI.skin.box);
            boxStyle.normal.background = bgTex;

            titleStyle = new GUIStyle(GUI.skin.label);
            titleStyle.fontSize = 22;
            titleStyle.fontStyle = FontStyle.Bold;
            titleStyle.normal.textColor = new Color(1f, 0.85f, 0.1f);
            titleStyle.alignment = TextAnchor.MiddleCenter;

            labelStyle = new GUIStyle(GUI.skin.label);
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = new Color(0.95f, 0.85f, 0.7f);

            selectedStyle = new GUIStyle(GUI.skin.label);
            selectedStyle.fontSize = 15;
            selectedStyle.fontStyle = FontStyle.Bold;
            selectedStyle.normal.textColor = new Color(1f, 0.85f, 0.1f);
            selectedStyle.normal.background = selectedTex;

            hintStyle = new GUIStyle(GUI.skin.label);
            hintStyle.fontSize = 12;
            hintStyle.normal.textColor = new Color(0.7f, 0.6f, 0.4f);
            hintStyle.alignment = TextAnchor.MiddleCenter;

            buttonStyle = new GUIStyle(GUI.skin.label);
            buttonStyle.fontSize = 14;
            buttonStyle.normal.textColor = new Color(0.95f, 0.85f, 0.7f);
            buttonStyle.normal.background = buttonTex;
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            buttonSelectedStyle = new GUIStyle(GUI.skin.label);
            buttonSelectedStyle.fontSize = 14;
            buttonSelectedStyle.fontStyle = FontStyle.Bold;
            buttonSelectedStyle.normal.textColor = new Color(1f, 0.85f, 0.1f);
            buttonSelectedStyle.normal.background = buttonSelTex;
            buttonSelectedStyle.alignment = TextAnchor.MiddleCenter;

            popupStyle = new GUIStyle(GUI.skin.box);
            popupStyle.normal.background = popupTex;

            popupTitleStyle = new GUIStyle(GUI.skin.label);
            popupTitleStyle.fontSize = 18;
            popupTitleStyle.fontStyle = FontStyle.Bold;
            popupTitleStyle.normal.textColor = new Color(1f, 0.85f, 0.1f);
            popupTitleStyle.alignment = TextAnchor.MiddleCenter;

            popupTextStyle = new GUIStyle(GUI.skin.label);
            popupTextStyle.fontSize = 15;
            popupTextStyle.normal.textColor = Color.white;
            popupTextStyle.alignment = TextAnchor.MiddleCenter;

            stylesInit = true;
        }

        Camera GetCam()
        {
            if (mainCam == null)
                mainCam = Camera.main;
            return mainCam;
        }

        void ApplyZoom(float amount)
        {
            Camera cam = GetCam();
            if (cam == null) return;
            if (!camPosStored)
            {
                originalOrtho = cam.orthographicSize;
                camPosStored = true;
            }
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + amount, 1f, 20f);
        }

        void ResetCamera()
        {
            Camera cam = GetCam();
            if (cam == null) return;
            cam.orthographicSize = originalOrtho;
            camPosStored = false;
        }

        void PressButton(int btnIndex)
        {
            switch (btnIndex)
            {
                case 0: ApplyZoom(1f); break;
                case 1: ApplyZoom(-1f); break;
                case 2: ResetCamera(); break;
                case 3:
                    blackAndWhite = !blackAndWhite;
                    if (blackAndWhite) twoStrip = false;
                    buttons[3] = blackAndWhite ? "B&W: ON" : "B&W: OFF";
                break;
                case 4:
                    twoStrip = !twoStrip;
                    if (twoStrip) blackAndWhite = false;
                    buttons[4] = twoStrip ? "2-Strip: ON" : "2-Strip: OFF";
                break;
                case 5:
                    showVersionPopup = !showVersionPopup;
                    break;
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.M))
                menuOpen = !menuOpen;

            if (showVersionPopup && Input.GetKeyDown(KeyCode.Return))
            {
                showVersionPopup = false;
                return;
            }

            if (!menuOpen) return;

            if (Input.GetKeyDown(KeyCode.UpArrow))
                selectedIndex = (selectedIndex - 1 + TotalItems) % TotalItems;

            if (Input.GetKeyDown(KeyCode.DownArrow))
                selectedIndex = (selectedIndex + 1) % TotalItems;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (selectedIndex < names.Length)
                {
                    menuOpen = false;
                    devilPhase2 = false;
                    camPosStored = false;
                    SceneLoader.LoadScene(levels[selectedIndex],
                                          SceneLoader.Transition.Fade,
                                          SceneLoader.Transition.Fade);
                    StartCoroutine(DisableBackground());
                }
                else
                {
                    PressButton(selectedIndex - names.Length);
                }
            }
        }

        IEnumerator DisableBackground()
        {
            string[] bgNames = {
                "Background", "background", "BackGround",
                "BG", "bg", "LevelBackground", "Foreground",
                "Spire", "TreeSpawner", "Midground", "Backgrounds",
                "Foregrounds", "LevelArt", "Clouds", "Sky_sea",
                "Phase2Background", "AlternateForeground", "Water",
                "BackgroundPhase1and2",
            };

            while (true)
            {
                yield return new WaitForSeconds(1f);

                string currentScene = Application.loadedLevelName;
                if (currentScene.StartsWith("scene_map_world"))
                    continue;

                foreach (string name in bgNames)
                {
                    GameObject bg = GameObject.Find(name);
                    if (bg != null && bg.activeSelf)
                    {
                        bg.SetActive(false);
                        Plugin.Log.LogInfo("Фон отключён: " + name);
                    }
                }

                GameObject phase2 = GameObject.Find("Phase2Background");
                if (phase2 != null && !devilPhase2)
                {
                    devilPhase2 = true;
                    GameObject phase1 = GameObject.Find("Phase1Background");
                    if (phase1 != null)
                    {
                        phase1.SetActive(false);
                        Plugin.Log.LogInfo("Дьявол фаза 2 — Phase1Background отключён");
                    }
                }
            }
        }

        void OnGUI()
        {
            if (!stylesInit) InitStyles();

            if (blackAndWhite || twoStrip)
            {
                Texture2D filterTex = new Texture2D(1, 1);
                if (blackAndWhite)
                    filterTex.SetPixel(0, 0, new Color(0f, 0f, 0f, 0.6f));
                else
                    filterTex.SetPixel(0, 0, new Color(0.9f, 0.4f, 0f, 0.35f));
                filterTex.Apply();
                GUI.depth = -100;
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), filterTex);
                GUI.depth = 0;
            }

            if (showVersionPopup)
            {
                float pw = 340;
                float ph = 160;
                float px = Screen.width / 2f - pw / 2f;
                float py = Screen.height / 2f - ph / 2f;
                GUI.depth = -200;
                GUI.Box(new Rect(px - 4, py - 4, pw + 8, ph + 8), "");
                GUI.Box(new Rect(px, py, pw, ph), "", popupStyle);
                GUI.Label(new Rect(px, py + 15, pw, 30), "✦ Engine Info ✦", popupTitleStyle);
                GUI.Label(new Rect(px, py + 55, pw, 30), "Unity Version:", popupTextStyle);
                GUI.Label(new Rect(px, py + 85, pw, 30), Application.unityVersion, popupTitleStyle);
                GUI.Label(new Rect(px, py + 125, pw, 25), "[ ENTER ] Close", hintStyle);
                GUI.depth = 0;
                return;
            }

            if (!menuOpen) return;

            float w = 360;
            float h = 750;
            float x = Screen.width / 2f - w / 2f;
            float y = Screen.height / 2f - h / 2f;

            GUI.Box(new Rect(x - 4, y - 4, w + 8, h + 8), "");
            GUI.Box(new Rect(x, y, w, h), "", boxStyle);

            GUI.Label(new Rect(x, y + 15, w, 30), "✦ Cuphead Black Screen Tool ✦", titleStyle);
            GUI.Label(new Rect(x + 20, y + 50, w - 40, 20), "- - - - - - - - - - - - - - - - - -", hintStyle);

            for (int i = 0; i < names.Length; i++)
            {
                Rect r = new Rect(x + 20, y + 75 + i * 30, w - 40, 26);
                bool sel = selectedIndex == i;
                GUI.Label(r, (sel ? "  ► " : "      ") + names[i], sel ? selectedStyle : labelStyle);
            }

            float btnY = y + 75 + names.Length * 30 + 5;
            GUI.Label(new Rect(x + 20, btnY, w - 40, 20), "- - - - - - - - - - - - - - - - - -", hintStyle);
            btnY += 25;

            for (int i = 0; i < buttons.Length; i++)
            {
                int globalIdx = names.Length + i;
                bool sel = selectedIndex == globalIdx;
                Rect r = new Rect(x + 20, btnY + i * 33, w - 40, 26);
                GUI.Label(r, (sel ? "► " : "   ") + buttons[i], sel ? buttonSelectedStyle : buttonStyle);
            }

            btnY += buttons.Length * 33 + 5;
            GUI.Label(new Rect(x + 20, btnY, w - 40, 20), "- - - - - - - - - - - - - - - - - -", hintStyle);
            btnY += 20;
            GUI.Label(new Rect(x, btnY, w, 25), "[ ↑↓ ] Navigation      [ ENTER ] Select", hintStyle);
        }
    }
}
