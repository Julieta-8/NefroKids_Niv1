using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Knob : MonoBehaviour
{
    public enum KnobPosition
    {
        Posicion1,
        Posicion2,
        Posicion3,
        Posicion4
    }

    [Header("REFERENCIA")]
    [SerializeField] private Transform knob;

    [Header("ROTACIONES")]
    [SerializeField] private float[] rotationAngles;

    [SerializeField] private float rotationDuration = 0.4f;

    [Header("ESTADO")]
    [SerializeField]
    private KnobPosition currentPosition =
        KnobPosition.Posicion1;

    private bool isRotating = false;
    private bool isUnlocked = false;

    public KnobPosition CurrentPosition => currentPosition;

    public bool IsRotating => isRotating;

    public bool IsUnlocked => isUnlocked;


    // Avisará cuando la perilla terminó de llegar
    // a una posición.
    public event Action<KnobPosition> OnPositionReached;


    private void Start()
    {
        SetInitialPosition();
    }


    private void SetInitialPosition()
    {
        currentPosition = KnobPosition.Posicion1;

        if (rotationAngles == null ||
            rotationAngles.Length == 0)
        {
            Debug.Log(
                "Knob: No hay rotationAngles configurados."
            );

            return;
        }

        knob.rotation = Quaternion.Euler(
            0,
            0,
            rotationAngles[(int)currentPosition]
        );
    }


    // =====================================================
    // INTERACCIÓN
    // =====================================================

    public void EnableInteraction()
    {
        isUnlocked = true;
    }


    public void DisableInteraction()
    {
        isUnlocked = false;
    }


    // Estos dos pueden quedar como alias para mantener
    // compatibilidad con tu código anterior.

    public void Unlock()
    {
        EnableInteraction();
    }


    public void Lock()
    {
        DisableInteraction();
    }


    // =====================================================
    // MOVIMIENTO
    // =====================================================

    public void RotateRight()
    {
        if (!isUnlocked)
            return;

        if (isRotating)
            return;

        if (currentPosition ==
            KnobPosition.Posicion4)
            return;


        KnobPosition nextPosition =
            (KnobPosition)(
                (int)currentPosition + 1
            );


        StartCoroutine(
            RotateToPosition(nextPosition)
        );
    }

    public void RotateLeft()
    {
        if (!isUnlocked || isRotating)
            return;

        if (currentPosition <= 0)
            return;

        KnobPosition nextPosition =
            (KnobPosition)((int)currentPosition - 1);

        currentPosition = nextPosition;

        StartCoroutine(
            RotateToPosition(nextPosition)
        );
    }
    private IEnumerator RotateToPosition(
        KnobPosition targetPosition)
    {
        isRotating = true;


        float startAngle =
            knob.eulerAngles.z;

        float targetAngle =
            rotationAngles[
                (int)targetPosition
            ];


        float elapsed = 0f;


        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                elapsed / rotationDuration;


            float angle =
                Mathf.LerpAngle(
                    startAngle,
                    targetAngle,
                    t
                );


            knob.rotation =
                Quaternion.Euler(
                    0,
                    0,
                    angle
                );


            yield return null;
        }


        knob.rotation =
            Quaternion.Euler(
                0,
                0,
                targetAngle
            );


        currentPosition = targetPosition;

        isRotating = false;


        // Avisamos al Level_5
        OnPositionReached?.Invoke(
            currentPosition
        );
    }


    // =====================================================
    // VOLVER A UNA POSICIÓN
    // =====================================================

    public void ResetToPosition(
        KnobPosition targetPosition)
    {
        if (isRotating)
            return;

        StartCoroutine(
            ResetAnimation(targetPosition)
        );
    }


    private IEnumerator ResetAnimation(
        KnobPosition targetPosition)
    {
        isRotating = true;


        float startAngle =
            knob.eulerAngles.z;

        float targetAngle =
            rotationAngles[
                (int)targetPosition
            ];


        float elapsed = 0f;


        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;

            float t =
                elapsed / rotationDuration;


            float angle =
                Mathf.LerpAngle(
                    startAngle,
                    targetAngle,
                    t
                );


            knob.rotation =
                Quaternion.Euler(
                    0,
                    0,
                    angle
                );


            yield return null;
        }


        knob.rotation =
            Quaternion.Euler(
                0,
                0,
                targetAngle
            );


        currentPosition = targetPosition;

        isRotating = false;
    }
}