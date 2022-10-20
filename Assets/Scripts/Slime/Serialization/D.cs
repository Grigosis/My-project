using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Assets.Scripts.Slime.Core.Algorythms;
using Assets.Scripts.Sugar;
using ROR.Lib;
using UnityEngine;

namespace ROR.Core.Serialization
{
    public class D
    {
        private static D _instance;
        private readonly Dictionary<string, Definition> Definitions = new Dictionary<string, Definition>();
        private readonly Dictionary<Type, List<Definition>> DefinitionsByType = new Dictionary<Type, List<Definition>>();
        
        private static string ExtraPath = "Assets/XmlData/";//"Assets/Resources/Data/"
        private string AbsolutePathToResources;
        
        public static D Instance
        {
            get
            {
                lock (typeof(D))
                {
                    if (_instance == null)
                    {
                        _instance = new D();
                        _instance.Init();
                    } 
                    return _instance;
                }
            }
        }

        
        public void Init()
        {
            AbsolutePathToResources = Path.GetFullPath(".")+"/";
            //D:\Unity\ssh\Temp\Bin\Debug\Assets\Resources\Data
            
           
            WriteToFile("test.tst", GetExportXML());
            
            using (new Measure("Known types"))
            {
                var kt = KnownTypes;
            }
            using (new Measure("Load all"))
            {
                LoadAll("");
            }
        }
        
        static Definitions GetExportXML()
        {
            var d = new Definitions();
            d.DefinitionList = new SerializableList<Definition>();

            var behavior = new AIBehaviorDefinition();
            behavior.Behavior = new AIBehaviorActionXml[1];

            var behaviorAction = new AIBehaviorUseSkillXml();
            behaviorAction.SkillType = "Attack";
            behaviorAction.TargetFilter = "Ally";
            behaviorAction.TargetSelectorFx = "Closest";
            behaviorAction.MaxMoveAPToCast = 2;

            behavior.Behavior[0] = behaviorAction;

            behavior.Triggers = new AIBehaviorTriggerXml[1];

            var trigger = new AIBehaviorTriggerUnitDiedXml();
            trigger.TriggerFx = "Say";
            trigger.Params = new FxParamXml[1];
            trigger.Params[0] = new FxParamXml()
            {
                Name = "Text",
                Value = "You bastard!"
            };
            
            trigger.Ally = true;

            behavior.Triggers[0] = trigger;

            behavior.Positioning = new AIPositioningXml();

            behavior.Positioning.Layers = new AIPositioningLayerXml[1];
            behavior.Positioning.Layers[0] = new AIPositioningLayerXml();
            behavior.Positioning.Layers[0].MinValue = 1;
            behavior.Positioning.Layers[0].MaxValue = 3;
            behavior.Positioning.Layers[0].ValueFx = "StayAwayFromEnemies";
            behavior.Positioning.Layers[0].Params = new FxParamXml[2];
            behavior.Positioning.Layers[0].Params[0] = new FxParamXml()
            {
                Name = "MinRange",
                Value = "3",
            };
            behavior.Positioning.Layers[0].Params[1] = new FxParamXml()
            {
                Name = "MaxRange",
                Value = "8",
            };
            
            
            d.DefinitionList.Add(behavior);
            //var effect = new EffectDefinition();
            //effect.Class = new SClass<EffectEntity>();
            //effect.Class.Type = typeof(HealingEffectEntity);
            //d.DefinitionList.Add(effect);
            
            /*d.DefinitionList.Add(new LivingEntityDefinition()
            {
                EquippedItems = new string[] { "Item/WTF"}, 
                Id = "LivingEntity/Hero", 
                Skills = new []{ "Skill/SimpleAttack"},
            });
            
            
            d.DefinitionList.Add(new SkillDefinition()
            {
                Id = "Skill/SimpleAttack",
                Attacks = new ElementAttack[]
                {
                    new ElementAttack("CRUSH", 0.5f),
                    new ElementAttack("FIRE", 0.5f)
                }
            });
            
            
            d.DefinitionList.Add(new EquipmentDefinition()
            {
                Id = "Item/BaseWeapon"
            });*/
            
            return d;
        }
        
       
        
        private static Type[] _knownTypes;
        public static Type[] KnownTypes
        {
            get
            {
                if (_knownTypes == null)
                {
                    var types =  new List<Type>();
                    foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
                    {
                        if (typeof(Definition).IsAssignableFrom(t) || t.Name.EndsWith("Xml"))
                        {
                            types.Add(t);
                        }
                    }
                        
                            
                    _knownTypes = types.ToArray();
                }

                return _knownTypes;
            }
        }

        public void Register(Definition d)
        {
            Debug.Log("Register:" + d.Id);
            Definitions.Add(d.Id, d);
            DefinitionsByType.GetOrCreate(D.Instance.GetType()).Add(d);
        }

        public T Get<T>(string Id) where T : Definition
        {
            if(Definitions.TryGetValue(Id, out var def))
            {
                return def as T;
            }

            throw new Exception($"Definition with id [{Id}] not found");
        }
        
        public void GetAll<T>(ICollection<T> where) where T : Definition
        {
            if (DefinitionsByType.TryGetValue(typeof(T), out var defs))
            {
                foreach (var t in defs)
                {
                    where.Add((T)t);
                }
            }
        }

       
        
        
       

        private void LoadAll(string folder)
        {
            Debug.Log("InitDefinitions:" + AbsolutePathToResources+ExtraPath+folder);
            var files = Directory.GetFiles(AbsolutePathToResources+ExtraPath+folder, "*.xml", SearchOption.AllDirectories);
            var list = new List<Definition>();
            
            Parallel.ForEach(files, (file) =>
            {
                Definitions defs;
                try
                {
                    defs = ReadFromFile<Definitions>(file);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Debug.LogError("Error in file: " + file + " : " + e);
                    return;
                }

                lock (list)
                {
                    list.AddRange(defs.DefinitionList);
                }
            });

            foreach (var d in list)
            {
                Register(d);
            }
            
        }


        public void WriteToFile(string fname, Definition definition)
        {
            WriteToFile(fname, new Definitions()  { DefinitionList = new SerializableList<Definition>() { definition }});
        }
        
        public void WriteToFile(string filePath, object objectToWrite, bool append = false) 
        {
            TextWriter writer = null;
            try
            {
                var serializer = GetSerializer(objectToWrite.GetType());
                writer = new StreamWriter(AbsolutePathToResources+ExtraPath+filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        private T ReadFrom<T>(string filePath)
        {
            return ReadFromFile<T>(AbsolutePathToResources+ExtraPath+filePath);
        }
        
        private T ReadFromFile<T>(string filePath) 
        {
            Debug.Log("ReadFrom:" + filePath);
            TextReader reader = null;
            try
            {
                var serializer = GetSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T) serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());//Debug.Log("Error at:" + filePath + " " + e);
                throw e;
            }
            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }


        [ThreadStatic] private static Dictionary<Type, XmlSerializer> Serializers;
        
        public static XmlSerializer GetSerializer(Type t)
        {
            if (Serializers == null)
            {
                Serializers = new Dictionary<Type, XmlSerializer>();
            }
            
            if (!Serializers.ContainsKey(t))
            {
                XmlAttributeOverrides xOver = new XmlAttributeOverrides();
                AddAsAttributeOverLoad<Vector2>(xOver,  "x");
                AddAsAttributeOverLoad<Vector2>(xOver,  "y");
                AddAsAttributeOverLoad<Vector3>(xOver, "x");
                AddAsAttributeOverLoad<Vector3>(xOver, "y");
                AddAsAttributeOverLoad<Vector3>(xOver, "z");
                var ser = new XmlSerializer(t, xOver, KnownTypes, null, null);
                Serializers.Add(t, ser);
            }
            return Serializers[t];
        }
        
        private static void AddAsAttributeOverLoad<T>(XmlAttributeOverrides over, string fieldName, string attrName = null)
        {
            attrName ??= fieldName;
            XmlAttributes xAttrs = new XmlAttributes();
            XmlAttributeAttribute xAttribute = new XmlAttributeAttribute(attrName);
            xAttrs.XmlAttribute = xAttribute;
  
            over.Add(typeof(T), fieldName, xAttrs);
        }
    } 
}