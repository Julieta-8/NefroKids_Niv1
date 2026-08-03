using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Nivel3 : MonoBehaviour
{
    // Preparado del ANDY
    //riku: se usan dos tipos de bolsas, una con el liquido que va a entrar y otra para la infusion
    //   -Elecci�n entre 3 tipos de bolsas, una limpia, sucia y con liquido(si elije mal re aparece el texto, una vez elejida la correcta, desaparecer las otras dos por medio de una animacion)
    // Desinfectar manos y esperar a que se sequen
    //   comprimir bolsa(la bolsa que se eligio antes!) para comprobar ausencia de fuga de liquido

    //     -limpiar el palo del Andy con alcohol y pa�uelos(se cambia de escenario al Andy, el usuario deber� colocar el alcohol, despues deber� pasar la pañuelo por unos segundos para limiar, mientras se limpia, el andy realiza una animacion donde de apoco cambia de srpite a uno limpio)

    public enum Step
    {
        Intro,

        ChooseBag,

        BagZoom,

        InspectBag,

        WaitingAnswer,

        BagVerified,

        Phase2Intro,

        DragAlcohol,

        AlcoholPlaced,

        DragTowel,

        TowelPlaced,

        Finished
    }

    [Header("================================")]
    [Header("FASE 1 - BOLSAS")]
    [Header("================================")]
    public Bag bag1;
    public Bag bag2;
    public Bag bag3;

    public Transform bagZoomPosition;


    public Sprite bagSimpleSprite;
    public Sprite bagDetailedSprite;
    private bool fechaChecked;
    private bool volumenChecked;
    private bool glucosaChecked;
    

    [Header("================================")]
    [Header("FASE 2 - ANDY")]
    [Header("================================")]

    public SpriteRenderer AndyRenderer;
    public Sprite[] wetAnimation;
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
    public GameObject phase3Objects;

    [Header("================================")]
    [Header("UI")]
    [Header("================================")]

    public TMP_Text instructionText;

    public Button nextButton;

    public GameObject dialoguePanel;

    public GameObject congratsPanel;

    [SerializeField] Button yesButton;

    [SerializeField] Button noButton;


    [Header("INSPECCIÓN")]

    [SerializeField] TMP_Text fechaText;

    [SerializeField] TMP_Text volumenText;

    [SerializeField] TMP_Text glucosaText;

    [SerializeField] GameObject inspectionPanel;

    [Header("RIKU")]
    public SpriteRenderer rikuRenderer;

    public Sprite rikuNeutral;
    public Sprite rikuCurious;

    private bool rikuNeutralState = true;



    [Header("ESCENA SIGUIENTE")]
    public string nextSceneName;




    //ESTADO
    public Step currentStep = Step.Intro;

    //FLAGS
    private bool BagChosen = false;
    private bool HeparinaPlaced = false;

    private bool AlcoholPlaced = false;
    private bool towelPlaced = false;


    //DRAG
    private GameObject draggingObject;
    private Vector3 draggingOffset;
    Bag selectedBag;

    void SelectBag(Bag bag)
    {
        if (currentStep != Step.ChooseBag)
            return;

        selectedBag = bag;

        StartCoroutine(ZoomBagRoutine());
       
    }
   
  public void StartLevel()
    {
        void GenerateBags()
        {
            int correct =
                Random.Range(0, 3);

            bag1.bagData =
                GenerateRandomBag(correct == 0);

            bag2.bagData =
                GenerateRandomBag(correct == 1);

            bag3.bagData =
                GenerateRandomBag(correct == 2);
        }

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

            case Step.BagVerified:

                StartPhase2();

                currentStep = Step.Phase2Intro;

                ShowDialogue();
                break;

            case Step.Phase2Intro:

                currentStep = Step.DragAlcohol;
                break;

            case Step.AlcoholPlaced:

                currentStep = Step.DragTowel;
                break;

            case Step.TowelPlaced:

                currentStep = Step.Finished;

                ShowDialogue();

               // StartCoroutine(FinishRoutine());

                break;
        }
    }

    void HandleMouseInput()
    {
        switch (currentStep)
        {
            case Step.ChooseBag:
                HandleBagSelection();
                break;

            case Step.DragAlcohol:
            case Step.DragTowel:
                HandleDragInput();
                break;
        }
    }
    void HandleBagSelection()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit =
            Physics2D.OverlapPoint(mouse);

        if (hit == null)
            return;

        Bag bag = hit.GetComponent<Bag>();

        if (bag != null)
        {
            SelectBag(bag);
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
    IEnumerator ZoomBagRoutine()
    {
        currentStep = Step.BagZoom;

        yield return StartCoroutine(AnimateBagZoom());

        ShowDetailedBag();

        currentStep = Step.InspectBag;

        ShowDialogue();
    }
    IEnumerator AnimateBagZoom()
    {
        Vector3 startPos = selectedBag.transform.position;
        Vector3 endPos = bagZoomPosition.position;

        Vector3 startScale = selectedBag.transform.localScale;
        Vector3 endScale = startScale * 2.5f;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime;

            selectedBag.transform.position =
                Vector3.Lerp(startPos, endPos, t);

            selectedBag.transform.localScale =
                Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }
    }
    void ShowDetailedBag()
    {
        selectedBag.SpriteRenderer.sprite =
        bagDetailedSprite;
        fechaChecked = false;
        volumenChecked = false;
        glucosaChecked = false;

        // actualizar textos

        fechaText.text =
            selectedBag.bagData.fecha;

        volumenText.text =
            selectedBag.bagData.volumen + " ml";

        glucosaText.text =
            selectedBag.bagData.glucosa + "%";

        inspectionPanel.SetActive(true);
    }
    void HandleDragging()
    {
        if (draggingObject == null)
            return;

        if (!Input.GetMouseButton(0))
            return;

        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouse.z = 0;

        draggingObject.transform.position =
            mouse + draggingOffset;
    }
    void BeginDrag(GameObject go)
    {
        draggingObject = go;

        Vector3 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouse.z = 0;

        draggingOffset =
            go.transform.position - mouse;
    }
    GameObject GetCurrentDraggable()
    {
        switch (currentStep)
        {
            case Step.DragAlcohol:

                return AlcoholObj;

            case Step.DragTowel:

                return towelObject;
        }

        return null;
    }
    Collider2D GetCurrentDropZone()
    {
        switch (currentStep)
        {
            case Step.DragAlcohol:

            case Step.DragTowel:

                return AndyDropZone;
        }

        return null;
    }
    void ReturnObject(GameObject go)
    {
        if (go == AlcoholObj)
        {
            go.transform.position =
                AlcoholStartPosition.position;
        }

        if (go == towelObject)
        {
            go.transform.position =
                towelStartPosition.position;
        }
    }
    IEnumerator FadeAlcohol()
    {
        Color c = AndyRenderer.color;

        while (c.a > 0)
        {
            c.a -= Time.deltaTime * 0.6f;

            AndyRenderer.color = c;

            yield return null;
        }

        AndyRenderer.color = Color.white;
    }
    void ShowWrongAnswerMessage()
    {
        instructionText.text =
            "Esa respuesta no es correcta. Revisá nuevamente la fecha, el volumen y la concentración de glucosa.";

        ShowDialogue();
    }
    void StartPhase2()
    {
        phase1Objects.SetActive(false);

        phase2Objects.SetActive(true);

        backgroundRenderer.sprite =
            AndyBackground;

        currentStep = Step.Phase2Intro;
    }
    void ResetInspection()
    {
        fechaChecked = false;

        volumenChecked = false;

        glucosaChecked = false;

        inspectionPanel.SetActive(false);
    }
    public void AnswerYes()
    {
        ValidateAnswer(true);
    }

    public void AnswerNo()
    {
        ValidateAnswer(false);
    }
    void ValidateAnswer(bool answer)
    {
        bool correct =
            selectedBag.bagData.IsCorrect();

        if (answer == correct)
        {
            currentStep = Step.BagVerified;

            ShowDialogue();
        }
        else
        {
            ShowWrongAnswerMessage();

            currentStep = Step.InspectBag;
        }
    }
    void HandleDragInput()
    {
        HandleBeginDrag();

        HandleDragging();

        HandleRelease();
    }
    void HandleBeginDrag()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector2 mouse =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Collider2D hit =
            Physics2D.OverlapPoint(mouse);

        if (hit == null)
            return;

        GameObject draggable =
            GetCurrentDraggable();

        if (hit.gameObject == draggable)
        {
            BeginDrag(draggable);
        }
    }
    void HandleRelease()
    {
        if (draggingObject == null)
            return;

        if (!Input.GetMouseButtonUp(0))
            return;

        TryDrop(draggingObject);

        draggingObject = null;
    }
    void TryDrop(GameObject go)
    {
        Collider2D zone =
            GetCurrentDropZone();

        if (zone == null)
            return;

        if (!zone.OverlapPoint(go.transform.position))
        {
            ReturnObject(go);
            return;
        }

        switch (currentStep)
        {
            case Step.DragAlcohol:

                StartCoroutine(PlaceAlcohol());

                break;

            case Step.DragTowel:

                StartCoroutine(PlaceTowel());

                break;
        }
    }
    BagData GenerateRandomBag(bool correct)
    {
        BagData bag = new BagData();

        if (correct)
        {
            // Bolsa completamente correcta
            bag.fecha = "10/2028";
            bag.volumen = 2000;
            bag.glucosa = 1.5f;
            bag.tieneFugas = false;
            bag.estaVencida = false;
        }
        else
        {
            // Primero generamos una bolsa correcta
            bag.fecha = "10/2028";
            bag.volumen = 2000;
            bag.glucosa = 1.5f;
            bag.tieneFugas = false;
            bag.estaVencida = false;

            // Después le agregamos UN error aleatorio
            int error = Random.Range(0, 4);

            switch (error)
            {
                case 0:
                    bag.estaVencida = true;
                    bag.fecha = "05/2023";
                    break;

                case 1:
                    bag.volumen = 1000;
                    break;

                case 2:
                    bag.glucosa = 2.5f;
                    break;

                case 3:
                    bag.tieneFugas = true;
                    break;
            }
        }

        return bag;
    }
    // ----------------------------------------FASE 1--------------------------------------


    void RevisarBolsa()
    {
        if (fechaChecked &&
           volumenChecked &&
           glucosaChecked)
        {
            currentStep = Step.WaitingAnswer;
            ShowDialogue();
        }
    }
    public void CheckFecha()
    {
        fechaChecked = true;

        RevisarBolsa();
    }
    public void CheckVolumen()
    {
        volumenChecked = true;

        RevisarBolsa();
    }
    public void CheckGlucosa()
    {
        glucosaChecked = true;

        RevisarBolsa();
    }
    //   ----------------------------------------FASE 2--------------------------------------
    IEnumerator WetAndyAnimation()
    {
        foreach (Sprite s in wetAnimation)
        {
            AndyRenderer.sprite = s;

            yield return new WaitForSeconds(0.15f);
        }
    }
    IEnumerator PlaceAlcohol()
    {
        AlcoholPlaced = true;

        AlcoholObj.SetActive(false);

        yield return StartCoroutine(WetAndyAnimation());

        currentStep = Step.AlcoholPlaced;

        ShowDialogue();
    }
    IEnumerator PlaceTowel()
    {
        towelPlaced = true;

        towelObject.SetActive(false);

        yield return StartCoroutine(FadeAlcohol());

        AndyRenderer.sprite = AndyLimpio;

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
            case Step.InspectBag:

                instructionText.text =
                "Revisá cuidadosamente la información de la bolsa.";

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
}
   /* IEnumerator FinishRoutine()
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