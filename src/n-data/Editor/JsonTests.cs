using UnityEngine;
using N.Package.Data;
using NUnit.Framework;

public class JsonTests : N.Tests.Test
{
    public class Bar
    {
        public float z;
        public string value;
    }

    public class Foo
    {
        public Foo() { WHATEVER = ""; }
        public Foo(string what) { WHATEVER = what; }
        public bool Is(string what) { return WHATEVER == what; }
        private string WHATEVER;
        public int x;
        public int y;
        public string name;
        public Vector3 vec;
        public Bar[] bars;
    }

    [Test]
    public void test_can_parse_typed_json()
    {
        var json = @"{
          ""x"": 1,
          ""y"": 2,
          ""name"": ""foo"",
          ""vec"": {""x"":0.0,""y"":0.0,""z"":0.0},
          ""bars"": [
            { ""z"": 1.0, ""value"": ""one"" },
            { ""z"": 2.0, ""value"": ""two"" }
          ]
      }";

        Json.Deserialize<Foo>(json).Then((Foo foo) =>
        {
            this.Assert(foo.x == 1);
            this.Assert(foo.y == 2);
            this.Assert(foo.name == "foo");
            this.Assert(foo.bars.Length == 2);
            this.Assert(foo.bars[0].z == 1.0);
            this.Assert(foo.bars[0].value == "one");
            this.Assert(foo.bars[1].z == 2.0);
            this.Assert(foo.bars[1].value == "two");
        }, () =>
        {
            Unreachable();
        });
    }

    [Test]
    public void test_can_parse_invalid_json()
    {
        var json = @"{
          ""x"": 1,
          ""y"": 2,
          ""name"": ""foo"",
          ""bars"": [
            { ""z"": 1.0, ""value"": ""one"" },
            { ""z"": 2.0, ""value"": ""two"" }
          ], <--- Notice this is invalid
      }";

        var foo = Json.Deserialize<Foo>(json, true);
        this.Assert(foo.IsNone);
    }

    [Test]
    public void test_can_parse_resource()
    {
        Json.Resource<Foo>("package-data/tests/json/sample").Then((foo) =>
        {
            this.Assert(foo.x == 1);
            this.Assert(foo.y == 2);
            this.Assert(foo.name == "foo");
            this.Assert(foo.bars.Length == 2);
            this.Assert(foo.bars[0].z == 1.0);
            this.Assert(foo.bars[0].value == "one");
            this.Assert(foo.bars[1].z == 2.0);
            this.Assert(foo.bars[1].value == "two");
        }, () =>
        {
            Unreachable();
        });
    }

    [Test]
    public void test_save_load()
    {
        var foo = new Foo("Hello World");
        Assert(foo.Is("Hello World"));

        foo.x = 100;
        foo.y = 200;
        var output = Json.Serialize(foo);

        var input = Json.Deserialize<Foo>(output);
        if (input)
        {
            Assert(input.Unwrap().x == 100);
            Assert(input.Unwrap().y == 200);
            Assert(input.Unwrap().Is("")); // Notice private variable is ignored.
        }
    }

    [Test]
    public void test_serialize_vector()
    {
        var vec = new Vector3(1f, 2f, 3f);
        var dump = Json.Serialize(vec);
        var vec_ = Json.Deserialize<Vector3>(dump);
        if (vec_)
        {
            var vec2 = vec_.Unwrap();
            Assert(vec2[0] == 1f);
            Assert(vec2[1] == 2f);
            Assert(vec2[2] == 3f);
        }
        else
        {
            Unreachable();
        }
    }
}
