/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Nivel3 : MonoBehaviour
{
    /* Preparado del ANDY
 riku: se usan dos tipos de bolsas, una con el liquido que va a entrar y otra para la infusion
     -Elecci�n entre 3 tipos de bolsas, una limpia, sucia y con liquido(si elije mal re aparece el texto, una vez elejida la correcta, desaparecer las otras dos por medio de una animacion)
     Desinfectar manos y esperar a que se sequen
     comprimir bolsa(la bolsa que se eligio antes!) para comprobar ausencia de fuga de liquido
     -Inyecci�n de heparina(atravez de una animacion aparece la heparina, el usuario deber� arrastrar la a la bolsa elejida y una vez est� en el rango de la bolsa, conformaran un nuevo asset, la emzcla entre la bolsa y heparina juntas)

     -limpiar el palo del Andy con alcohol y pa�uelos(se cambia de escenario al Andy, el usuario deber� colocar el alcohol, despues deber� pasar la pañuelo por unos segundos para limiar, mientras se limpia, el andy realiza una animacion donde de apoco cambia de srpite a uno limpio)


     PROXIME NIVEL 4
     -colocar las bolsas(desaparecen el alcohol y la toalla y ahora aparecen las dos bolsas que deben ser arrastradas al andy, estan denominadas bolsa 1 y 2 poer no es necesario respetar el orden al colocarlas)

     desplegar el cable de la bolsa de drenaje(linea de drenaje) y conectarlo al disco a la izquierda
     conectar el cable de la bolsa de infusion
     conectar el tapon desinfectante al lado izquierdo del disco


     proximo proximo 5
     conexion con el paciente!
     cateter conectado al disco

     -retirar tapones(el usuario deber� retirar 2 tapones del any, sin orden definido)

     CHEQUEAR SI SE USA HEPARINAo hay otras opciones*/
 /*   public enum Step
    {
        Intro,
        ChooseBag,
        BagChosen,

        DragHands, //ACA SE COMPRIME LA BOLSA
        HandsDraged,// aparece una
        
        DragHeparina,
        HeprinaPlaced,
        


        DragAlcohol,
        AlcoholPlaced,
        
        DragTowel,
        TowelPlaced,
        Finished
    }

    [Header("================================")]
    [Header("FASE 1 - BOLSAS")]
    [Header("================================")]

    public GameObject Bag1;
    public GameObject Bag3;
    public GameObject Bag2;

    public Transform Bag1StartPosition;
    public Transform Bag2StartPosition;
    public Transform Bag3StartPosition;


    public GameObject HeparinaObj;

    public SpriteRenderer BolsaRenderer;
    public Sprite BolsaSola;
    public Sprite BolsaConHeparina;

    public SpriteRenderer Manos;
    public Sprite ManosP1;
    public Sprite ManosP2;


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



    public Transform AlcoholStartPosition;
    public Transform towelStartPosition;

    public Collider2D AndyDropZone;

    [Header("================================")]
    [Header("FONDOS")]
    [Header("================================")]

    public SpriteRenderer backgroundRenderer;
    public Sprite Mesa;
    public Sprite roomBackground;
    public Sprite AndyBackground;


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




    //ESTADO
    public Step currentStep = Step.Intro;

    //FLAGS
    private bool BagChosen = false;
    private bool HeparinaPlaced = false;

    private bool AlcoholPlaced = false;
    private bool towelPlaced = false;
    private bool Bag1Placed = false;
    private bool Bag2Placed = false;

    //DRAG
    private GameObject draggingObject;
    private Vector3 draggingOffset;

    void Start()
    {

        phase1Objects.SetActive(true);
        phase2Objects.SetActive(false);

        
       congratsPanel?.SetActive(false);

        currentStep = Step.Intro;

        ShowDialogue();

        
        if (nextButton != null)
        {
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextPressed);
        }


    }

   // Update is called once per frame;
    void Update()
    {

        HandleMouseInput();

       

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


            case Step.BagChosen:

                currentStep = Step.DragHands;

                break;

            case Step.HandsDraged:

            case Step.HandsDraged:

                currentStep = Step.DragHeparina;

                break;

            case Step.HeprinaPlaced:


              //  -----------------------ETAPA 2 INICIO-------------------------- -


                StartPhase2();

                currentStep = Step.DragAlcohol;

                ShowDialogue();

                break;

     

            case Step.AlcoholPlaced:

                currentStep = Step.DragTowel;

                break;

            case Step.TowelPlaced:



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



    }
    void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        instructionText.gameObject.SetActive(false);
        rikuRenderer.enabled = false;
        nextButton.gameObject.SetActive(false);

    }
   // ----------------------------------------FASE 1--------------------------------------
    void PlaceHeparina()
    {
        if (HeparinaPlaced) return;

        HeparinaPlaced = true;

        HeprinaObj.SetActive(false);

        BolsaRenderer.sprite =
            BolsaConHeparina;

        currentStep = Step.HeprinaPlaced;

        ShowDialogue();
    }

 //   ----------------------------------------FASE 2--------------------------------------
    void PlaceAlcohol()
    {
        if (AlcoholPlaced) return;

        AlcoholPlaced = true;

        AlcoholObj.SetActive(false);

        AndyRenderer.sprite =
            AndySucio;

        currentStep = Step.AlcoholPlaced;

        ShowDialogue();
    }
    void PlaceTowel()
    {
        if (towelPlaced) return;

        towelPlaced = true;

        towelObject.SetActive(false);

        AndyRenderer.sprite =
            AndyLimpio;

        currentStep = Step.TowelPlaced;

        ShowDialogue();
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
                    "Eleg� la bolsa correcta";
                HideDialogue();
                break;

            case Step.BagChosen:

                instructionText.text =
                    "�Muy bien! Ahora continuemos.";

                break;
            case Step.DragHands:
                instructionText.text =
                    "Arrastra las manos para detectar si hya agujeros.";
                HideDialogue();
                break;
            case Step.HandsDraged:
                "Perfecto";
                HideDialogue();
                break;

            case Step.DragHeparina:

                instructionText.text =
                    "Ahora coloca la heparina.";
                HideDialogue();
                break;

            case Step.HeprinaPlaced:

                instructionText.text =
                    "�Excelente! Ahora vamos al ANDY.";

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
                    "�Perfecto! Ahora pasa la toalla.";

                break;

            case Step.TowelPlaced:

                instructionText.text =
                    "Coloquemos las bolsas";

                break;
            ///////////



            case Step.Finished:

                instructionText.text =
                    "�Completado!";

                break;
        }
    }

    IEnumerator FinishRoutine()
    {
        IsCompleted = true;
        OnLevelCompleted?.Invoke();
        congratsPanel?.SetActive(true);



        ResultData result = new ResultData
        {
            levelId = 3,
         
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









}*/