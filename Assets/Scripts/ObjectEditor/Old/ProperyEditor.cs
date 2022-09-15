using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using Assets.Scripts.Slime.Sugar;
using BlockEditor.Serialization;
using UnityEngine;
using UnityEngine.UIElements;

/*
namespace BlockEditor.Editors
{
    public partial class PropertyEditor : VisualElement
    {
        public PropertyEditor()
        {
            //InitializeComponent();
        }

        public List<object> toEditObjects = new List<object>();
        private MemberInfo memberInfo;
        public event Action<UserControl> OnChanged;
        public event Action OnShowFull;

        public void Copy(PropertyEditor pe)
        {
            this.OnShowFull = pe.OnShowFull;
        }
        
        
         //TODO
         public void Clear()
        {
            toEditObjects = null;
            memberInfo = null;
            OnChanged = null;

            GUI.RemoveFromParent(this);

            var arr = new List<object>();
            foreach (var child in Grid.Children)
            {
                arr.Add(child);
            }

            foreach (var a in arr)
            {
                GUI.Dealloc(a);
            }
        }

        public void Edit(Type t, List<object> toEdit, MemberInfo memberInfo = null, bool sort = true, string[] favoriteFields = null, bool? favoritesOnly = null) //var t = toEdit[0].GetType();
        {
            this.memberInfo = memberInfo;
            this.toEditObjects = new List<object>(toEdit);
            if (toEdit.Count == 0) return;
            
            var row = 0;
            var cc = ClassCache.Get(t);

            var all = new List<MemberInfo>();
            all.AddRange(cc.AttributeFields.Values.Select((x)=>x.Item1));
            all.AddRange(cc.Fields.Values);

            if (sort)
            {
                if (favoriteFields != null)
                {
                    var temp = new List<MemberInfo>();
                    foreach (var info in all)
                    {
                        if (favoriteFields.Contains(info.Name))
                        {
                            temp.Add(info);
                        }
                    }
                    foreach (var info in temp)
                    {
                        all.Remove(info);
                    }

                    if (favoritesOnly.HasValue && favoritesOnly.Value)
                    {
                        all.Clear();
                    }
                    else
                    {
                        all.Sort(Sugar.Sort);
                    }
                    
                    
                    temp.Sort((a,b)=> favoriteFields.Index(a.Name) - favoriteFields.Index(b.Name));
                    all.InsertRange(0, temp);
                }
                else
                {
                    all.Sort(Sugar.Sort);
                }
            }
            

            if (toEditObjects.Contains(null))
            {
                return;
            }
            
            GUI.InvokeGui(this, () =>
            {
                foreach (var f in all)
                {
                    bool should = false;
                    foreach (var o in toEditObjects)
                    {
                        should |= cc.ShouldSerialize(o, f);
                    }
                
                    if (!should) continue;
                
                    MakeGenerator(f, ref row);
                }

                if (favoritesOnly.HasValue)
                {
                    var btn = new Button();
                    btn.Content = favoritesOnly.Value ? "Show All" : "Show Favorites only";
                    btn.Click += (a, b) =>
                    {
                        OnShowFull();
                    };
                    Grid.Children.Add(btn);
                }
                
            
                foreach (var c in Grid.Children)
                {
                    var rd = new RowDefinition();
                    rd.Height = GridLength.Auto;//;
                    Grid.RowDefinitions.Add(rd);
                }
                GUI.Execute();
            });

            
            
        }
        
        

        void MakeGenerator(MemberInfo member, ref int row)
        {
            try
            {
                MakeGeneratorInternal(member, ref row);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        void MakeGeneratorInternal(MemberInfo member, ref int row) {
            try
            {
                if (!X.ShouldSerialize(member)) return;

                var tt = member.GetMemberType();
                if (tt.IsArray || tt.ImplementsGenericInterface(typeof(IList<>))) {
                    var ed = GUI.ArrayEditors.Allocate();
                    ed.Header.Text = member.Name;
                    Apply(ed, row);
                    Grid.Children.Add(ed);
                    row++;
                    
                    GUI.InvokeParallelSilent(() =>
                    {
                        var list = new List<object>();
                        foreach (var v in toEditObjects)
                        {
                            try
                            {
                                if (v == null)
                                {
                                    list.Add(null);
                                    continue;
                                }

                                var dd = member.GetValue(v);
                                list.Add(dd);
                            }
                            catch (Exception e)
                            {
                                throw new Exception("WTF?");
                            }
                        }

                        ed.SetEditorObjects(list, member, toEditObjects);
                    });
                } else if (tt == typeof(bool))
                {
                    var rd = GUI.RadioEditors.Allocate();
                    Apply(rd, row);
                    rd.Header.Text = member.Name;
                    rd.OnChanged += TriggerChanged;
                    Grid.Children.Add(rd);
                    row++;

                    GUI.InvokeParallelSilent(() =>
                    {
                        rd.SetEditorObjects(member, toEditObjects);
                        rd.RefreshRadio();
                    });
                } else if (tt == typeof(SerializableVector3) || tt == typeof(SerializableVector3D) || tt == typeof(SerializableVector3I) || 
                           tt == typeof(SerializableVector3?) || tt == typeof(SerializableVector3D?) || tt == typeof(SerializableVector3I?)) {
                    var ve = GUI.VectorEditors.Allocate(); 
                    ve.Header.Text = member.Name;
                    Apply(ve, row);
                    ve.OnChanged += TriggerChanged; 
                    Grid.Children.Add(ve);
                    row++; 
                    GUI.InvokeParallelSilent(() =>
                    {
                        ve.SetEditorObjects(member, toEditObjects);
                        ve.RefreshVector();
                    });
                } else if (tt.IsEnum) {
                    var en = GUI.EnumEditors.Allocate();
                    Apply(en, row);
                    en.Header.Text = member.Name;
                    en.OnChanged += TriggerChanged;
                    Grid.Children.Add(en);
                    row++;
                    
                    GUI.InvokeParallelSilent(() => en.SetEditorObjects(member, toEditObjects));
                } else if (X.GetSerializer(tt) != null && X.GetDeserializer(tt) != null) {
                    var te = GUI.TextEditors.Allocate();
                    Apply(te, row);
                    te.Header.Text = member.Name;
                    te.OnChanged += TriggerChanged;
                    Grid.Children.Add(te);
                    row++;

                    GUI.InvokeParallelSilent(() => {
                        var list = new List<object>();
                        foreach (var v in toEditObjects)
                        {
                            list.Add(member.GetValue(v));
                        }
                        te.SetEditorObjects(list, tt, member, toEditObjects);
                    });
                } else {
                    try
                    {
                        var txt = new TextBlock();
                        txt.SetValue(Grid.RowProperty, row);
                        txt.SetValue(Grid.ColumnProperty, 0);
                        txt.Text = member.Name;
                        Grid.Children.Add(txt);
                        row++;
                        
                        var innerEditor = new PropertyEditor();
                        
                        innerEditor.OnChanged += TriggerChanged;
                        innerEditor.Margin = new Thickness(30, 5, 0, 5);
                        innerEditor.SetValue(Grid.RowProperty, row);
                        innerEditor.SetValue(Grid.ColumnProperty, 0);
                        Grid.Children.Add(innerEditor);
                        row++;
                        
                        GUI.InvokeParallelSilent(() =>
                        {
                                
                            var inner = new List<object>();
                            foreach (var o in toEditObjects)
                            {
                                if (o == null)
                                {
                                    inner.Add(null);
                                }
                                else
                                {
                                    var oo = member.GetValue(o);
                                    inner.Add(oo);
                                }
                            }
                            innerEditor.Edit(tt, inner, member);
                        });
                    }
                    catch (Exception e)
                    {
                        Output.Verbose(e);
                    }
                }
            }
            catch (Exception e)
            {
                Output.Verbose(e);
            }
        }

        private void TriggerChanged(VisualElement control)
        {
            if (control is PropertyEditor pe)
            {
                try
                {
                    for (var x = 0; x < toEditObjects.Count; x++)
                    {
                        var parent = toEditObjects[x];
                        var value = pe.toEditObjects[x];
                        var mi = pe.memberInfo;

                        mi.SetValue(parent, value);
                    }
                    //pe.Background = Brushes.Transparent;
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                    //pe.Background = Brushes.Maroon;
                }
            }
 
            
            OnChanged?.Invoke(this);
        }


       
    }
}*/