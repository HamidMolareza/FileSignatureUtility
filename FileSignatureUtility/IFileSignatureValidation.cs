using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileSignatureUtility {
    public interface IFileSignature {
        Task<List<FileType>> GetTypesAsync (params string[] targetTypes);
        Task<string?> FindSignature (string fileSignature);
    }
}