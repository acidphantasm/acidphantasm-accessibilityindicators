using BepInEx;
using BepInEx.Logging;
using EFT;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.AI;
using Color = UnityEngine.Color;
using Image = UnityEngine.UI.Image;

namespace acidphantasm_accessibilityindicators.Helpers
{
    public class DebugGizmos : MonoBehaviour
    {
        internal class TempCoroutine : MonoBehaviour
        {
            /// <summary>
            /// Class to run coroutines on a MonoBehaviour.
            /// </summary>
            internal class TempCoroutineRunner : MonoBehaviour { }
            public static Coroutine DisableAfterFade(GameObject obj, Image img, float delay)
            {
                if (obj != null)
                {
                    var runner = obj.AddComponent<TempCoroutineRunner>();
                    runner.StartCoroutine(RunFade(obj, img, delay));
                }
                return null;
            }
            public static Coroutine DisableAfterRefade(GameObject obj, Image img, Quaternion endValue, float delay)
            {
                if (obj != null)
                {
                    var runner = obj.AddComponent<TempCoroutineRunner>();
                    runner.StartCoroutine(RotateAndFadeAgain(obj, img, endValue, delay));
                }
                return null;
            }
            private static YieldInstruction fadeInstruction = new YieldInstruction();
            public static IEnumerator RunFade(GameObject obj, Image img, float fadeTime)
            {
                if (obj != null)
                {
                    var objCoroutineHolder = obj.GetComponent<ObjectIDInfo>().coroutineActive = true;
                    var elapsedTime = 0.0f;
                    Color currentColor = img.color;

                    while ((elapsedTime < fadeTime) && objCoroutineHolder)
                    {
                        yield return fadeInstruction;
                        elapsedTime += Time.deltaTime;
                        currentColor.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
                        img.color = currentColor;
                    }

                    obj.transform.rotation = Quaternion.identity;
                    objCoroutineHolder = false;
                    obj.SetActive(false);

                    TempCoroutineRunner runner = obj?.GetComponent<TempCoroutineRunner>();
                    if (runner != null)
                    {
                        Destroy(runner);
                    }
                }
            }
            public static IEnumerator RotateAndFadeAgain(GameObject obj, Image img, Quaternion endValue, float fadeTime)
            {
                if (obj != null)
                {
                    var objCoroutineHolder = obj.GetComponent<ObjectIDInfo>().coroutineActive = true;
                    var elapsedTime = 0.0f;
                    Color currentColor = img.color;
                    Quaternion startValue = obj.transform.rotation;

                    while ((elapsedTime < fadeTime) && objCoroutineHolder)
                    {
                        yield return fadeInstruction;
                        currentColor.a = 1.0f - Mathf.Clamp01(elapsedTime / fadeTime);
                        obj.transform.rotation = Quaternion.Lerp(startValue, endValue, elapsedTime / fadeTime);
                        elapsedTime += Time.deltaTime;
                        img.color = currentColor;
                    }

                    obj.transform.rotation = Quaternion.identity;
                    objCoroutineHolder = false;
                    obj.SetActive(false);

                    TempCoroutineRunner runner = obj?.GetComponent<TempCoroutineRunner>();
                    if (runner != null)
                    {
                        Destroy(runner);
                    }
                }
            }
        }
    }
}
