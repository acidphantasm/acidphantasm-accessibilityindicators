using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using acidphantasm_accessibilityindicators.Helpers;
using acidphantasm_accessibilityindicators.IndicatorUI;
using UnityEngine;

namespace acidphantasm_accessibilityindicators.Scripts
{
    internal class KeepNorthRotation : MonoBehaviour
    {
        public void Update()
        {
            var player = Utils.GetMainPlayer();
            Transform camera = player.CameraPosition;
            float lookDirection = camera.transform.rotation.eulerAngles.y;


            Panel.HUDCenterPoint.transform.rotation = Quaternion.Euler(0, 0, lookDirection + Panel.northDirection);
        }
    }
}
