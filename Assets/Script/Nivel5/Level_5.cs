using System.Collections;
using System.Collections.Generic;
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
       // OpenPinza,

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
   // public GameObject phase1Objects;
    //public GameObject phase2Objects;

    /// <summary>/////////////////////////
    /// UI
    /// </summary>
    public GameObject Backgorund;
    public GameObject Phase1;
    public GameObject Phase2;
    public GameObject Phase3;

    public TMP_Text instructionText;
    public TMP_Text postionText;

    public Button nextButton;

    public GameObject dialoguePanel;

    public GameObject congratsPanel;

    public Button rightArrow;
    public Button leftArrow;
    public Button  verifyButton;

    public Sprite DialisisInfusion;
    public Sprite DialisisDrenaje;

    public Sprite AndyS;
    public SpriteRenderer Dialisis;




    public GameObject Andy;

    [Header("RIKU")]
    public SpriteRenderer rikuRenderer;

    public Sprite rikuNeutral;
    public Sprite rikuCurious;

    private bool rikuNeutralState = true;


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
    //MANEQUINDROP ZONE

    public Collider2D Bolsa1Drop;
    public Collider2D Bolsa2Drop;

    public GameObject Liquid1Obj;
    public GameObject Liquid2Obj;
    public Transform Liquid1Position;
    public Transform Liquid2Position;

    public GameObject pinObject;
    public GameObject pinzaObject;



   

    public Collider2D ChicoDropZone;

    private bool Position1Placed = false;
    private bool Position2Placed = false;
    private bool BagCompleted = false;
    private bool Position3Placed = false;
    private bool Position4Placed = false;
    private bool FluidRedirected = false;
    private void SetupInitialState()
    {
        // phase1Objects.SetActive(true);
        //phase2Objects.SetActive(false);
        Backgorund.SetActive(true);
       

        congratsPanel.SetActive(false);

        nextButton.gameObject.SetActive(false);

        currentStep = Step.Intro;
    }

    private void Start()
    {

        knob.OnPositionReached += OnKnobPositionReached;

        nextButton.onClick.AddListener(NextButton);
        verifyButton.onClick.AddListener(VerifyPosition);

        rightArrow.onClick.AddListener(knob.RotateRight);
        leftArrow.onClick.AddListener(knob.RotateLeft);

        ChangeStep(Step.Intro);

    }
    void Update()
    {
        HandleMouseInput();
    }
    public void ChangeStep(Step newStep)
    {
        currentStep = newStep;

        UpdateInstruction();

        SetObjectsForStep();//PODRIA RECIBIRSE DESDE ACA
    }

    private void SetObjectsForStep()
    {

        Phase1.SetActive(true);
        Phase2.SetActive(false);
        Phase3.SetActive(false);

        Liquid1Obj.SetActive(false);
        Liquid2Obj.SetActive(false);
        /*
        verifyButton.gameObject.SetActive(false);
        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
        */
        switch (currentStep)
        {
            /*case Step.OpenPinza:

                pinzaObject.SetActive(true);

                break;
           

            case Step.ChoosePosition1:

                verifyButton.gameObject.SetActive(true);
                leftArrow.gameObject.SetActive(true);
                rightArrow.gameObject.SetActive(true);
                Dialisis.sprite = DialisisDrenaje;

                break;
             */
            case Step.Intro:

 

                break;
            case Step.RedirectFluid1:
                Phase2.SetActive(true);
                Phase1.SetActive(false);
                Dialisis.sprite = DialisisDrenaje;

                break;
            case Step.Infusion:

                Phase2.SetActive(true);
                Phase3.SetActive(true);
                Phase1.SetActive(false); 
                Liquid2Obj.SetActive(true);
                break;


            case Step.RedirectFluid2:
                Dialisis.enabled = true;
                Phase2.SetActive(true);
                Phase3.SetActive(true);
                Phase1.SetActive(false);

                Liquid2Obj.SetActive(true);
                Dialisis.sprite = DialisisInfusion;

                break;

            case Step.InsertPin:

                pinObject.SetActive(true);

                break;


            case Step.ChoosePosition1:
                Phase1.SetActive(true);
                StartChoosePosition1();
                break;

            case Step.ChoosePosition2:
                Phase1.SetActive(true);
                Phase2.SetActive(false);
                Phase3.SetActive(false);
                StartChoosePosition2();
                break;

            case Step.Position1Correct:
            case Step.Position2Correct:
            case Step.ChoosePosition3:
            case Step.Position3Correct:
            case Step.ChoosePosition4:
            case Step.Position4Correct:
                Phase2.SetActive(false);
                Phase3.SetActive(false);
                Phase1.SetActive(true);
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
    /*private void StartOpenPinza()
    {
        currentStep = Step.OpenPinza;

        knob.DisableInteraction();

        verifyButton.gameObject.SetActive(false);

        instructionText.text = "Abrí la pinza para comenzar.";
    }*/
    public void NextButton()
    {
        switch (currentStep)
        {
            case Step.Intro:

                HideDialogue();

                ChangeStep(Step.ChoosePosition1);
                //StartOpenPinza();

                break;


            case Step.Position1Correct:

                HideDialogue();

                ContinueFromPosition1();

                break;


            case Step.ChoosePosition2:

                HideDialogue();

                StartChoosePosition2();

                break;


            case Step.Position2Correct:

                HideDialogue();

                ContinueFromPosition2();

                break;


            case Step.Position3Correct:

                HideDialogue();

                ContinueFromPosition3();

                break;


            case Step.Position4Correct:

                HideDialogue();

                ContinueFromPosition4();

                break;
            case Step.ChoosePosition1:
           // case Step.ChoosePosition2:
            case Step.ChoosePosition3:
            case Step.ChoosePosition4:

                HideDialogue();

                switch (currentStep)
                {
                    /*case Step.ChoosePosition1:
                        StartChoosePosition1();
                        break;*/

                    case Step.ChoosePosition2:
                        StartChoosePosition2();
                        break;

                    case Step.ChoosePosition3:
                        StartChoosePosition3();
                        break;

                    case Step.ChoosePosition4:
                        StartChoosePosition4();
                        break;
                }

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

       //OJO instructionText.text =    "Ahora colocá el PIN para asegurar el sistema.";
    }
   
    private void StartChoosePosition1()
    {
        postionText.text =
      "Ahora tenemos que elegir la posición correcta para comenzar el drenaje.";

        nextButton.gameObject.SetActive(false);
        Phase1.gameObject.SetActive(true);
        verifyButton.gameObject.SetActive(true);
        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);

        knob.EnableInteraction();

        targetPosition = Knob.KnobPosition.Posicion1;
    }
    private void StartChoosePosition4()
    {
        currentStep = Step.ChoosePosition4;

        targetPosition = Knob.KnobPosition.Posicion4;

        verifyButton.gameObject.SetActive(true);

        leftArrow.gameObject.SetActive(true);
        rightArrow.gameObject.SetActive(true);

        knob.EnableInteraction();
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

       // knob.DisableInteraction();

        currentStep = Step.Position1Correct;

        ShowDialogue();
    }
    private void ContinueFromPosition1()
    {
        currentStep = Step.RedirectFluid1;
        ChangeStep(Step.RedirectFluid1);

        Liquid1Obj.SetActive(true);

       //OJO instructionText.text =        "Ahora seguí el recorrido del líquido hasta la bolsa de drenaje.";
    }
    private void CorrectPosition2()
    {
        verifyButton.gameObject.SetActive(false);

        leftArrow.gameObject.SetActive(false);
        rightArrow.gameObject.SetActive(false);
       // knob.DisableInteraction();

        currentStep = Step.Position2Correct;

        ShowDialogue(
           
        );
    }
    private void ContinueFromPosition2()
    {
        currentStep = Step.RedirectFluid2;
        ChangeStep(Step.RedirectFluid2);

        Liquid2Obj.SetActive(true);

        postionText.text =
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
        ChangeStep(Step.Infusion);

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
    {/*
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
        }*/
    currentPosition = position;
        Debug.Log("Level_5 recibió posición: " + position);
    }
    public void VerifyPosition()
    {
        switch (currentStep)
        {
            case Step.ChoosePosition1:

                if (currentPosition == Knob.KnobPosition.Posicion1)
                {
                    CorrectPosition1();
                }
                else
                {
                    WrongPosition();
                }

                break;
            case Step.ChoosePosition2:

                if (currentPosition == Knob.KnobPosition.Posicion2)
                {
                    CorrectPosition2();
                }
                else
                {
                    WrongPosition();
                }
                break;


            case Step.ChoosePosition3:

                if (currentPosition == Knob.KnobPosition.Posicion3)
                {
                    CorrectPosition3();
                }
                else
                {
                    WrongPosition();
                }
                break;


            case Step.ChoosePosition4:

                if (currentPosition == Knob.KnobPosition.Posicion4)
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
                postionText.text = "Esa no es la posición que necesitamos para realizar el lavado. Probá nuevamente.";
                ShowDialogue( );

                break;


            case Step.ChoosePosition3:
                postionText.text = "Todavía no. Buscá la posición que permite comenzar la infusión.";

                ShowDialogue( );

                break;


            case Step.ChoosePosition4:
                postionText.text = "Todavía no. Necesitamos llevar la perilla hasta la posición final para cerrar el sistema.";
                ShowDialogue( );

                break;
        }
    }
    private void StartInfusion()
    {
        currentStep = Step.Infusion;

       //OJO instructionText.text =   "Ahora seguí el recorrido del líquido hasta que termine la infusión.";

        //knob.DisableInteraction();

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

            Debug.Log("CLICK - Posición mundo: " + worldPoint);

            Collider2D hit =
                Physics2D.OverlapPoint(worldPoint);

            if (hit == null)
            {
                Debug.Log("CLICK - No detectó ningún Collider");
                return;
            }

            Debug.Log("CLICK - Detectó: " + hit.gameObject.name);

            GameObject clickedObject = hit.gameObject;

            Debug.Log("STEP ACTUAL: " + currentStep);

            if (currentStep == Step.RedirectFluid1 &&
                clickedObject == Liquid1Obj)
            {
                Debug.Log("LIQUID1 DETECTADO - Comenzando Drag");

                BeginDrag(clickedObject);
                return;
            }

            Debug.Log("Detectó un objeto, pero NO es Liquid1Obj");
        }

        if (draggingObject != null &&
            Input.GetMouseButton(0))
        {
            Debug.Log("ARRASTRANDO: " + draggingObject.name);

            Vector3 mouseWorld =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            mouseWorld.z = 0f;

            draggingObject.transform.position =
                mouseWorld + draggingOffset;
        }

        if (draggingObject != null &&
            Input.GetMouseButtonUp(0))
        {
            Debug.Log("SOLTANDO: " + draggingObject.name);

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

// OJO       instructionText.text =          "Seguí el líquido hasta la bolsa de drenaje.";
    }
    private void StartFluid2()
    {
        Liquid2Obj.SetActive(true);

      //OJO  instructionText.text =     "Seguí el líquido hasta el paciente.";
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
                    Bolsa1Drop.OverlapPoint(go.transform.position);
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
        /*if (go == Liquid2Obj &&
            currentStep == Step.Infusion)
        {

        CompleteInfusion(); return;
        }*/

        if (go == Liquid2Obj &&
                currentStep == Step.RedirectFluid2)
        {

            CompleteInfusion(); return;
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

        ChangeStep(Step.ChoosePosition2);

        currentStep = Step.ChoosePosition2;

        ShowDialogue();
    }

      /*  void PlaceLiquid2()
        {
            if (BagCompleted) return;

        BagCompleted = true;

            Liquid2Obj.SetActive(false);
        Liquid1Obj.SetActive(false);

        //SegundaBolsa.sprite = Bolsa2ConL;


        currentStep = Step.ChoosePosition3;

        StartChoosePosition3();
       
            ShowDialogue();
        }*/
    private void UpdateInstruction()
    {
        switch (currentStep)
        {
            case Step.Intro:

                instructionText.text =
                    "¡Hola! Soy Riku. Hoy vamos a aprender " +
                    "cómo controlar el recorrido del líquido.";

                break;
                /*

            case Step.OpenPinza:

                instructionText.text =
                    "Primero tenemos que abrir la pinza " +
                    "para permitir que el líquido pueda pasar.";

                break;
                */

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

}