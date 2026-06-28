/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nivel3 : MonoBehaviour
{
          Preparado del ANDY
 -Elección entre 3 tipos de bolsas, una limpia, sucia y con liquido(si elije mal re aparece el texto, una vez elejida la correcta, desaparecer las otras dos por medio de una animacion)
 -Inyección de heparina(atravez de una animacion aparece la heparina, el usuario deberá arrastrar la a la bolsa elejida y una vez esté en el rango de la bolsa, conformaran un nuevo asset, la emzcla entre la bolsa y heparina juntas)
 -limpiar el palo del Andy con alcohol y pañuelos(se cambia de escenario al Andy, el usuario deberá colocar el alcohol, despues deberá pasar la totalla por unos segundos para limiar, mientras se limpia, el andy realiza una animacion donde de apoco cambia de srpite a uno limpio)
 -colocar las bolsas(desaparecen el alcohol y la toalla y ahora aparecen las dos bolsas que deben ser arrastradas al andy, estan denominadas bolsa 1 y 2 poer no es necesario respetar el orden al colocarlas)
 -retirar tapones(el usuario deberá retirar 2 tapones del any, sin orden definido)

 
public enum Step
    {
        Intro,
        ChooseBag,
        BagChosen,
        //Animacion
        DragHeparina,
        HeprinaPlaced,
        //Animacion
        Intro2,
        DragAlcohol,
        AlcoholPlaced,
        //Animacion que desvanezca la suciedad
        DragTowel,
        TowelPlaced,
        //Aparicion de bolsas
        DragBag1,
        Bag1Placed,
        DragBag2,
        Bag2Placed,
        TakeLit1,
        TakeLit2,
        Finished
    }
    [Header("================================")]
    [Header("FASE 0 - RANDOM")]
    [Header("================================")]
    public System.Action OnLevelCompleted;
    public bool IsCompleted { get; private set; }

    public float Phase1Time => phase1Timer;
    public float Phase2Time => phase2Timer;
    public float AverageTime => averageTime;

    public int Phase1Stars => phase1Stars;
    public int Phase2Stars => phase2Stars;

    private float phase1Timer;
    private float phase2Timer;
    private float averageTime;

    private bool timingPhase1;
    private bool timingPhase2;

    private int phase1Stars;
    private int phase2Stars;
    [Header("================================")]
    [Header("FASE 1 - BOLSAS")]
    [Header("================================")]

    public GameObject Bag1;
    public GameObject Bag3;
    public GameObject Bag2;

    public Transform Bag1StartPosition;
    public Transform Bag2StartPosition;
    public Transform Bag3StartPosition;


    [Header("================================")]
    [Header("FASE 2 - ANDY")]
    [Header("================================")]

    public SpriteRenderer AndyRenderer;

    public Sprite AndyLimpio;
    public Sprite AndySucio;


    public GameObject AlcoholObj;
    public GameObject towelObject;

    public GameObject Bolsa1Obj;
    public GameObject Bolsa2Obj;

    public GameObject Tapa1Obj;
    public GameObject Tapa2Obj;


    public Transform AlcoholStartPosition;
    public Transform towelStartPosition;

    public Collider2D AndyDropZone;

    [Header("================================")]
    [Header("FONDOS")]
    [Header("================================")]

    public SpriteRenderer backgroundRenderer;

    public Sprite roomBackground;

    [Header("================================")]
    [Header("GRUPOS")]
    [Header("================================")]

    public GameObject phase1Objects;
    public GameObject phase2Objects;

    [Header("================================")]
    [Header("UI")]
    [Header("================================")]

    public TMP_Text instructionText;

    public Button nextButton;

    public GameObject dialoguePanel;

    public GameObject congratsPanel;


    public TMP_Text DatoCuriosoText;


    public GameObject DatoCuriosoPanel;


    [Header("RIKU")]
    public SpriteRenderer rikuRenderer;

    public Sprite rikuNeutral;
    public Sprite rikuCurious;

    private bool rikuNeutralState = true;

    public Sprite CabezarikuCurious;


    [Header("ESCENA SIGUIENTE")]
    public string nextSceneName;




    // ESTADO
    public Step currentStep = Step.Intro;

    // FLAGS
    private bool BagChosen = false;
    private bool HeparinaPlaced = false;

    private bool AlcoholPlaced = false;
    private bool towelPlaced = false;
    private bool Bag1Placed = false;
    private bool Bag2Placed = false;

    // DRAG
    private GameObject draggingObject;
    private Vector3 draggingOffset;

    void Start()
    {

        phase1Objects.SetActive(true);
        phase2Objects.SetActive(false);

        // UI
        congratsPanel?.SetActive(false);

        currentStep = Step.Intro;

        ShowDialogue();

        // BOTON
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextPressed);
        }


    }

    // Update is called once per frame
    void Update()
    {

        HandleMouseInput();

        if (timingPhase1)
            phase1Timer += Time.deltaTime;

        if (timingPhase2)
            phase2Timer += Time.deltaTime;

    }
    void OnNextPressed()
    {
        ToggleRikuExpression();

        HideDialogue();

        switch (currentStep)
        {
            case Step.Intro:

                currentStep = Step.ChooseBag;

                break;

            //case Step.StartTimer


            case Step.BagChosen:
                phase1Timer = 0;
                timingPhase1 = true;

                currentStep = Step.DragHeparina;

                break;

            case Step.HeprinaPlaced:


                //-----------------------ETAPA 2 INICIO---------------------------


                StartPhase2();

                currentStep = Step.Phase2Intro;

                ShowDialogue();

                break;

            case Step.Phase2Intro:
                phase2Timer = 0;
                timingPhase2 = true;
                currentStep = Step.DragAlcohol;

                break;

            case Step.AlcoholPlaced:

                currentStep = Step.DragTowel;

                break;

            case Step.TowelPlaced:

                currentStep = Step.DragBag1;

                break;
            case Step.Bag1Placed:

                currentStep = Step.DragBag2;

                break;

            case Step.TakeLit1:

                currentStep = Step.TakeLit2;

                break;

            case Step.TakeLit2:

                currentStep = Step.Finished;

                ShowDialogue();

                StartCoroutine(FinishRoutine());

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


        timingPhase1 = false;
        timingPhase2 = false;

    }
    void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        instructionText.gameObject.SetActive(false);
        rikuRenderer.enabled = false;
        nextButton.gameObject.SetActive(false);

    }

        //------------------------------------------------------------------------------------

        void UpdateInstruction()
    {
        if (instructionText == null) return;

        switch (currentStep)
        {
            case Step.Intro:

                instructionText.text =
                    "Ahora vamos a ver como colocar todo para por fin relizar la dialisis";

                break;



            case Step.ChooseBag:

                instructionText.text =
                    "Elegí la bolsa correcta";
                HideDialogue();
                break;

            case Step.BagChosen:

                instructionText.text =
                    "¡Muy bien! Ahora continuemos.";

                break;

            case Step.DragHeparina:

                instructionText.text =
                    "Ahora coloca la heparina.";
                HideDialogue();
                break;

            case Step.HeprinaPlaced:

                instructionText.text =
                    "¡Excelente! Ahora vamos al ANDY.";

                break;

            case Step.Phase2Intro:

                instructionText.text =
                    "Debemos higienizar el palo.";

                break;

            case Step.DragAlcohol:

                instructionText.text =
                    "Primero coloca el alcohol.";
                HideDialogue();
                break;

            case Step.AlcoholPlaced:

                instructionText.text =
                    "¡Perfecto! Ahora pasa la toalla.";

                break;

            case Step.TowelPlaced:

                instructionText.text =
                    "Coloquemos las bolsas";

                break;
                /////////////

            case Step.DragBag1:

                instructionText.text =
                    "LLeva la bolsa al andy";

                break;
            case Step.DragBag2:

                instructionText.text =
                    "¡Muy bien! Ahora la segunda.";
                HideDialogue();
                break;

                ////////////////
            case Step.TakeLit1|;

                instructionText.text =
                    "Saca uno de los tapones";
                HideDialogue();
                break;

            case Step.TakeLit2:

                instructionText.text =
                    "¡Excelente trabajo! Terminaste correctamente.";

                break;








            case Step.Finished:

                instructionText.text =
                    "¡Completado!";

                break;
        }
    }

    IEnumerator FinishRoutine()
    {
        IsCompleted = true;
        OnLevelCompleted?.Invoke();
        congratsPanel?.SetActive(true);

        yield return new WaitForSeconds(2f);
        
        instructionText.text =
       "Tiempo fase 1: " + phase1Timer.ToString("F1") + "s\n" +
      "Tiempo fase 2: " + phase2Timer.ToString("F1") + "s\n" +
       "Promedio: " + averageTime.ToString("F1") + "s";
        ResultData result = new ResultData
        {
            levelId = 1,
            completed = true,

            phase1Time = phase1Timer,
            phase2Time = phase2Timer,
            averageTime = averageTime,

            phase1Stars = Phase1Stars,
            phase2Stars = Phase2Stars
        };
        React_Connection bridge = FindObjectOfType<React_Connection>();
        if (bridge != null)
        {
            bridge.SendResult(result);
        }
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }









}
*/