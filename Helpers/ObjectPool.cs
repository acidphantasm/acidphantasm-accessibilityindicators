using System.Collections.Generic;
using UnityEngine;
using AccessibilityIndicators.IndicatorUI;
using AccessibilityIndicators.Scripts;

namespace AccessibilityIndicators.Helpers
{

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool SharedInstance;
        private static List<GameObject> stepIndicators;
        private static List<GameObject> shotIndicators;
        private static List<GameObject> voiceIndicators;
        private static List<GameObject> verticalityIndicators;

        void Awake()
        {
            SharedInstance = this;
        }
        public static void PoolStepIndicators(GameObject objectToPool, GameObject parentObject, int amountToPool)
        {
            stepIndicators = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, parentObject.transform);
                tmp.AddComponent<ObjectIDInfo>();
                tmp.SetActive(false);
                stepIndicators.Add(tmp);
            }
        }
        public static void PoolVoiceIndicators(GameObject objectToPool, GameObject parentObject, int amountToPool)
        {
            voiceIndicators = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, parentObject.transform);
                GameObject armPivot = tmp.transform.GetChild(0).gameObject;
                tmp.AddComponent<ObjectIDInfo>();
                armPivot.AddComponent<KeepVerticalRotation>();
                tmp.SetActive(false);
                voiceIndicators.Add(tmp);
            }
        }
        public static void PoolShotIndicators(GameObject objectToPool, GameObject parentObject, int amountToPool)
        {
            shotIndicators = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, parentObject.transform);
                tmp.AddComponent<ObjectIDInfo>();
                tmp.SetActive(false);
                shotIndicators.Add(tmp);
            }
        }
        public static void PoolVerticalityIndicators(GameObject objectToPool, GameObject parentObject, int amountToPool)
        {
            verticalityIndicators = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, parentObject.transform);
                GameObject belowArmPivot = tmp.transform.GetChild(0).gameObject;
                GameObject aboveArmPivot = tmp.transform.GetChild(1).gameObject;
                tmp.AddComponent<ObjectIDInfo>();
                belowArmPivot.AddComponent<KeepVerticalRotation>();
                aboveArmPivot.AddComponent<KeepVerticalRotation>();
                tmp.SetActive(false);
                verticalityIndicators.Add(tmp);
            }
        }

        public static GameObject GetPooledStepObject(string ownerID = "none")
        {
            var amountToPool = Panel.PoolObjectsSteps;
            for (int i = 0; i < amountToPool; i++)
            {
                ObjectIDInfo info = stepIndicators[i].GetComponent<ObjectIDInfo>();
                if (info.ownerID == ownerID)
                {
                    return stepIndicators[i];
                }
                if (!stepIndicators[i].activeInHierarchy)
                {
                    info.ownerID = ownerID;
                    return stepIndicators[i];
                }
            }
            return null;
        }

        public static GameObject GetPooledVoiceObject(string ownerID = "none")
        {
            var amountToPool = Panel.PoolObjectsVoice;
            for (int i = 0; i < amountToPool; i++)
            {
                ObjectIDInfo info = voiceIndicators[i].GetComponent<ObjectIDInfo>();
                if (info.ownerID == ownerID)
                {
                    return voiceIndicators[i];
                }
                if (!voiceIndicators[i].activeInHierarchy)
                {
                    info.ownerID = ownerID;
                    return voiceIndicators[i];
                }
            }
            return null;
        }

        public static GameObject GetPooledShotObject(string ownerID = "none")
        {
            var amountToPool = Panel.PoolObjectsShots;
            for (int i = 0; i < amountToPool; i++)
            {
                ObjectIDInfo info = shotIndicators[i].GetComponent<ObjectIDInfo>();
                if (info.ownerID == ownerID)
                {
                    return shotIndicators[i];
                }
                if (!shotIndicators[i].activeInHierarchy)
                {
                    info.ownerID = ownerID;
                    return shotIndicators[i];
                }
            }
            return null;
        }

        public static GameObject GetPooledVerticalityObject(string ownerID = "none")
        {
            var amountToPool = Panel.PoolObjectsVerticality;
            for (int i = 0; i < amountToPool; i++)
            {
                ObjectIDInfo info = verticalityIndicators[i].GetComponent<ObjectIDInfo>();
                if (info.ownerID == ownerID)
                {
                    return verticalityIndicators[i];
                }
                if (!verticalityIndicators[i].activeInHierarchy)
                {
                    info.ownerID = ownerID;
                    return verticalityIndicators[i];
                }
            }
            return null;
        }
    }

}
