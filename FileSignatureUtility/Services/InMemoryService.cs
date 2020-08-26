using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FileSignatureUtility.Services {
    public class InMemoryService : IFileSignature {
        private readonly List<TypeDataModel> _typeData;

        public InMemoryService (string dataFileName) {
            var jsonData = File.ReadAllText (dataFileName);
            _typeData = JsonSerializer.Deserialize<List<TypeDataModel>> (jsonData);
        }

        public Task<List<FileType>> GetTypesAsync (params string[] targetTypes) =>
            Task.FromResult (_typeData.Where (data => PredicateGetTypes (data, targetTypes))
                .Select (data => new FileType {
                    TypeName = data.TypeName,
                        Signature = data.Signature
                }).ToList ());

        public Task<List<string>> FindSignature (string fileSignature) {
            var matchTypes = _typeData.Where (data => fileSignature.StartsWith (data.Signature))
                .Select (data => data.TypeName).ToList ();
            return Task.FromResult (matchTypes);
        }

        private static bool PredicateGetTypes (TypeDataModel dataModel, IEnumerable<string> targetTypes) =>
            targetTypes.Any (targetType =>
                string.Equals (targetType, dataModel.TypeName, StringComparison.CurrentCultureIgnoreCase));
    }
}