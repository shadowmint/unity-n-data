using UnityEngine;

namespace N.Package.Data.Scene
{
    /// Place this marker on scene prefabs so they can be serialized.
    [DisallowMultipleComponent]
    [AddComponentMenu("N/Data/SceneFabricated")]
    public class SceneFabricated : MonoBehaviour
    {

        /// The path to the prefab for this object
        public string prefab;

        /// Arbitrary data for this instance
        public string data;

        /// Are we ready yet?
        private bool active = false;

        /// Internal ready
        private SceneFabReadyDelegate ready = null;

        /// Active this object
        public void Activate(string data)
        {
            this.data = data;
            this.active = true;
            if (ready != null)
            {
                ready(data);
            }
        }

        /// Invoke on ready
        public void Ready(SceneFabReadyDelegate ready)
        {
            if (active)
            {
                ready(data);
            }
            else
            {
                this.ready = ready;
            }
        }
    }

    /// Helper
    public delegate void SceneFabReadyDelegate(string data);
}
