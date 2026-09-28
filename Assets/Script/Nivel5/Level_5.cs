using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Level_5 : MonoBehaviour
{
    /*Va constar de un nivel donde se enfocará en la perilla y como moverla, + como cerrar el programa
     
     -hacer zoom a la perilla
    -Arrastrar la perilla cerrada para desbloquearla
    -En esta parte se debe lograr que la perilla pueda rotar en vez de moverla, que mantenga su psociones, pero cuando el usuario la toca debe solo rotar
    y debe tener 4 psoiciones con sensores que detecten cuando la perilla los apunta (esto podria hacerse con flechas como botones, una indicando a la derecha y otra a la izquierda, donde invocan una animacion del a perilla girando de una posicion a la otra, en vez de tener que adivinar dond eel ususario psociono la perilla)
    1)drenaje pos 1 (riku va a mencionar para que sirve cada una cuando cosnigue la correcta) --> cuando el jugador mueve la perilla deberia haber un boton que diga "verificar" para saber si esta realmente seguro si es esa psoiciones, en caso de ser incorrecto aparecerá riku con su texto
    2)el nene debera guiar el flujo (va a ser el cable con una pantalla atras que representara el liquido, debera arrastrarla para que se drene)
    3) cambio de posiciones, ahora la bolsa debee recibir, cambio de perilla a pos 2(riku vuelve a explicar para que sirve)
    4)vuelve a seguir el liquido
    5)lavado completado
    6)infusion: pos 3
    7)cierre: pos 4
     */

    // Start is called before the first frame update

    //ver tema perilla
    public enum Step
    {
        Intro,
        OpenPinza,

        // 2. DRENAJE
        ChoosePosition1,
        Position1Correct,
        RedirectFluid1,

        // 3. LAVADO
        ChoosePosition2,
        Position2Correct,
        RedirectFluid2,

        // 4. INFUSIÓN
        ChoosePosition3,
        Position3Correct,
        Infusion,

        // 5. PIN
        InsertPin,

        // 6. CIERRE
        ChoosePosition4,
        Position4Correct,

        Finished
    }

    //PERILLA////////////////////////////////
    [SerializeField]
    private Knob knob;
    [SerializeField]
    private Knob.KnobPosition currentPosition;

    [SerializeField]
    private Knob.KnobPosition targetPosition;
    [SerializeField]
    private float rotationStep = 90f;

    [SerializeField]
    private float rotationDuration = 0.4f;

    private bool isRotating;
    private bool knobUnlocked;
    //BACKGROUNDS 1 PERILLA 2 BLANCO/////////////////////

    ///PHASES
    ///uno para perilla otro para la transfusion
    public GameObject phase1Objects;
    public GameObject phase2Objects;

    /// <summary>/////////////////////////
    /// UI
    /// </summary>
    public TMP_Text instructionText;

    public Button nextButton;

    public GameObject dialoguePanel;

    public GameObject congratsPanel;

    public GameObject rightArrow;
    public GameObject leftArrow;
    public GameObject verifyButton;


    public TMP_Text DatoCuriosoText;


    public GameObject DatoCuriosoPanel;


    [Header("RIKU")]
    public SpriteRenderer rikuRenderer;

    public Sprite rikuNeutral;
    public Sprite rikuCurious;

    private bool rikuNeutralState = true;

    public Sprite CabezarikuCurious;

    public Transform RikuStartPosition;
    public Transform PanelStartPosition;

    [Header("ESCENA SIGUIENTE")]
    public string nextSceneName;

    [Header("REACT CONNECTION")]
    private ReactConnection bridge;
    private bool isRunning;

    /// <summary>
    /// //////////////VARIABLES
    /// </summary>
    //BOOLS
    private GameObject draggingObject;
    private Vector3 draggingOffset;
    public Step currentStep = Step.Intro;

    /// <summary>
    /// /////TRANSFUSION///////
    /// </summary>
    /// 
    public SpriteRenderer Chico;
    public SpriteRenderer SegundaBolsa;

    public Sprite mannequinNormal;

    public Sprite Bolsa2SinL;
    public Sprite Bolsa2ConL;

    public GameObject Liquid1Obj;
    public GameObject Liquid2Obj;
    public Transform Liquid1Position;
    public Transform Liquid2Position;

    public GameObject pinObject;
    public GameObject pinzaObject;



    public Transform Bolsa1Position;
    public Transform Bolsa2Position;

    public Collider2D ChicoDropZone;

    private bool Position1Placed = false;
    private bool Position2Placed = false;
    private bool BagCompleted = false;
    private bool Position3Placed = false;
    private bool Position4Placed = false;
    private bool FluidRedirected = false;
    private void SetupInitialState()
    {
        phase1Objects.SetActive(true);
        phase2Objects.SetActive(false);

        Liquid1Obj.SetActive(false);
        Liquid2Obj.SetActive(false);

        SegundaBolsa.gameObject.SetActive(false);

        DatoCuriosoPanel.SetActive(false);
        congratsPanel.SetActive(false);

        nextButton.gameObject.SetActive(false);

        currentStep = Step.Intro;
        Start();
    }

    private void Start()
    {
        knob.OnPositionReached += OnKnobPositionReached;
        ChangeStep(Step.Intro);

    }
    public void ChangeStep(Step newStep)
    {
        currentStep = newStep;

        UpdateInstruction();

        SetObjectsForStep();
    }

    private void SetObjectsForStep()
    {
        // Primero apagamos elementos temporales.

        Liquid1Obj.SetActive(false);
        Liquid2Obj.SetActive(false);

        pinObject.SetActive(false);

        verifyButton.gameObject.SetActive(false);

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);


        switch (currentStep)
        {
            case Step.OpenPinza:

                pinzaObject.SetActive(true);

                break;


            case Step.ChoosePosition1:

                verifyButton.gameObject.SetActive(true);
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(true);

                break;


            case Step.RedirectFluid1:

                Liquid1Obj.SetActive(true);

                break;


            case Step.ChoosePosition2:

                verifyButton.gameObject.SetActive(true);
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(true);

                SegundaBolsa.gameObject.SetActive(true);

                break;


            case Step.RedirectFluid2:

                Liquid2Obj.SetActive(true);

                break;


            case Step.ChoosePosition3:

                verifyButton.gameObject.SetActive(true);
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(true);

                break;


            case Step.InsertPin:

                pinObject.SetActive(true);

                break;


            case Step.ChoosePosition4:

                verifyButton.gameObject.SetActive(true);
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(true);

                break;
        }
    }
    void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
        rikuRenderer.enabled = true;
        nextButton.gameObject.SetActive(true);
        instructionText.gameObject.SetActive(true);
        UpdateInstruction();

    }


    void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        instructionText.gameObject.SetActive(false);
        rikuRenderer.enabled = false;
        nextButton.gameObject.SetActive(false);
    }
    void ToggleRikuExpression()
        {
            if (rikuRenderer == null) return;

            rikuNeutralState = !rikuNeutralState;

            if (rikuNeutralState)
            {
                rikuRenderer.sprite = rikuNeutral;
            }
            else
            {
                rikuRenderer.sprite = rikuCurious;
            }
        }
    public void NextButton()
    {
        switch (currentStep)
        {
            case Step.Intro:

                HideDialogue();

                currentStep = Step.OpenPinza;

                StartOpenPinza();

                break;
        }
    }
    public void OnPinzaOpened()
    {
        currentStep = Step.ChoosePosition1;

        StartChoosePosition1();
    }
    /// <summary>
    /// ///////PERILLA ROTATION/////////
    /// </summary>
    /// 
    float GetAngleForPosition(Knob.KnobPosition position)
    {
        switch (position)
        {
            case Knob.KnobPosition.Posicion1:
                return 0f;

            case Knob.KnobPosition.Posicion2:
                return 90f;

            case Knob.KnobPosition.Posicion3:
                return 180f;

            case Knob.KnobPosition.Posicion4:
                return 270f;
        }

        return 0f;
    }
    private void StartPin()
    {
        pinObject.SetActive(true);

        instructionText.text =
            "Ahora colocá el PIN para asegurar el sistema.";
    }
   
    private void StartChoosePosition1()
    {
        instructionText.text =
            "Ahora tenemos que elegir la posición correcta para comenzar el drenaje.";

        verifyButton.gameObject.SetActive(true);

        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);

        knob.EnableInteraction();

        targetPosition = Knob.KnobPosition.Posicion1;
    }
    private void StartChoosePosition2()
    {
        targetPosition = Knob.KnobPosition.Posicion2;

        verifyButton.gameObject.SetActive(true);

        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);

        knob.EnableInteraction();
    }
    private void StartChoosePosition3()
    {
        targetPosition = Knob.KnobPosition.Posicion3;
        verifyButton.gameObject.SetActive(true);

        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);

        knob.EnableInteraction();
        verifyButton.gameObject.SetActive(true);

    }
    private void CorrectPosition1()
    {
        verifyButton.gameObject.SetActive(false);

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);

        knob.DisableInteraction();

        currentStep = Step.Position1Correct;

        ShowDialogue();
    }
    private void ContinueFromPosition1()
    {
        currentStep = Step.RedirectFluid1;

        Liquid1Obj.SetActive(true);

        instructionText.text =
            "Ahora seguí el recorrido del líquido hasta la bolsa de drenaje.";
    }
    private void CorrectPosition2()
    {
        verifyButton.gameObject.SetActive(false);

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        knob.DisableInteraction();

        currentStep = Step.Position2Correct;

        ShowDialogue(
           
        );
    }
    private void ContinueFromPosition2()
    {
        currentStep = Step.RedirectFluid2;

        Liquid2Obj.SetActive(true);

        instructionText.text =
            "Seguí el recorrido del líquido hasta la bolsa.";
    }
    private void CorrectPosition3()
    {
        verifyButton.gameObject.SetActive(false);

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        currentStep = Step.Position3Correct;

        ShowDialogue(
           
        );
    }
    private void ContinueFromPosition3()
    {
        currentStep = Step.Infusion;

        StartInfusion();
    }
    private void CorrectPosition4()
    {
        verifyButton.gameObject.SetActive(false);

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        currentStep = Step.Position4Correct;

        ShowDialogue(
           
        );
    }
    private void ContinueFromPosition4()
    {
        currentStep = Step.Finished;

        FinishLevel();
    }
    private void OnKnobPositionReached(Knob.KnobPosition position)
    {
        switch (currentStep)
        {
            case Step.ChoosePosition1:

                if (position == Knob.KnobPosition.Posicion1)
                {
                    CorrectPosition1();
                }
                else
                {
                    WrongPosition();
                }

                break;


            case Step.ChoosePosition2:

                if (position == Knob.KnobPosition.Posicion2)
                {
                    CorrectPosition2();
                }
                else
                {
                    WrongPosition();
                }
                break;


            case Step.ChoosePosition3:

                if (position == Knob.KnobPosition.Posicion3)
                {
                    CorrectPosition3();
                }
                else
                {
                    WrongPosition();
                }
                break;


            case Step.ChoosePosition4:

                if (position == Knob.KnobPosition.Posicion4)
                {
                    CorrectPosition4();
                }
                else
                {
                    WrongPosition();
                }
                break;
        }
    }
    public void VerifyPosition()
    {
        if (knob.CurrentPosition ==     Knob.KnobPosition.Posicion1)
        {
            CorrectPosition1();
        }
        else
        {
            WrongPosition(
            );
        }
    }
    private void WrongPosition()
    {
        knob.DisableInteraction();

        switch (currentStep)
        {
            case Step.ChoosePosition1:

                ShowDialogue(//DEBERIA MOSTRAR EL ERROR
                );

                break;


            case Step.ChoosePosition2:
                instructionText.text = "Esa no es la posición que necesitamos para realizar el lavado. Probá nuevamente.";
                ShowDialogue( );

                break;


            case Step.ChoosePosition3:
                instructionText.text = "Todavía no. Buscá la posición que permite comenzar la infusión.";

                ShowDialogue( );

                break;


            case Step.ChoosePosition4:
                instructionText.text = "Todavía no. Necesitamos llevar la perilla hasta la posición final para cerrar el sistema.";
                ShowDialogue( );

                break;
        }
    }
    private void StartInfusion()
    {
        currentStep = Step.Infusion;

        instructionText.text =
            "Ahora seguí el recorrido del líquido hasta que termine la infusión.";

        knob.DisableInteraction();

        verifyButton.gameObject.SetActive(false);

        // Mostrar el líquido de infusión
        Liquid2Obj.SetActive(true);
    }
    public void CompleteInfusion()
    {
        Liquid2Obj.SetActive(false);

        currentStep = Step.InsertPin;

        StartPin();
    }
    public void CompletePinza()
    {
        pinzaObject.SetActive(false);

        StartChoosePosition1();
    }
    void HandleMouseInput()
        {
            if (Input.GetMouseButtonDown(0))
            {

               

                Vector2 worldPoint =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition);

                Collider2D hit =
                    Physics2D.OverlapPoint(worldPoint);

                if (hit == null)
                {
                    return;
                }

                GameObject clickedObject = hit.gameObject;



                if (hit/*.collider*/ != null)
                {
                    //GameObject clickedObject = hit/*.collider*/.gameObject;
                    //Debug.Log(clickedObject.name);
                  
                    if (currentStep == Step.RedirectFluid1 &&
                        clickedObject == Liquid1Obj)
                    {
                        BeginDrag(clickedObject);
                        return;
                    }

                    if (currentStep == Step.Infusion &&
                        clickedObject == Liquid2Obj)
                    {
                        BeginDrag(clickedObject);
                        return;
                    }
                 
                }
            }

            // DRAG
            if (draggingObject != null &&
                Input.GetMouseButton(0))
            {
                Vector3 mouseWorld =
                    Camera.main.ScreenToWorldPoint(Input.mousePosition);

                mouseWorld.z = 0f;

                draggingObject.transform.position =
                    mouseWorld + draggingOffset;
            }

            // RELEASE
            if (draggingObject != null &&
                Input.GetMouseButtonUp(0))
            {
                TryDrop(draggingObject);

                draggingObject = null;
            }
        }
    /// <summary>
    /// /////////////////////////////////////////////
    /// </summary>
    /// <param name="go"></param>
    /// 
    private void StartFluid1()
    {
        Liquid1Obj.SetActive(true);

        instructionText.text =
            "Seguí el líquido hasta la bolsa de drenaje.";
    }
    private void StartFluid2()
    {
        Liquid2Obj.SetActive(true);

        instructionText.text =
            "Seguí el líquido hasta la bolsa.";
    }
    void BeginDrag(GameObject go)
        {
            draggingObject = go;

            Vector3 mouseWorld =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseWorld.z = 0f;

            draggingOffset =
                go.transform.position - mouseWorld;
        }

        void TryDrop(GameObject go)
        {
            bool insideDropZone = false;


            // DROP MANOS
            if ((go == Liquid1Obj) &&
                Liquid1Position != null)
            {
                insideDropZone =
                    ChicoDropZone.OverlapPoint(go.transform.position);
            }

            // SI FALLA
            if (!insideDropZone)
            {
                ReturnObject(go);
                return;
            }

            // LIQUIDO1
            if (go == Liquid1Obj &&
                currentStep == Step.RedirectFluid1)
            {
            CompleteFluid1();
                return;
            }

            // LIQUIDO2
            if (go == Liquid2Obj &&
                currentStep == Step.Infusion)
            {
            PlaceLiquid2();
                return;
            }

        
        }

        void ReturnObject(GameObject go)
        {
            // PARTE1
            if (go == Liquid1Obj &&
                Liquid1Position != null)
            {
                go.transform.position =
                    Liquid1Position.position;
            }

            // PARTE2
            if (go == Liquid2Obj &&
                Liquid2Position != null)
            {
                go.transform.position =
                    Liquid2Position.position;
            }
        }
        void CompleteFluid1()
        {
        Liquid1Obj.SetActive(false);

        Chico.sprite = mannequinNormal;

        currentStep = Step.ChoosePosition2;

        ShowDialogue();
    }

        void PlaceLiquid2()
        {
            if (BagCompleted) return;

        BagCompleted = true;

            Liquid2Obj.SetActive(false);

            SegundaBolsa.sprite =
                Bolsa2ConL;


            currentStep = Step.InsertPin;

            DatoCuriosoPanel.gameObject.SetActive(false);
            DatoCuriosoText.gameObject.SetActive(false);
            ShowDialogue();
        }
    private void UpdateInstruction()
    {
        switch (currentStep)
        {
            case Step.Intro:

                instructionText.text =
                    "¡Hola! Soy Riku. Hoy vamos a aprender " +
                    "cómo controlar el recorrido del líquido.";

                break;


            case Step.OpenPinza:

                instructionText.text =
                    "Primero tenemos que abrir la pinza " +
                    "para permitir que el líquido pueda pasar.";

                break;


            case Step.ChoosePosition1:

                instructionText.text =
                    "Elegí la posición que permite comenzar el drenaje.";

                break;
            case Step.Position1Correct:

                instructionText.text =
                    "¡Muy bien! Esta posición permite comenzar el drenaje.";

                break;

            case Step.RedirectFluid1:

                instructionText.text =
                    "Seguí el recorrido del líquido hasta la bolsa de drenaje.";

                break;


            case Step.ChoosePosition2:

                instructionText.text =
                    "Ahora cambiemos el recorrido para realizar el lavado.";

                break;
            case Step.Position2Correct:

                instructionText.text =
                    "¡Exacto! Ahora podemos cambiar el recorrido para realizar el lavado.";

                break;

            case Step.RedirectFluid2:

                instructionText.text =
                    "Seguí el líquido hasta que llegue a la bolsa.";

                break;


            case Step.ChoosePosition3:

                instructionText.text =
                    "Ahora prepararemos la infusión. " +
                    "Elegí la posición correspondiente.";

                break;
            case Step.Position3Correct:

                instructionText.text =
                    "¡Muy bien! Esta es la posición que necesitamos para comenzar la infusión.";

                break;


            case Step.InsertPin:

                instructionText.text =
                    "Colocá el PIN para asegurar el sistema.";

                break;


            case Step.ChoosePosition4:

                instructionText.text =
                    "Último paso: llevá la perilla hasta la posición final.";

                break;
            case Step.Position4Correct:

                instructionText.text =
                    "¡Excelente! La perilla está en la posición correcta para cerrar el sistema.";

                break;

            case Step.Finished:

                instructionText.text =
                    "¡Completaste el procedimiento!";

                break;
        }
    }
    private void FinishLevel()
    {
        instructionText.text =
            "¡Completaste correctamente el procedimiento!";

        knob.DisableInteraction();

        congratsPanel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
