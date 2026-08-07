using UnityEngine;

namespace AccessibilityIndicators.Helpers
{
    using UnityEngine.Serialization;

    internal class ObjectIDInfo: MonoBehaviour
    {
        [FormerlySerializedAs("_OwnerID")] public string ownerID = "none";
    }
}
