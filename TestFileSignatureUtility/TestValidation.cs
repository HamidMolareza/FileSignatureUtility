using FileSignatureUtility;
using Xunit;

namespace TestFileSignatureUtility
{
    public class TestValidation
    {
        private readonly FileSignatureService _fileSignatureService = new FileSignatureService ();
        private readonly Validation _fileValidation;

        public TestValidation()
        {
            _fileValidation = new Validation (_fileSignatureService);
        }

        [Fact]
        public void Test1()
        {

        }
    }
}
