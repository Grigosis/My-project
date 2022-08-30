using Assets.Scripts.AbstractNodeEditor;
using Slime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace DS.Windows
{

    public class ANEWindow : EditorWindow
    {
        private ANEGraph graphView;

        private readonly string defaultFileName = "DialoguesFileName";

        private static TextField fileNameTextField;
        private Button saveButton;
        private Button miniMapButton;

        private SplitView splitView;

        [MenuItem("Window/ANE/Editor")]
        public static void Open()
        {
            GetWindow<ANEWindow>("Dialogue Graph");
        }

        private void OnEnable()
        {
            var button = new Button();
            button.text = "Push me23333";
            
            graphView = new ANEGraph(this);
            graphView.StretchToParentSize();
            
            splitView = new SplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(button);
            splitView.Add(graphView);
            
            rootVisualElement.Add(splitView);
            
            Toolbar toolbar = new Toolbar();

            fileNameTextField = Helper.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = Helper.CreateButton("Save", () => Save());

            Button loadButton = Helper.CreateButton("Load", () => Load());
            Button clearButton = Helper.CreateButton("Clear", () => Clear());

            miniMapButton = Helper.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(miniMapButton);

            toolbar.AddStyleSheets("DialogueSystem/DSToolbarStyles.uss");

            rootVisualElement.Add(toolbar);

            rootVisualElement.AddStyleSheets("DialogueSystem/DSVariables.uss");
        }

        private void Save()
        {
            /*if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("Invalid file name.", "Please ensure the file name you've typed in is valid.", "Roger!");

                return;
            }

            DSIOUtility.Initialize(graphView, fileNameTextField.value);
            DSIOUtility.Save();*/
        }

        private void Load()
        {
            /*string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/Editor/DialogueSystem/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            Clear();

            DSIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            DSIOUtility.Load();*/
        }

        private void Clear()
        {
            /*graphView.ClearGraph();*/
        }

        private void ToggleMiniMap()
        {
            graphView.ToggleMiniMap();

            miniMapButton.ToggleInClassList("ds-toolbar__button__selected");
        }

        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }

        private Editor editor;
        public void OnNodeSelected(ANENode node)
        {
            Object.DestroyImmediate(editor);
            editor = Editor.CreateEditor(node.Node);
            var container = new IMGUIContainer(() =>
            {
                editor.OnInspectorGUI();
            });
            splitView.Add(container);

        }
    }
}