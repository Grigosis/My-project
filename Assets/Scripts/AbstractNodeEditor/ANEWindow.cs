﻿using System;
using SecondCycleGame.Assets.Scripts.ANEImpl.Impls;
using SecondCycleGame.Assets.Scripts.ObjectEditor;
using Slime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

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

        private ObjectEditorWrapper editor;
        
        private void OnEnable()
        {
            var button = new Button();
            button.text = "Push me23333";
            
            graphView = new ANEGraph(this, new DialogANEPresentation());
            graphView.StretchToParentSize();
            
            
            splitView = new SplitView(0, 200, TwoPaneSplitViewOrientation.Horizontal);
            splitView.Add(button);
            splitView.Add(graphView);
            
            rootVisualElement.Add(splitView);
            
            Toolbar toolbar = new Toolbar();

            fileNameTextField = Helper.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue;//.RemoveWhitespaces().RemoveSpecialCharacters()
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

            editor = new ObjectEditorWrapper(splitView);
        }

        private void Save()
        {
            graphView.Save("Assets/Database/Dialogs", "TestGraph");
        }

        private void Load()
        {
            graphView.Load("Assets/Database/Dialogs", "TestGraph");
        }

        private void Clear()
        {
            graphView.Clear();
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


        
        
        public class ObjectEditorWrapper
        {
            protected Action<object> OnEditorFinished;
            protected object ObjectToEdit;
            protected AbstractObjectEditor editor;
            protected VisualElement editorView;
            protected VisualElement editorContainterView;
            
            

            public ObjectEditorWrapper(VisualElement editorContainterView)
            {
                this.editorContainterView = editorContainterView;
            }
            
            public void RequestEditObject(object objectToEdit, Action<object> onEditorFinished = null)
            {
                FinishEdit();
                
                Debug.LogError($"RequestEditObject [{objectToEdit}]");
                ObjectToEdit = objectToEdit;
                OnEditorFinished = onEditorFinished;

                editorView = AbstractObjectEditor.CreateEditor(objectToEdit);

                editorContainterView.Add(editorView);
            }
            
            public void FinishEdit()
            {
                if (OnEditorFinished != null)
                {
                    try
                    {
                        OnEditorFinished?.Invoke(ObjectToEdit);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);   
                    }
                }

                OnEditorFinished = null;
                ObjectToEdit = null;
                
                if (editorView != null)
                {
                    editorView.parent.Remove(editorView);
                    editorView = null;
                }
            }
        }

        public ObjectEditorWrapper GetEditor()
        {
            return editor;
        }

        //public void Hide(DialogAneNode node)
        //{
        //    foreach (var toPort in node.AnswerToPorts)
        //    {
        //        
        //    }
        //}

        
    }
}