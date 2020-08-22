using System.Collections.Generic;

namespace FileSignatureValidation
{
    public class FileType
    {
        public string TypeName { get; set; }
        public List<byte[]> Signatures { get; set; }
    }
}