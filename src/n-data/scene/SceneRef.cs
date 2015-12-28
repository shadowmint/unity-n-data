using UnityEngine;
using N.Package.Data;

namespace N.Package.Data.Scene
{
    /// The most basic representation of a prefab instance in a scene
    [System.Serializable]
    public class SceneRef
    {
        /// The active object from this ref, if one currently exists
        private Option<GameObject> instance;
        public Option<GameObject> gameObject { get { return instance; } }

        /// The path to the prefab
        public string prefab;

        /// Some arbitrary data for this object
        public string data;

        /// The transform for the prefab
        public SceneTransform transform;

        /// From game object -> scene object
        public SceneRef Serialize(GameObject target)
        {
            transform.Serialize(target);
            var info = target.GetRequiredComponent<SceneFabricated>();
            prefab = info.prefab;
            data = info.data;
            instance = Option.Some(target);
            return this;
        }

        /// From scene object -> game object
        public Option<GameObject> Deserialize()
        {
            var rtn = Option.None<GameObject>();
            N.Scene.Prefab(prefab).Then((fp) =>
            {
                N.Scene.Spawn(fp).Then((ip) =>
                {
                    instance = Option.Some(ip);
                    transform.Deserialize(ip);
                    ip.GetRequiredComponent<SceneFabricated>().Activate(data);
                    rtn = Option.Some(ip);
                });
            });
            return rtn;
        }

        /// Destroy the referenced game object, if any
        public void Destroy(bool immediate = false)
        {
            if (instance)
            {
                if (immediate)
                {
                    Object.DestroyImmediate(instance.Unwrap());
                }
                else
                {
                    Object.Destroy(instance.Unwrap());
                }
                instance = Option.None<GameObject>();
            }
        }
    }

    /// Simple transform type
    [System.Serializable]
    public struct SceneTransform
    {

        public Vector3 position;

        public Vector3 scale;

        public Quaternion rotation;

        /// From game object -> scene object
        public void Serialize(GameObject target)
        {
            position = target.transform.position;
            scale = target.transform.localScale;
            rotation = target.transform.rotation;
        }

        /// From scene object -> game object
        public void Deserialize(GameObject target)
        {
            target.transform.position = position;
            target.transform.localScale = scale;
            target.transform.rotation = rotation;
        }
    }
}
