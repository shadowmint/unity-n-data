#if N_DATA_TESTS
using N.Package.Core.Tests;
using N.Package.Data;
using NUnit.Framework;

public class TestManifestBuilder : TestCase
{
  public void test_manifest_builder()
  {
    ManifestBuilder.Run("package-data");
  }
}
#endif