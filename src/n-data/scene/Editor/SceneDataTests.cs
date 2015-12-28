#if N_DATA_TESTS
using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using N;
using N.Package.Data;
using N.Package.Data.Scene;

public class SceneDataTests : N.Tests.Test
{

    // Spawn a test object
    private void spawn()
    {
        var instance = this.SpawnComponent<SceneFabricated>();
        instance.prefab = "package-data/test";
    }

    [Test]
    public void test_serialize_deserialize()
    {
        spawn();
        spawn();
        spawn();
        spawn();
        spawn();
        var instance = new SceneData().Serialize();
        var serialized = Json.Serialize(instance);

        var thawed = Json.Deserialize<SceneData>(serialized).Unwrap();
        thawed.Deserialize();
        thawed.Destroy();

        this.TearDown();
    }
}
#endif
