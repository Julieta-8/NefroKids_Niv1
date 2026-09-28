//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Knob : MonoBehaviour
//{
//    [SerializeField] private Transform knob;

//    [SerializeField] private float[] rotationAngles;

//    [SerializeField] private float rotationDuration = 0.4f;

//    private int currentPosition = 0;

//    private bool isRotating = false;
//    private bool isUnlocked = false;

//    public int CurrentPosition => currentPosition;

//    public void Unlock()
//    {
//        isUnlocked = true;
//    }

//    public void RotateRight()
//    {
//        if (!isUnlocked || isRotating)
//            return;

//        if (currentPosition >= rotationAngles.Length - 1)
//            return;

//        currentPosition++;

//        StartCoroutine(RotateAnimation());
//    }
//    private IEnumerator RotateAnimation()
//    {
//        isRotating = true;

//        float startAngle = knob.eulerAngles.z;
//        float targetAngle = rotationAngles[currentPosition];

//        float elapsed = 0f;

//        while (elapsed < rotationDuration)
//        {
//            elapsed += Time.deltaTime;

//            float t = elapsed / rotationDuration;

//            float angle = Mathf.LerpAngle(
//                startAngle,
//                targetAngle,
//                t
//            );

//            knob.rotation = Quaternion.Euler(0, 0, angle);

//            yield return null;
//        }

//        knob.rotation =
//            Quaternion.Euler(0, 0, targetAngle);

//        isRotating = false;
//    }
//    public void RotateLeft()
//    {
//        if (!isUnlocked || isRotating)
//            return;

//        // Podríamos bloquearlo porque el sistema
//        // solo permite avanzar.
//        ShowWrongDirection();

//        return;
//    }
//}