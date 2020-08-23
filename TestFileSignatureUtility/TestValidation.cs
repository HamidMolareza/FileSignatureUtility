using System.Threading.Tasks;
using FileSignatureUtility;
using Xunit;

namespace TestFileSignatureUtility {
    public class TestValidation {
        private readonly FileSignatureService _fileSignatureService = new FileSignatureService ();
        private readonly Validation _fileValidation;
        private const string BasePath = @"TestFiles\";

        public TestValidation () {
            _fileValidation = new Validation (_fileSignatureService);
        }

        [Theory]
        [InlineData (BasePath + "exe", "exe")]
        [InlineData (BasePath + "image", "png")]
        [InlineData (BasePath + "pdf", "pdf")]
        [InlineData (BasePath + "xlsx", "xlsx")]
        public async Task ValidateAsync_FileHasValidType_ReturnTrue (string fileName, string validType) {
            var methodResult = await _fileValidation.ValidateAsync (fileName, validType);

            Assert.True (methodResult.IsSuccess);
            Assert.True (methodResult.Value);
        }

        [Theory]
        [InlineData (BasePath + "exe", "png")]
        [InlineData (BasePath + "image", "exe")]
        [InlineData (BasePath + "pdf", "xlsx")]
        [InlineData (BasePath + "xlsx", "pdf")]
        public async Task ValidateAsync_FileHasNotValidType_ReturnFalse (string fileName, string validType) {
            var methodResult = await _fileValidation.ValidateAsync (fileName, validType);

            Assert.True (methodResult.IsSuccess);
            Assert.False (methodResult.Value);
        }
    }
}