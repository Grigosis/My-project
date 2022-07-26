using System.Collections.Generic;
using System.Xml.Serialization;

namespace ROR.Core.Serialization
{
    public class SerializableList<V> : List<V>, IXmlSerializable
    {
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema() {
            return null;
        }
        
        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            this.Clear();
            var serializer = D.GetSerializer(typeof(V));
            
            if (reader.IsEmptyElement)
            {
                reader.Read();
                return;
            }
            
            reader.Read();
            
            while (reader.IsStartElement())
            {
                var d = (V)serializer.Deserialize(reader);
                Add(d);
            }
            
            reader.ReadEndElement();
        }
        
        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            XmlSerializer serializer = D.GetSerializer(typeof(V));
            foreach (var entry in this) serializer.Serialize(writer, entry);
        }
    }

    public class SerializableDictionary<T, V> : Dictionary<T, V>, IXmlSerializable
    {
        [XmlType("Item")]
        public struct Entry {
            public Entry(T key, V value) : this() { Key = key; Value = value; }
            [XmlAttribute("Key")]
            public T Key { get; set; }
            [XmlText]
            public V Value { get; set; }
        }
        
        System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema() {
            return null;
        }
        void IXmlSerializable.ReadXml(System.Xml.XmlReader reader) {
            this.Clear();
            var serializer = D.GetSerializer(typeof(Entry));
            
            if (reader.IsEmptyElement)
            {
                reader.Read();
                return;
            }
            
            reader.Read();
            
            while (reader.IsStartElement())
            {
                var d = (Entry)serializer.Deserialize(reader);
                Add(d.Key, d.Value);
            }
            
            reader.ReadEndElement();
        }
        
        
        void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer) {
            XmlSerializer serializer = D.GetSerializer(typeof(Entry));
            foreach (var entry in this)
            {
                serializer.Serialize(writer, new Entry () { Key = entry.Key, Value = entry.Value });
            }
        }
    }
}