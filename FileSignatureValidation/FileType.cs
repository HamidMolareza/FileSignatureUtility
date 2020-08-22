namespace FileSignatureValidation {
    public class FileType {
        public string TypeName { get; set; } = null!;
        public byte[] Signature { get; set; } = null!;
    }
}