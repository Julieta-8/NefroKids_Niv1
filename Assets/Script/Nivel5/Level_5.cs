//using System.Collections;
//using System.Collections.Generic;
//using System.Diagnostics;
//using UnityEngine;
//using UnityEngine.UI;
//using TMPro;

//public class Level_5 : MonoBehaviour
//{
//    /*Va constar de un nivel donde se enfocará en la perilla y como moverla, + como cerrar el programa
     
//     -hacer zoom a la perilla
//    -Arrastrar la perilla cerrada para desbloquearla
//    -En esta parte se debe lograr que la perilla pueda rotar en vez de moverla, que mantenga su psociones, pero cuando el usuario la toca debe solo rotar
//    y debe tener 4 psoiciones con sensores que detecten cuando la perilla los apunta (esto podria hacerse con flechas como botones, una indicando a la derecha y otra a la izquierda, donde invocan una animacion del a perilla girando de una posicion a la otra, en vez de tener que adivinar dond eel ususario psociono la perilla)
//    1)drenaje pos 1 (riku va a mencionar para que sirve cada una cuando cosnigue la correcta) --> cuando el jugador mueve la perilla deberia haber un boton que diga "verificar" para saber si esta realmente seguro si es esa psoiciones, en caso de ser incorrecto aparecerá riku con su texto
//    2)el nene debera guiar el flujo (va a ser el cable con una pantalla atras que representara el liquido, debera arrastrarla para que se drene)
//    3) cambio de posiciones, ahora la bolsa debee recibir, cambio de perilla a pos 2(riku vuelve a explicar para que sirve)
//    4)vuelve a seguir el liquido
//    5)lavado completado
//    6)infusion: pos 3
//    7)cierre: pos 4
//     */

//    // Start is called before the first frame update

//    //ver tema perilla
//    public enum Step
//    {
//        Intro,
//        // TUTORIAL
//        AndyZoom,//en duda
//        //perfecto
//        OpenButton,

//        DragButton,

//        DragToPosition1,
//        Position1Placed,
//        //cambiar de escena para que el niño pueda guiar el flujo
//        RedirectFluid,
//        FluidRedirected,
//        DragtoPosition2,
//        Position2Placed,
//        RedirectToBag2,
//        BagCompleted,
//        DragtoPosition3,
//        Position3Placed,
//        DragtoPosition4,
//        Position4Placed,
//        Finished
//    }
//    public enum KnobPosition
//    {
//        Posicion1,
//        Posicion2,
//        Posicion3,
//        Posicion4
//    }
//    //PERILLA////////////////////////////////
//    [SerializeField]
//    private KnobPosition currentPosition;

//    [SerializeField]
//    private KnobPosition targetPosition;
//    [SerializeField]
//    private float rotationStep = 90f;

//    [SerializeField]
//    private float rotationDuration = 0.4f;

//    private bool isRotating;
//    private bool knobUnlocked;
//    //BACKGROUNDS 1 PERILLA 2 BLANCO/////////////////////

//    ///PHASES
//    ///uno para perilla otro para la transfusion
//    public GameObject phase1Objects;
//    public GameObject phase2Objects;

//    /// <summary>/////////////////////////
//    /// UI
//    /// </summary>
//    public TMP_Text instructionText;

//    public Button nextButton;

//    public GameObject dialoguePanel;

//    public GameObject congratsPanel;


//    public TMP_Text DatoCuriosoText;


//    public GameObject DatoCuriosoPanel;


//    [Header("RIKU")]
//    public SpriteRenderer rikuRenderer;

//    public Sprite rikuNeutral;
//    public Sprite rikuCurious;

//    private bool rikuNeutralState = true;

//    public Sprite CabezarikuCurious;


//    [Header("ESCENA SIGUIENTE")]
//    public string nextSceneName;

//    [Header("REACT CONNECTION")]
//    private ReactConnection bridge;
//    private bool isRunning;

//    /// <summary>
//    /// //////////////VARIABLES
//    /// </summary>
//    //BOOLS

//    void Start()


//    {

//        switch (currentStep)
//        {
//            case Step.Intro:

//                currentStep = Step.OpenButton;


//                Debug.Log("PASO DESPUES: " + currentStep);


//                break;

//            case Step.DragButton:
//                currentStep = Step.DragToPosition1;
//                break;

//            case Step.Position1Placed:

//                currentStep = Step.RedirectFluid;
//                break;

//            case Step.FliudRedirected:

//                currentStep = Step.DragToPosition2;

//                break;

//            case Step.Position2Placed:

//                currentStep = Step.RedirectToBag2;

//                break;

//            case Step.BagCompleted:

//                currentStep = Step.DragtoPosition3;

//                break;
//            case Step.Position3Placed:

//                currentStep = Step.DragtoPosition4;

//                break;

//            case Step.Position4Placed:

//                currentStep = Step.Finished;

//                ShowDialogue();

//                StartCoroutine(FinishRoutine());

//                break;
//        }
//    }

//    void ToggleRikuExpression()
//        {
//            if (rikuRenderer == null) return;

//            rikuNeutralState = !rikuNeutralState;

//            if (rikuNeutralState)
//            {
//                rikuRenderer.sprite = rikuNeutral;
//            }
//            else
//            {
//                rikuRenderer.sprite = rikuCurious;
//            }
//        }

//    /// <summary>
//    /// ///////PERILLA ROTATION/////////
//    /// </summary>
//    /// 
//    float GetAngleForPosition(KnobPosition position)
//    {
//        switch (position)
//        {
//            case KnobPosition.Posicion1:
//                return 0f;

//            case KnobPosition.Posicion2:
//                return 90f;

//            case KnobPosition.Posicion3:
//                return 180f;

//            case KnobPosition.Posicion4:
//                return 270f;
//        }

//        return 0f;
//    }
//    public void RotateRight()
//    {
//        if (isRotating)
//            return;

//        if (!knobUnlocked)
//            return;

//        if (currentPosition == KnobPosition.Posicion4)
//            return;

//        KnobPosition nextPosition =
//            (KnobPosition)((int)currentPosition + 1);

//        RotateToPosition(nextPosition);
//    }
//    public void RotateLeft()
//    {
//        if (isRotating)
//            return;

//        if (!knobUnlocked)
//            return;

//        if (currentPosition == KnobPosition.Posicion1)
//            return;

//        KnobPosition nextPosition =
//            (KnobPosition)((int)currentPosition - 1);

//        RotateToPosition(nextPosition);
//    }
//    void HandleMouseInput()
//        {
//            if (Input.GetMouseButtonDown(0))
//            {
//                Debug.Log("CLICK DETECTADO");

//                if (Camera.main == null)
//                {
//                    Debug.LogError("NO HAY UNA CAMARA CON TAG MainCamera");
//                    return;
//                }

//                Vector2 worldPoint =
//                    Camera.main.ScreenToWorldPoint(Input.mousePosition);

//                Collider2D hit =
//                    Physics2D.OverlapPoint(worldPoint);

//                if (hit == null)
//                {
//                    Debug.Log("NO SE ENCONTRO NINGUN COLLIDER");
//                    return;
//                }

//                GameObject clickedObject = hit.gameObject;

//                Debug.Log("COLLIDER ENCONTRADO: " + clickedObject.name);


//                if (hit/*.collider*/ != null)
//                {
//                    //GameObject clickedObject = hit/*.collider*/.gameObject;
//                    Debug.Log(clickedObject.name);
                  
//                    if (currentStep == Step.DragLiquido1 &&
//                        clickedObject == Liquid1Obg)
//                    {
//                        BeginDrag(clickedObject);
//                        return;
//                    }

//                    if (currentStep == Step.DragLiquido2 &&
//                        clickedObject == Liquid2Obg)
//                    {
//                        BeginDrag(clickedObject);
//                        return;
//                    }
                 
//                }
//            }

//            // DRAG
//            if (draggingObject != null &&
//                Input.GetMouseButton(0))
//            {
//                Vector3 mouseWorld =
//                    Camera.main.ScreenToWorldPoint(Input.mousePosition);

//                mouseWorld.z = 0f;

//                draggingObject.transform.position =
//                    mouseWorld + draggingOffset;
//            }

//            // RELEASE
//            if (draggingObject != null &&
//                Input.GetMouseButtonUp(0))
//            {
//                TryDrop(draggingObject);

//                draggingObject = null;
//            }
//        }

//        void BeginDrag(GameObject go)
//        {
//            draggingObject = go;

//            Vector3 mouseWorld =
//                Camera.main.ScreenToWorldPoint(Input.mousePosition);

//            mouseWorld.z = 0f;

//            draggingOffset =
//                go.transform.position - mouseWorld;
//        }

//        void TryDrop(GameObject go)
//        {
//            bool insideDropZone = false;


//            // DROP MANOS
//            if ((go == soapObject || go == towelObject) &&
//                handsDropZone != null)
//            {
//                insideDropZone =
//                    handsDropZone.OverlapPoint(go.transform.position);
//            }

//            // SI FALLA
//            if (!insideDropZone)
//            {
//                ReturnObject(go);
//                return;
//            }

//            // LIQUIDO1
//            if (go == Liquid1Obg &&
//                currentStep == Step.RedirectFluid)
//            {
//                PlaceVest();
//                return;
//            }

//            // LIQUIDO2
//            if (go == Liquid2Obg &&
//                currentStep == Step.RedirectToBag2)
//            {
//                PlaceMask();
//                return;
//            }

        
//        }

//        void ReturnObject(GameObject go)
//        {
//            // PARTE1
//            if (go == Liquid1Obg &&
//                LiquidStartPosition1 != null)
//            {
//                go.transform.position =
//                    liquidStartPosition.position;
//            }

//            // PARTE2
//            if (go == Liquid2Obg &&
//                LiquidStartPosition2 != null)
//            {
//                go.transform.position =
//                    liquid2StartPosition.position;
//            }
//        }
//        void PlaceLiquid1()
//        {
//            if (vestPlaced) return;

//            vestPlaced = true;

//            vestObject.SetActive(false);

//            mannequinRenderer.sprite =
//                mannequinWithVest;

//            currentStep = Step.VestPlaced;

//            ShowDialogue();
//        }

//        void PlaceLiquid2()
//        {
//            if (maskPlaced) return;

//            maskPlaced = true;

//            maskObject.SetActive(false);

//            mannequinRenderer.sprite =
//                mannequinComplete;

//            timingPhase1 = false;

//            currentStep = Step.MaskPlaced;
//            ShowHandwashingTip2();
//            DatoCuriosoPanel.gameObject.SetActive(false);
//            DatoCuriosoText.gameObject.SetActive(false);
//            ShowDialogue();
//        }
//    void UpdateInstruction()
//    {
//        if (instructionText == null) return;

//        switch (currentStep)
//        {
//            case Step.Intro:

//                instructionText.text =
//                    "�Hola! Soy Riku y voy a ense�arte c�mo prepararte correctamente.";

//                break;
//            case Step.OpenButton:

//                instructionText.text =
//                    "Para empezar, hay que anrir el tapón";

//                break;
//            case Step.DragButton:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.DragToPosition1:

//                instructionText.text =
//                    "hay diferentes lugares, cada uno es diferente, el primer paso es abrir para dirigir el fluido. cual posicion será? ";

//                break;
//            case Step.Position1Placed:

//                instructionText.text =
//                    "Perfecto:(explciar que es) ";

//                break;
//            case Step.RedirectFluid:

//                instructionText.text =
//                    "Dirije el fluido ";

//                break;
//            case Step.FluidRedirected:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.DragtoPosition2:

//                instructionText.text =
//                    " Ahora vamos a realizar el drenaje";

//                break;
//            case Step.Position2Placed:

//                instructionText.text =
//                    " (Explicar)";

//                break;
//            case Step.RedirectToBag2:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.BagCompleted:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.DragtoPosition3:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.Position3Placed:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.DragtoPosition4:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.Position4Placed:

//                instructionText.text =
//                    " ";

//                break;
//            case Step.Finished:

//                instructionText.text =
//                    "�Completado!";

//                break;
//        }
//    }


//    // Update is called once per frame
//    void Update()
//    {
        
//    }
//}
