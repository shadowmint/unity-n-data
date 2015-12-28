using UnityEngine;
using System.Collections.Generic;

namespace N.Package.Data.Scene
{
    /// A simple representation of all objects in a scene
    public class SceneData
    {
        /// The collection of reference on this scene
        public SceneRef[] refs;

        /// Serialize the current scene
        public SceneData Serialize()
        {
            var refs = new List<SceneRef>();
            foreach (var fab in N.Scene.FindComponents<SceneFabricated>())
            {
                refs.Add(new SceneRef().Serialize(fab.gameObject));
            }
            this.refs = refs.ToArray();
            return this;
        }

        /// Deserialize into the current scene
        public void Deserialize()
        {
            if (refs != null)
            {
                foreach (var rp in refs)
                {
                    rp.Deserialize().Then((ip) => { }, () =>
                    {
                        _.Log("Failed to deserialize object: {0}", rp.prefab);
                    });
                }
            }
        }

        /// Destroy all objects that are linked to this scene
        /// @param immediate Immediate mode, for tests
        public SceneData Destroy(bool immediate = false)
        {
            foreach (var rp in refs)
            {
                rp.Destroy(immediate);
            }
            return this;
        }
    }
}
