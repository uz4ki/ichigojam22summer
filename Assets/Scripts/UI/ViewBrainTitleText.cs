using Title;
using UnityEngine;

namespace UI
{
    public class ViewBrainTitleText : MonoBehaviour
    {
        // Update is called once per frame
        private void Update()
        {
            if (TitleManager.Instance.JoyconL != null)
            {
                var accel = TitleManager.Instance.JoyconL.GetGyro().magnitude;
                transform.Rotate(Vector3.up * accel * 0.1f);
            }
        }
    }
}
