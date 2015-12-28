using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using FullSerializer;
using N;

namespace N.Package.Data
{
    /// Engine specifics for Json
    public class Json
    {
        private static readonly fsSerializer _serializer = new fsSerializer();

        /// Load a resource path as a json object
        public static Option<T> Resource<T>(string path)
        {
            return Json.Resource<T>(path, false);
        }

        /// Load a resource path as a json object
        public static Option<T> Resource<T>(string path, bool quiet)
        {
            var asset = Resources.Load(path) as TextAsset;
            if (asset == null)
            {
                if (!quiet)
                {
                    N.Console.Error("Invalid resource path: " + path);
                }
            }
            else
            {
                bool warnings;
                var rtn = Json.Deserialize<T>(asset.text, out warnings, quiet);
                if (rtn.IsSome && warnings)
                {
                    if (!quiet)
                    {
                        N.Console.Error("Warning: Some problems with {0}", path);
                    }
                }
                return rtn;
            }
            return Option.None<T>();
        }

        /// Serialize some object into json
        public static String Serialize<T>(T instance)
        {
            fsData data;
            _serializer.TrySerialize(typeof(T), instance, out data).AssertSuccessWithoutWarnings();
            return fsJsonPrinter.CompressedJson(data);
        }

        public static Option<T> Deserialize<T>(string serializedState, bool quiet = false)
        {
            bool warnings = false;
            return Json.Deserialize<T>(serializedState, out warnings, quiet);
        }

        public static Option<T> Deserialize<T>(string serializedState, out bool hadWarnings, bool quiet = false)
        {
            hadWarnings = false;
            try
            {
                fsData data = fsJsonParser.Parse(serializedState);
                object deserialized = null;
                var result = _serializer.TryDeserialize(data, typeof(T), ref deserialized);
                if (result.Succeeded)
                {

                    // If there were warnings, warn about them
                    if (result.HasWarnings)
                    {
                        hadWarnings = true;
                        if (!quiet)
                        {
                            foreach (var warning in result.RawMessages)
                            {
                                Console.Log(warning);
                            }
                        }
                    }

                    // If there is any mismatch between properties and fields, warn
                    var dump = data.AsDictionary;
                    var keys = dump.Keys;
                    var fields = N.Reflect.Type.Fields<T>();
                    foreach (var key in keys)
                    {
                        if (!fields.Contains(key))
                        {
                            Console.Error("Warning: Data contains unused key {0}:{1} on {2}", key, dump[key], N.Reflect.Type.Name<T>());
                            hadWarnings = true;
                        }
                    }
                    foreach (var key in fields)
                    {
                        if (!keys.Contains(key))
                        {
                            Console.Error("Warning: Data missing key {0} on {1}", key, N.Reflect.Type.Name<T>());
                            hadWarnings = true;
                        }
                    }

                    // Return the deserialized result anyhow
                    T rtn = (T)deserialized;
                    return Option.Some(rtn);
                }
            }
            catch (Exception err)
            {
                if (!quiet)
                {
                    Console.Error("Invalid json: {0}", serializedState);
                    Console.Error(err);
                }
            }
            return Option.None<T>();
        }
    }
}
