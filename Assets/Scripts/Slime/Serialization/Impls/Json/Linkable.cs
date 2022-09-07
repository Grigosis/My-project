using System.Collections.Generic;

namespace ROR.Core.Serialization.Json
{
    public interface Linkable
    {
        /// <summary>
        /// Maybe null, but always non null before StoreLinks called
        /// </summary>
        public string GUID { get; set; }
        public void GetLinks(HashSet<Linkable> links);
        public void RestoreLinks(ReferenceSerializer dictionary);
        public void StoreLinks();
    }
}