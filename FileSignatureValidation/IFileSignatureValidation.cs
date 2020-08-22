using System.Collections.Generic;
using System.Threading.Tasks;

namespace FileSignatureValidation
{
    public interface IFileSignatureValidation
    {
        Task<List<FileType>> GetTypesAsync(params string[] targetTypes);
        Task<string?> FindSignature(IEnumerable<byte> fileSignature);
    }
}