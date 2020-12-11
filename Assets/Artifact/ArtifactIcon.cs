using UnityEngine;

namespace Assets.Artifacts
{
    public class ArtifactIcon : MonoBehaviour
    {
        public GameObject icon;

        public void setIconMaterial(Material material)
        {
            icon.GetComponent<MeshRenderer>().material = material;
        }
    }
}
