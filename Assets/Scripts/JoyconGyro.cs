using System;
using System.Collections;
using System.Collections.Generic;
using General;
using UnityEngine;

public class JoyconGyro : MonoBehaviour
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
        if (GameManager.Instance.IsPlayingGame) CalcJoyconGyro();
    }

    private void ResetViewBrain()
    {
        viewBrain.rotation = Quaternion.identity;
    }

    private void CalcJoyconGyro()
    {
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
