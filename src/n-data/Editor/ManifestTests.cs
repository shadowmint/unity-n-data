#if N_DATA_TESTS
using N.Package.Data;
using NUnit.Framework;

public class ManifestTests : N.Tests.Test {

    [Test]
    public void test_load_manifest() {
      var manifest = Manifest.Load("package-data/tests/json");
      this.Assert(manifest != null);
      this.Assert(manifest.files.Length > 0);
      this.Assert(manifest.folders.Length == 0);
    }

    [Test]
    public void test_load_missing_manifest() {
      var manifest = Manifest.Load("blah/blah", true);
      this.Assert(manifest == null);
    }
}
#endif
