using Assets.Scripts.AbstractNodeEditor;
using DS.Windows;
using SecondCycleGame.Assets.Scripts.AbstractNodeEditor;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace SecondCycleGame.Assets.Scripts.ANEImpl.Views
{
    public abstract class RowView<PDATA, DATA> : VisualElement
    {
        protected DATA Data;
        protected PDATA ParentData;
        protected IRowListener<DATA> Listener;
        protected ANEGraph GraphView;
        public Label Text;
        public ExtendedPort EPort { get; protected set; }
        
        public RowView(string path) : base()
        {
            VisualTreeAsset uiAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            TemplateContainer ui = uiAsset.CloneTree();
            Add(ui);
        }
        
        public virtual void Init(ANEGraph graphView, PDATA pdata, DATA data, IRowListener<DATA> listener)
        {
            Data = data;
            ParentData = pdata;
            Listener = listener;
            GraphView = graphView;
            
            var container = this.Q<VisualElement>("port-container");
            
            var deleteBtn = this.Q<Button>("delete-btn");
            var upBtn = this.Q<Button>("move-up");
            var downBtn = this.Q<Button>("move-down");
            var editBtn = this.Q<Button>("edit-btn");
            
            Text = this.Q<Label>("row-text"); 
            
            if (deleteBtn != null) deleteBtn.clickable.clicked += OnClickedDelete;
            if (editBtn != null) editBtn.clickable.clicked += OnClickedEdit;
            if (upBtn != null) upBtn.clickable.clicked += ()=>listener.OnMoveRequest(this, this.Data, -1);
            if (downBtn != null) downBtn.clickable.clicked += ()=>listener.OnMoveRequest(this, this.Data, 1);

            EPort = CreatePort(container);
            container.Add(EPort);
            BindPortData();
        }

        public virtual void SetData(DATA newData)
        {
            var oldData = Data;
            Data = newData;
            BindPortData();
            Listener.OnSubNodeValueChanged(this, oldData, newData);
            UpdateUI();
        }

        protected abstract void BindPortData();

        protected abstract ExtendedPort CreatePort(VisualElement element);

        public virtual void UpdateUI()
        {
        }
        
        public virtual void OnEditorFinished(DATA editedObject)
        {
        }
        
        private void OnClickedDelete()
        {
            Listener.OnSubNodeDelete(this, Data);
        }
        
        private void OnClickedEdit()
        {
            Listener.OnEditRequest(this, Data);
        }
    }
}