using System;
using JoyconLib_scripts;
using UnityEngine;

namespace General
{
    public class JoyconInput : SingletonMonoBehaviour<JoyconInput>
    {
        public Joycon JoyconR { get; private set; }
        public Vector2 stickVec { get; private set; }
    
        private void Start()
        {
            var joycons = JoyconManager.Instance.j;

            if ( joycons == null || joycons.Count <= 0 ) return;

            JoyconR = joycons.Find( c =>  !c.isLeft );
        }

        public bool InputStick()
        {
            var stick = JoyconR.GetStick();
            if (Mathf.Abs(stick[0]) < 0.8f && Mathf.Abs(stick[1]) < 0.8f)
            {
                stickVec = Vector2.zero;
                return false;
            }

            if (Mathf.Abs(stick[0]) > Mathf.Abs(stick[1]))
            {
                stickVec = Vector2.down * Mathf.Sign(stick[0]);
                return true;
            }
            else
            {
                stickVec = Vector2.right * Mathf.Sign(stick[1]);
                return true;
            }
        }
    }
}
