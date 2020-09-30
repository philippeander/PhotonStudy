using UnityEngine;
using System;
using TMPro;
using System.Collections;

public class AccelerationManager : MonoBehaviour
{
    static public AccelerationManager Instance;

    private Vector3 accel;
    private Vector3 prev;
    private const float kFilterFactor = 0.05f;
    public Matrix4x4 calibrationMatrix;
    private Vector3 accelerationSnapshot;
    private Coroutine m_calcCo;
     
    public Vector3 acceleration = Vector3.zero;

    public TextMeshProUGUI tmp;


    private void Awake()
    {
        Instance = this;
    }

    public void StartCalcMachine()
    {
        if (m_calcCo != null) StopCoroutine(InputCalc_Co());
        m_calcCo = StartCoroutine(InputCalc_Co());
    }

    private IEnumerator InputCalc_Co() 
    {
        Calibrate();

        yield return new WaitForEndOfFrame();

        while (true)
        {
            CalcAcceleration();

            yield return new WaitForEndOfFrame();
        }
    }

    private void CalcAcceleration() 
    {
        accel = Input.acceleration.normalized * kFilterFactor + (1.0f - kFilterFactor) * prev;

        Vector3 accelRounded = new Vector3((float)Math.Round(accel.x, 3),
                                           (float)Math.Round(accel.y, 3),
                                           (float)Math.Round(accel.z, 3));
        accel = accelRounded;
        prev = accel;

        acceleration = accel;
        acceleration.y = calibrationMatrix.MultiplyVector(accel).y;

        tmp.text = "X = " + accel.x +
                 "\nY = " + accel.y +
                 "\nZ = " + accel.z;

    }

    //Re-calibrar sempre que passar 1 ou 2 segundos paradao em uma posição
    //Re-calibrar sempre que os valores mudarem 0.1 a mais.

    //--------------------------------------------------------------------------
    public void Calibrate()
    {
        accel = Input.acceleration.normalized;

        Vector3 accelRounded = new Vector3((float)Math.Round(accel.x, 3),
                                           (float)Math.Round(accel.y, 3),
                                           (float)Math.Round(accel.z, 3));
        accel = accelRounded;
        accelerationSnapshot = accel;

        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0.0f, 0.0f, -1.0f), accelerationSnapshot);
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1.0f, 1.0f, 1.0f));
        calibrationMatrix = matrix.inverse;
    }

}
