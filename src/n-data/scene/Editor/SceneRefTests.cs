using UnityEngine;
using N.Package.Data.Scene;
using N.Package.Data;
using N;
using NUnit.Framework;

public class SceneRefTests : N.Tests.Test
{
    public SceneRef fixture()
    {
        var obj = this.SpawnObjectWithComponent<SceneFabricated>();
        obj.GetComponent<SceneFabricated>().prefab = "package-data/test";
        return new SceneRef().Serialize(obj);
    }

    [Test]
    public void test_serialize()
    {
        var rp = fixture();
        Json.Serialize(rp);
        this.TearDown();
    }

    [Test]
    public void test_deserialize()
    {
        var rp = fixture();
        var output = Json.Serialize(rp);
        var instance = Json.Deserialize<SceneRef>(output).Unwrap();
        var obj = instance.Deserialize();
        Assert(obj.IsSome);
        Object.DestroyImmediate(obj.Unwrap());
        this.TearDown();
    }
}
