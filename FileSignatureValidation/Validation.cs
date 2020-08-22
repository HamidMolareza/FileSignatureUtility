using System.IO;
using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultUtility;

namespace FileSignatureValidation
{
    public class Validation
    {
        private readonly IFileSignatureValidation _fileSignatureValidation;

        public Validation(IFileSignatureValidation fileSignatureValidation)
        {
            _fileSignatureValidation = fileSignatureValidation;
        }


        public Task<MethodResult<bool>> ValidateAsync(Stream fileStream, params string[] validTypes) =>
            new Detection(_fileSignatureValidation).DetectFileTypeAsync(fileStream, validTypes)
                .OnSuccessAsync(fileType => !string.IsNullOrEmpty(fileType));

        public Task<MethodResult<bool>> ValidateAsync(string fileName, params string[] validTypes) =>
            new Detection(_fileSignatureValidation).DetectFileTypeAsync(fileName, validTypes)
                .OnSuccessAsync(fileType => !string.IsNullOrEmpty(fileType));
    }
}