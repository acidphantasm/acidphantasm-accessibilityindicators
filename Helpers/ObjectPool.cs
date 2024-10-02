using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using acidphantasm_accessibilityindicators.IndicatorUI;

namespace acidphantasm_accessibilityindicators.Helpers
{

    public class ObjectPool : MonoBehaviour
    {
        public static ObjectPool SharedInstance;
        public static List<GameObject> runIndicators;
        public static List<GameObject> sprintIndicators;
        public static List<GameObject> shotIndicators;

        void Awake()
        {
            SharedInstance = this;
        }
        public static void PoolRunIndicators(GameObject objectToPool, GameObject parentObject, int amountToPool)
        {
            runIndicators = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, parentObject.transform);
                tmp.AddComponent<ObjectIDInfo>();
                tmp.SetActive(false);
                runIndicators.Add(tmp);
            }
        }
        public static void PoolSprintIndicators(GameObject objectToPool, GameObject parentObject, int amountToPool)
        {
            sprintIndicators = new List<GameObject>();
            GameObject tmp;
            for (int i = 0; i < amountToPool; i++)
            {
                tmp = Instantiate(objectToPool, parentObject.transform);
                tmp.AddComponent<ObjectIDInfo>();
                tmp.SetActive(false);
                sprintIndicators.Add(tmp);
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

        public static GameObject GetPooledRunObject(string id = "none")
        {
            var amountToPool = Indicators.poolObjectsSteps;
            for (int i = 0; i < amountToPool; i++)
            {
                if (!runIndicators[i].activeInHierarchy)
                {
                    return runIndicators[i];
                }
                /*
                ObjectIDInfo objectId = runIndicators[i].GetComponent<ObjectIDInfo>();
                if (objectId.id == id)
                {
                    return runIndicators[i];
                }
                if (objectId.id == 0)
                {
                    objectId.id = id;
                    return runIndicators[i];
                }
                */
            }
            return null;
        }

        public static GameObject GetPooledSprintObject(string id = "none")
        {
            var amountToPool = Indicators.poolObjectsSteps;
            for (int i = 0; i < amountToPool; i++)
            {
                if (!sprintIndicators[i].activeInHierarchy)
                {
                    return sprintIndicators[i];
                }
                /*
                ObjectIDInfo objectId = sprintIndicators[i].GetComponent<ObjectIDInfo>();
                if (objectId.id == id)
                {
                    return sprintIndicators[i];
                }
                if (objectId.id == 0)
                {
                    objectId.id = id;
                    return sprintIndicators[i];
                }
                */
            }
            return null;
        }

        public static GameObject GetPooledShotObject(string id = "none")
        {
            var amountToPool = Indicators.poolObjectsShots;
            for (int i = 0; i < amountToPool; i++)
            {
                if (!shotIndicators[i].activeInHierarchy)
                {
                    return shotIndicators[i];
                }
                /*
                ObjectIDInfo objectId = shotIndicators[i].GetComponent<ObjectIDInfo>();
                if (objectId.id == id)
                {
                    return shotIndicators[i];
                }
                if (objectId.id == 0)
                {
                    objectId.id = id;
                    return shotIndicators[i];
                }
                */
            }
            return null;
        }
    }

}
