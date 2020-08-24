using System.Threading.Tasks;
using FileSignatureUtility;
using FunctionalUtility.Extensions;
using Xunit;

namespace TestFileSignatureUtility {
    public class TestDetection {
        private readonly FileSignatureService _fileSignatureService = new FileSignatureService ();
        private readonly Detection _fileDetection;
        private const string BasePath = @"TestFiles\";

        public TestDetection () {
            _fileDetection = new Detection (_fileSignatureService);
        }

        [Theory]
        [InlineData (BasePath + "exe", "exe")]
        [InlineData (BasePath + "image", "png")]
        [InlineData (BasePath + "pdf", "pdf")]
        [InlineData (BasePath + "xlsx", "xlsx")]
        public async Task DetectFileTypeAsync_FileTypeIsInTargetTypesRange_ReturnCorrectType (string fileName, string targetTypes) {
            var methodResult = await _fileDetection.DetectFileTypeAsync (fileName, targetTypes);

            Assert.True (methodResult.IsSuccess);
            Assert.NotNull (methodResult.Value);
            Assert.Equal (methodResult.Value!.ToLower (), targetTypes);
        }

        [Theory]
        [InlineData (BasePath + "exe", "png")]
        [InlineData (BasePath + "image", "exe")]
        [InlineData (BasePath + "pdf", "xlsx")]
        [InlineData (BasePath + "xlsx", "pdf")]
        public async Task DetectFileTypeAsync_FileTypeIsNotInTargetTypesRange_ReturnNull (string fileName, string targetTypes) {
            var methodResult = await _fileDetection.DetectFileTypeAsync (fileName, targetTypes);

            Assert.True (methodResult.IsSuccess);
            Assert.Null (methodResult.Value);
        }

        [Theory]
        [InlineData (BasePath + "exe", "EXE")]
        [InlineData (BasePath + "image", "PNG")]
        [InlineData (BasePath + "pdf", "PDF")]
        [InlineData (BasePath + "xlsx", "XLSX")]
        public async Task DetectFileTypeAsync_ValidFile_ReturnFileType (string fileName, string fileType) {
            var methodResult = await _fileDetection.DetectFileTypeAsync (fileName);

            Assert.True (methodResult.IsSuccess);
            Assert.NotNull (methodResult.Value);
            Assert.Contains (fileType, methodResult.Value);
        }

        [Fact]
        public async Task DetectFileTypeAsync_FileNameOrValidTypesAreEmpty_ReturnBadRequest () {
            var methodResult = await _fileDetection.DetectFileTypeAsync ("", "type");
            Assert.False (methodResult.IsSuccess);
            Assert.True (methodResult.IsBadRequestError ());

            var methodResult2 = await _fileDetection.DetectFileTypeAsync (BasePath + "image", "");
            Assert.False (methodResult2.IsSuccess);
            Assert.True (methodResult2.IsBadRequestError ());
        }

        [Fact]
        public async Task DetectFileTypeAsync_FileIsNotExist_ReturnNotFound () {
            var methodResult = await _fileDetection.DetectFileTypeAsync ("C:/Invalid_Path", "png");

            Assert.False (methodResult.IsSuccess);
            Assert.True (methodResult.IsNotFoundError ());
        }
    }
}