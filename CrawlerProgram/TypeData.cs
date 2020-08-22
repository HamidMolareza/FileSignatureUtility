namespace CrawlerProgram {
    public class TypeData {
        public TypeData () { }

        public TypeData (string typeName, string signature, string description) {
            // Signature.Add(ParseToBytes (signature));
            Signature = signature.TrimEnd ();
            TypeName = typeName;
            Description = description;
        }

        // private static byte[] ParseToBytes(string input) =>
        //     input.Split(" ")
        //         .Where(data => !string.IsNullOrWhiteSpace(data))
        //         .Select(data => Convert.ToByte(data, 16))
        //         .ToArray();

        public string TypeName { get; set; }
        public string Signature { get; set; }
        public string Description { get; set; }
    }
}