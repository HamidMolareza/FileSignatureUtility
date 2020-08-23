using FileSignatureUtility;

namespace TestFileSignatureUtility
{
    public class TestDetection
    {
        private readonly FileSignatureService _fileSignatureService = new FileSignatureService ();
        private readonly Detection _fileDetection;

        public TestDetection()
        {
            _fileDetection = new Detection (_fileSignatureService);
        }
    }
}