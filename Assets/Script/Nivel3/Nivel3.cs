//Ocultar botones de SI NO --> check
//HACER QUE v, f y g vayan apareciendo de apco
//explicar como funciona el nivel de la glucosa fecha y volumen
//Que termian una vez que el jugador elige una opcion correcta
//Agregar animación de andy mojado a andy limpio en vez de hacer que se desvanezca

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
        // TUTORIAL
        ExplainGlucose,
        ExplainExpiration,
        ExplainVolume,

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
    void GenerateBags()
    {
        int correct =
            UnityEngine.Random.Range(0, 3);

        bag1.bagData =
            GenerateRandomBag(correct == 0);

        bag2.bagData =
            GenerateRandomBag(correct == 1);

        bag3.bagData =
            GenerateRandomBag(correct == 2);
    }
    public void StartLevel()
    {
        GenerateBags();

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        phase1Objects.SetActive(true);
        phase2Objects.SetActive(false);

        fechaText.gameObject.SetActive(false);

         volumenText.gameObject.SetActive(false);

         glucosaText.gameObject.SetActive(false);

        congratsPanel?.SetActive(false);

        currentStep = Step.Intro;


        yesButton.onClick.RemoveAllListeners();
        yesButton.onClick.AddListener(AnswerYes);

        noButton.onClick.RemoveAllListeners();
        noButton.onClick.AddListener(AnswerNo);



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

                currentStep = Step.ExplainGlucose;
                ShowDialogue();

                break;

            case Step.ExplainGlucose:

                currentStep = Step.ExplainExpiration;
                ShowDialogue();

                break;

            case Step.ExplainExpiration:

                currentStep = Step.ExplainVolume;
                ShowDialogue();

                break;

            case Step.ExplainVolume:

                currentStep = Step.ChooseBag;
                HideDialogue();

                break;
            case Step.ChooseBag:

                currentStep = Step.InspectBag;
                break;
            case Step.InspectBag:

                currentStep = Step.WaitingAnswer;
                break;
            case Step.WaitingAnswer:

                currentStep = Step.BagVerified;
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
        fechaText.gameObject.SetActive(true);

        volumenText.gameObject.SetActive(true);

        glucosaText.gameObject.SetActive(true);

        phase2Objects.SetActive(false);

        phase3Objects.SetActive(true);
        currentStep = Step.BagZoom;

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);
        yield return StartCoroutine(AnimateBagZoom());

        ShowDetailedBag();
        nextButton.gameObject.SetActive(false);
        yield return StartCoroutine(ShowInspectionTexts());

        yesButton.gameObject.SetActive(true);
        noButton.gameObject.SetActive(true);

        inspectionPanel.SetActive(true);


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
    IEnumerator ShowInspectionTexts()
    {
        fechaText.gameObject.SetActive(true);
        volumenText.gameObject.SetActive(true);
        glucosaText.gameObject.SetActive(true);

        SetTextAlpha(fechaText, 0f);
        SetTextAlpha(volumenText, 0f);
        SetTextAlpha(glucosaText, 0f);

        yield return StartCoroutine(FadeInText(fechaText, 0.5f));

        yield return new WaitForSeconds(0.15f);

        yield return StartCoroutine(FadeInText(volumenText, 0.5f));

        yield return new WaitForSeconds(0.15f);

        yield return StartCoroutine(FadeInText(glucosaText, 0.5f));
    }
    IEnumerator FadeInText(TMP_Text text, float duration)
    {
        Color color = text.color;

        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float alpha = Mathf.Lerp(0f, 1f, time / duration);

            color.a = alpha;
            text.color = color;

            yield return null;
        }

        color.a = 1f;
        text.color = color;
    }
    void SetTextAlpha(TMP_Text text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
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
        phase3Objects.SetActive(false);

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
        bool correct = selectedBag.bagData.IsCorrect();

        // =========================================
        // BOLSA CORRECTA
        // =========================================

        if (correct)
        {
            if (answer == true)
            {
                // Correcta + respondió SÍ
                currentStep = Step.BagVerified;

                yesButton.gameObject.SetActive(false);
                noButton.gameObject.SetActive(false);

                ShowDialogue();

                return;
            }

            // Correcta + respondió NO
            ShowWrongAnswerMessage();

            return;
        }

        // =========================================
        // BOLSA INCORRECTA
        // =========================================

        if (answer == false)
        {
            // Incorrecta + respondió NO
            // ¡Esta es la respuesta correcta!
            StartCoroutine(ReturnToBagSelection());

            return;
        }

        // Incorrecta + respondió SÍ
        ShowWrongAnswerMessage();
    }
    IEnumerator ReturnToBagSelection()
    {
        // -----------------------------------
        // 1. Desactivar botones de respuesta
        // -----------------------------------

        yesButton.gameObject.SetActive(false);
        noButton.gameObject.SetActive(false);

        // Ocultar panel de inspección
        inspectionPanel.SetActive(false);

        // -----------------------------------
        // 2. Guardar posición actual
        // -----------------------------------

        Vector3 startPosition =
            selectedBag.transform.position;

        Vector3 startScale =
            selectedBag.transform.localScale;

        // Posición y escala originales
        Vector3 targetPosition =
            selectedBag.originalPosition;

        Vector3 targetScale =
            selectedBag.originalScale;

        // -----------------------------------
        // 3. Animar regreso
        // -----------------------------------

        float duration = 0.7f;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;

            float t = time / duration;

            selectedBag.transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            selectedBag.transform.localScale =
                Vector3.Lerp(
                    startScale,
                    targetScale,
                    t
                );

            yield return null;
        }

        // -----------------------------------
        // 4. Asegurar posición final
        // -----------------------------------

        selectedBag.transform.position =
            targetPosition;

        selectedBag.transform.localScale =
            targetScale;

        // -----------------------------------
        // 5. Volver al sprite normal
        // -----------------------------------

        selectedBag.SpriteRenderer.sprite =
            bagSimpleSprite;

        // -----------------------------------
        // 6. Ocultar datos de inspección
        // -----------------------------------

        fechaText.gameObject.SetActive(false);
        volumenText.gameObject.SetActive(false);
        glucosaText.gameObject.SetActive(false);

        // -----------------------------------
        // 7. Cambiar de fase
        // -----------------------------------

        phase2Objects.SetActive(false);
        phase3Objects.SetActive(false);

        phase1Objects.SetActive(true);

        // -----------------------------------
        // 8. Permitir seleccionar otra bolsa
        // -----------------------------------

        selectedBag = null;

        currentStep = Step.ChooseBag;

        UpdateInstruction();
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
            int error = UnityEngine.Random.Range(0, 4);

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
       

            case Step.ExplainGlucose:

                instructionText.text =
                    "La glucosa es un tipo de azúcar que puede estar dentro de la bolsa.\r\nEn esta misión tenemos que mirar cuánto hay y comprobar que tenga el valor indicado. 1,5 %\r\n\r\n👀 Hay que mirar este número";

                break;

            case Step.ExplainExpiration:

                instructionText.text = "Es la fecha que nos dice hasta cuándo podemos usar la bolsa.\r\nSi la fecha ya pasó, la bolsa está vencida y no debemos elegirla.\r\n10/2028\r\n\r\n✅ Todavía sirve";

                break;

            case Step.ExplainVolume:

                instructionText.text = "El volumen nos dice cuánto líquido hay dentro de la bolsa.\r\nTenemos que comprobar que tenga la cantidad indicada.2000 ml\r\n\r\n✅ Cantidad correcta";

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