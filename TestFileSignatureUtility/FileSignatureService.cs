using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using FileSignatureUtility;

namespace TestFileSignatureUtility
{
    public class FileSignatureService : IFileSignature
    {
        private readonly List<TypeDataModel> _typeData;

        public FileSignatureService()
        {
            var jsonData = File.ReadAllText(@"..\..\..\..\CrawlerProgram\Data\Data.json");
            _typeData = JsonSerializer.Deserialize<List<TypeDataModel>>(jsonData);
        }

        public Task<List<FileType>> GetTypesAsync(params string[] targetTypes) =>
            Task.FromResult(_typeData.Where(data => PredicateGetTypes(data, targetTypes))
                .Select(data => new FileType
                {
                    TypeName = data.TypeName,
                    Signature = data.Signature
                }).ToList());

        public Task<string?> FindSignature(string fileSignature)
        {
            var result = _typeData.Where(data => fileSignature.StartsWith(data.Signature))
                .Select(data => data.TypeName).SingleOrDefault(); //TODO: SingleOrDefault? Same?
            return Task.FromResult(result)!;
        }

        //TODO: ***
        // private static byte[] ParseToBytes(string input) =>
        //     input.Split(" ")
        //         .Select(data => Convert.ToByte(data, 16))
        //         .ToArray();

        private static bool PredicateGetTypes(TypeDataModel dataModel, IEnumerable<string> targetTypes) =>
            targetTypes.Any(targetType =>
                string.Equals(targetType, dataModel.TypeName, StringComparison.CurrentCultureIgnoreCase));

        //TODO: ***
        // private static string ConvertToString(IEnumerable<byte> bytes)
        // {
        //     var stringBuilder = new StringBuilder(60);
        //     foreach (var byteData in bytes)
        //     {
        //         stringBuilder.Append(byteData).Append(" ");
        //     }
        //
        //     stringBuilder.Remove(stringBuilder.Length - 1, 1);
        //
        //     return stringBuilder.ToString();
        // }
    }
}