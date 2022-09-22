using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] private float loopValue;
    [SerializeField] private float nowAngle;
    [SerializeField] private int rotateCount;
    [SerializeField] private Transform viewBrain;
    private Quaternion prevVec;

    private Joycon joyconL;
    private void Start()
    {
        var joycons = JoyconManager.Instance.j;

        if ( joycons == null || joycons.Count <= 0 ) return;

       joyconL = joycons.Find( c =>  c.isLeft );
    }

    private void Update()
    {
        if (joyconL.GetButtonDown(Joycon.Button.DPAD_DOWN))
        {
            viewBrain.rotation = Quaternion.identity;
        }


        nowAngle += joyconL.GetGyro().y * Time.deltaTime;
        if (nowAngle > loopValue)
        {
            nowAngle -= loopValue;
            rotateCount++;
        }
        if (nowAngle < - loopValue)
        {
            nowAngle += loopValue;
            rotateCount++;
        }
        
        transform.rotation = joyconL.GetVector();
    }
}
