using System;
using System.Collections;
using System.Collections.Generic;
using General;
using JoyconLib_scripts;
using UnityEngine;

public class JoyconGyro : MonoBehaviour
{
    [SerializeField] private float loopValue;
    [SerializeField] private float nowAngle;
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
        if (Input.GetKeyDown(KeyCode.A)) ResetViewBrain();
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
            MiniGameLoader.Instance.GetHints();
        }
        if (nowAngle < - loopValue)
        {
            nowAngle += loopValue;
            MiniGameLoader.Instance.GetHints();
        }
        
        
        transform.rotation = Quaternion.Euler(0, (nowAngle * 360f) / loopValue, 0);
    }
}
