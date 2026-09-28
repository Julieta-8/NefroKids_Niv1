using System;
using System.Collections;
using UnityEngine;

public class Knob : MonoBehaviour
{
    public enum KnobPosition
    {
        Posicion1,
        Posicion2,
        Posicion3,
        Posicion4
    }

    [Header("KNOB")]
    [SerializeField] private Transform knob;

    [SerializeField] private float[] rotationAngles;

    [SerializeField] private float rotationDuration = 0.4f;


    [Header("ESTADO")]
    [SerializeField] private KnobPosition currentPosition;

    private bool isRotating = false;
    private bool isUnlocked = false;


    public KnobPosition CurrentPosition => currentPosition;

    public bool IsRotating => isRotating;

    public bool IsUnlocked => isUnlocked;


    // EVENTO
    public event Action<KnobPosition> OnPositionReached;


    private void Start()
    {
        currentPosition = KnobPosition.Posicion1;

        knob.rotation = Quaternion.Euler(
            0,
            0,
            rotationAngles[(int)currentPosition]
        );
    }


    public void Unlock()
    {
        isUnlocked = true;
    }


    public void Lock()
    {
        isUnlocked = false;
    }


    public void RotateRight()
    {
        if (!isUnlocked)
            return;

        if (isRotating)
            return;

        if (currentPosition == KnobPosition.Posicion4)
            return;


        currentPosition =
            (KnobPosition)((int)currentPosition + 1);


        StartCoroutine(RotateAnimation());
    }


    private IEnumerator RotateAnimation()
    {
        isRotating = true;


        float startAngle = knob.eulerAngles.z;

        float targetAngle =
            rotationAngles[(int)currentPosition];


        float elapsed = 0f;


        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                elapsed / rotationDuration;


            float angle = Mathf.LerpAngle(
                startAngle,
                targetAngle,
                t
            );


            knob.rotation =
                Quaternion.Euler(0, 0, angle);


            yield return null;
        }


        knob.rotation =
            Quaternion.Euler(
                0,
                0,
                targetAngle
            );


        isRotating = false;


        // AVISARLE AL LEVEL 5
        OnPositionReached?.Invoke(currentPosition);
    }
}