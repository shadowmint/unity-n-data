using N.Package.Data;
using NUnit.Framework;

public class TestManifestBuilder : N.Tests.Test
{
    public void test_manifest_builder()
    {
        ManifestBuilder.Run("package-data");
    }
}
