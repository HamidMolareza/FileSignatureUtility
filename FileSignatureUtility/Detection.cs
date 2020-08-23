using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionalUtility.Extensions;
using FunctionalUtility.ResultDetails.Errors;
using FunctionalUtility.ResultUtility;

namespace FileSignatureUtility {
    public class Detection {
        private readonly IFileSignature _fileSignatureValidation;
        private const int MaximumReadBytes = 50;

        public Detection (IFileSignature fileSignatureValidation) {
            _fileSignatureValidation = fileSignatureValidation;
        }

        public Task<MethodResult<string?>> DetectFileTypeAsync (Stream fileStream, params string[] types) =>
            FailExtensions.FailWhen (types.IsNullOrEmpty (),
                new BadRequestError (message: $"{nameof(types)} is null or empty."))
            .OnSuccessAsync (() => _fileSignatureValidation.GetTypesAsync (types))
            .OnSuccessFailWhenAsync (targetTypes => targetTypes.IsNullOrEmpty (),
                new BadRequestError (message: "Type is not correct or not supported."))
            .OnSuccessAsync (targetTypes => GetMaximumSignaturesLength (targetTypes)
                .TryMapAsync (maxRead => ReadBytesAsync (fileStream, maxRead))
                .OnSuccessAsync (fileSignature => DetectFileType (ConvertBytesToHexString (fileSignature), targetTypes))
            );

        public Task<MethodResult<string?>> DetectFileTypeAsync (string fileName, params string[] types) =>
            ValidateInputs (fileName, types)
            .OnSuccessAsync (() => _fileSignatureValidation.GetTypesAsync (types))
            .OnSuccessFailWhenAsync (targetTypes => targetTypes.IsNullOrEmpty (),
                new BadRequestError (message: "Type is not correct or not supported."))
            .OnSuccessAsync (targetTypes => GetMaximumSignaturesLength (targetTypes)
                .TryMapAsync (maxRead => ReadBytesAsync (fileName, maxRead))
                .OnSuccessAsync (fileSignature => DetectFileType (ConvertBytesToHexString (fileSignature), targetTypes))
            );

        public Task<MethodResult<string?>> DetectFileTypeAsync (Stream fileStream,
                int maximumReadBytes = MaximumReadBytes) =>
            TryExtensions.TryAsync (() => ReadBytesAsync (fileStream, maximumReadBytes))
            .OnSuccessAsync (bytes => _fileSignatureValidation.FindSignature (ConvertBytesToHexString (bytes)));

        public Task<MethodResult<string?>> DetectFileTypeAsync (
                string fileName, int maximumReadBytes = MaximumReadBytes) =>
            TryExtensions.TryAsync (() => ReadBytesAsync (fileName, maximumReadBytes))
            .OnSuccessAsync (bytes => _fileSignatureValidation.FindSignature (ConvertBytesToHexString (bytes)));

        public static string? DetectFileType (string fileSignature, IEnumerable<FileType> types) {
            foreach (var type in types) {
                if (fileSignature.StartsWith (type.Signature))
                    return type.TypeName;
            }

            return null;
        }

        internal static string ConvertBytesToHexString (IReadOnlyCollection<byte> bytes) {
            var stringBuilder = new StringBuilder (bytes.Count * 4);
            foreach (var byteData in bytes) {
                stringBuilder.Append (Convert.ToString (byteData, 16)
                        .ToUpper ()
                        .PadLeft (2, '0'))
                    .Append (" ");
            }

            return stringBuilder.ToString ().TrimEnd ();
        }

        private static MethodResult ValidateInputs (string fileName, params string[] validTypes) {
            if (string.IsNullOrEmpty (fileName))
                return MethodResult.Fail (new BadRequestError (message: $"{nameof(fileName)} is null or empty."));
            if (!File.Exists (fileName))
                return MethodResult.Fail (new NotFoundError (message: $"{nameof(fileName)} is not exist."));
            if (validTypes.IsNullOrEmpty ())
                return MethodResult.Fail (new BadRequestError (message: $"{nameof(validTypes)} is null or empty."));
            return MethodResult.Ok ();
        }

        private static int GetMaximumSignaturesLength (IEnumerable<FileType> fileTypes) =>
            fileTypes.Aggregate ((f1, f2) =>
                f1.Signature.Length > f2.Signature.Length ? f1 : f2).Signature.Length;

        private static async Task<byte[]> ReadBytesAsync (string fileName, int maxReadCount) {
            await using var reader = File.OpenRead (fileName);
            var bytes = new byte[maxReadCount];
            await reader.ReadAsync (bytes, 0, maxReadCount);
            return bytes;
        }

        private static async Task<byte[]> ReadBytesAsync (Stream fileStream, int maxReadCount) {
            var bytes = new byte[maxReadCount];
            await fileStream.ReadAsync (bytes, 0, maxReadCount);
            return bytes;
        }
    }
}