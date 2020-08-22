using System;
using System.Collections.Generic;
using System.Linq;

namespace CrawlerProgram
{
    public class TypeData {
        public TypeData (string typeName, string signature, string description) {
            Signatures.Add(ParseToBytes (signature));
            BaseConstructor(typeName, description);
        }
        
        public TypeData(string typeName, string description)
        {
            BaseConstructor(typeName, description);
        }

        private void BaseConstructor(string typeName, string description)
        {
            TypeName = typeName;
            Description = description;
        }
        
        private static byte[] ParseToBytes(string input) =>
            input.Split(" ")
                .Where(data => !string.IsNullOrWhiteSpace(data))
                .Select(data => Convert.ToByte(data, 16))
                .ToArray();

        public string TypeName { get; set; } = null!;
        public List<byte[]> Signatures { get; set; } = new List<byte[]>();
        public string Description { get; set; } = null!;
    }
}