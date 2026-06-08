using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Nivel_1 : MonoBehaviour
{
    public enum Step
    {
        Intro,

        DragVest,
        VestPlaced,

        DragMask,
        MaskPlaced,

        Phase2Intro,

        DragSoap,
        SoapPlaced,

        TurnOnWater,
        WaterRunning,

        TurnOffWater,

        DragTowel,
        TowelPlaced,

        Finished
    }

    [Header("================================")]
    [Header("FASE 1 - MANIQUI")]
    [Header("================================")]

    public SpriteRenderer mannequinRenderer;

    public Sprite mannequinNormal;
    public Sprite mannequinWithVest;
    public Sprite mannequinComplete;

    public GameObject vestObject;
    public GameObject maskObject;

    public Transform vestStartPosition;
    public Transform maskStartPosition;

    public Collider2D mannequinDropZone;

    [Header("================================")]
    [Header("FASE 2 - LAVADO")]
    [Header("================================")]

    public SpriteRenderer handsRenderer;

    public Sprite handsNormal;
    public Sprite handsWithSoap;
    public Sprite handsWashing;
    public Sprite handsWet;
    public Sprite handsClean;

    public GameObject soapObject;
    public GameObject towelObject;
    public SpriteRenderer faucetRenderer;

    public Sprite faucetOff;
    public Sprite faucetOn;

    public Transform soapStartPosition;
    public Transform towelStartPosition;

    public Collider2D handsDropZone;

    [Header("================================")]
    [Header("FONDOS")]
    [Header("================================")]

    public SpriteRenderer backgroundRenderer;
    public SpriteRenderer backgroundRenderer_2;

    public Sprite roomBackground;
    public Sprite sinkBackground;

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

    [Header("RIKU")]
    public SpriteRenderer rikuRenderer;

    public Sprite rikuNeutral;
    public Sprite rikuCurious;

    private bool rikuNeutralState = true;

    [Header("ESCENA SIGUIENTE")]
    public string nextSceneName;

    // ESTADO
    public Step currentStep = Step.Intro;

    // FLAGS
    private bool vestPlaced = false;
    private bool maskPlaced = false;

    private bool soapPlaced = false;
    private bool towelPlaced = false;
    private bool waterOpened = false;

    // DRAG
    private GameObject draggingObject;
    private Vector3 draggingOffset;

    void Start()
    {
        // FONDO
        backgroundRenderer.sprite = roomBackground;
        backgroundRenderer_2.enabled = false;

        // MANIQUI
        mannequinRenderer.sprite = mannequinNormal;

        // MANOS
        handsRenderer.sprite = handsNormal;

        // FASES
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

                currentStep = Step.DragVest;

                break;

            case Step.VestPlaced:

                currentStep = Step.DragMask;

                break;

            case Step.MaskPlaced:



                //-----------------------ETAPA 2 INICIO---------------------------


                StartPhase2();

                currentStep = Step.Phase2Intro;

                ShowDialogue();

                break;

            case Step.Phase2Intro:

                currentStep = Step.DragSoap;

                break;

            case Step.SoapPlaced:

                currentStep = Step.TurnOnWater;

                break;

            case Step.WaterRunning:

                currentStep = Step.TurnOffWater;

                break;
            case Step.TurnOffWater:

                currentStep = Step.DragTowel;

                break;

            case Step.TowelPlaced:

                currentStep = Step.Finished;

                ShowDialogue();

                StartCoroutine(FinishRoutine());

                break;
        }
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            /*RaycastHit2D hit =
                Physics2D.Raycast(worldPoint, Vector2.zero);*/
            Collider2D hit =
    Physics2D.OverlapPoint(worldPoint);
            if (hit != null)
            {
                GameObject clickedObject = hit.gameObject;
            }
            if (hit/*.collider*/ != null)
            {
                GameObject clickedObject = hit/*.collider*/.gameObject;
                Debug.Log(clickedObject.name);
                // CHALECO
                if (currentStep == Step.DragVest &&
                    clickedObject == vestObject)
                {
                    BeginDrag(clickedObject);
                    return;
                }

                // BARBIJO
                if (currentStep == Step.DragMask &&
                    clickedObject == maskObject)
                {
                    BeginDrag(clickedObject);
                    return;
                }

                // JABON
                if (currentStep == Step.DragSoap &&
                    clickedObject == soapObject)
                {
                    BeginDrag(clickedObject);
                    return;
                }

                // TOALLA
                if (currentStep == Step.DragTowel &&
                    clickedObject == towelObject)
                {
                    BeginDrag(clickedObject);
                    return;
                }

                // CANILLA
                if (currentStep == Step.TurnOnWater &&
     clickedObject == faucetRenderer.gameObject)
                {
                    OpenWater();
                    return;
                }
                if (currentStep == Step.TurnOffWater &&
    clickedObject == faucetRenderer.gameObject)
                {
                    TurnOffFaucet();
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

        // DROP MANIQUI
        if ((go == vestObject || go == maskObject) &&
            mannequinDropZone != null)
        {
            insideDropZone =
                mannequinDropZone.OverlapPoint(go.transform.position);
        }

        // DROP MANOS
        if ((go == soapObject || go == towelObject) &&
            handsDropZone != null)
        {
            insideDropZone =
                handsDropZone.OverlapPoint(go.transform.position);
        }

        // SI FALLA
        if (!insideDropZone)
        {
            ReturnObject(go);
            return;
        }

        // CHALECO
        if (go == vestObject &&
            currentStep == Step.DragVest)
        {
            PlaceVest();
            return;
        }

        // BARBIJO
        if (go == maskObject &&
            currentStep == Step.DragMask)
        {
            PlaceMask();
            return;
        }

        // JABON
        if (go == soapObject &&
            currentStep == Step.DragSoap)
        {
            PlaceSoap();
            return;
        }

        // TOALLA
        if (go == towelObject &&
            currentStep == Step.DragTowel)
        {
            PlaceTowel();
            return;
        }
    }

    void ReturnObject(GameObject go)
    {
        // CHALECO
        if (go == vestObject &&
            vestStartPosition != null)
        {
            go.transform.position =
                vestStartPosition.position;
        }

        // BARBIJO
        if (go == maskObject &&
            maskStartPosition != null)
        {
            go.transform.position =
                maskStartPosition.position;
        }

        // JABON
        if (go == soapObject &&
            soapStartPosition != null)
        {
            go.transform.position =
                soapStartPosition.position;
        }

        // TOALLA
        if (go == towelObject &&
            towelStartPosition != null)
        {
            go.transform.position =
                towelStartPosition.position;
        }
    }

    // ==================================================
    // FASE 1
    // ==================================================

    void PlaceVest()
    {
        if (vestPlaced) return;

        vestPlaced = true;

        vestObject.SetActive(false);

        mannequinRenderer.sprite =
            mannequinWithVest;

        currentStep = Step.VestPlaced;

        ShowDialogue();
    }

    void PlaceMask()
    {
        if (maskPlaced) return;

        maskPlaced = true;

        maskObject.SetActive(false);

        mannequinRenderer.sprite =
            mannequinComplete;

        currentStep = Step.MaskPlaced;

        ShowDialogue();
    }

    // ==================================================
    // FASE 2
    // ==================================================

    void StartPhase2()
    {
        backgroundRenderer_2.enabled = true;

        backgroundRenderer_2.sprite = sinkBackground;

        backgroundRenderer.enabled = false ;

        phase1Objects.SetActive(false);

        phase2Objects.SetActive(true);

        handsRenderer.sprite = handsNormal;
    }

    void PlaceSoap()
    {
        if (soapPlaced) return;

        soapPlaced = true;

        soapObject.SetActive(false);

        handsRenderer.sprite =
            handsWithSoap;

        currentStep = Step.SoapPlaced;

        ShowDialogue();
    }

    void OpenWater()
    {
        if (waterOpened) return;

        waterOpened = true;
        faucetRenderer.sprite = faucetOn;

        handsRenderer.sprite =
            handsWashing;

        currentStep = Step.WaterRunning;

        StartCoroutine(WashingRoutine());
    }
    IEnumerator WashingRoutine()
    {
        yield return new WaitForSeconds(2f);

        handsRenderer.sprite = handsWashing;

        /*ShowDialogue(
            
        );*/

        currentStep = Step.TurnOffWater;
    }
    void TurnOffFaucet()
    {
        faucetRenderer.sprite = faucetOff;
        handsRenderer.sprite =
            handsWet;
        /*ShowDialogue(

          );*/

        currentStep = Step.DragTowel;
    }
    void PlaceTowel()
    {
        if (towelPlaced) return;

        towelPlaced = true;

        towelObject.SetActive(false);

        handsRenderer.sprite =
            handsWet;

        currentStep = Step.TowelPlaced;

        ShowDialogue();
    }

    // ==================================================
    // DIALOGOS
    // ==================================================

    void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
        rikuRenderer.enabled =true;
        nextButton.gameObject.SetActive(true);
        instructionText.gameObject.SetActive(true);
        UpdateInstruction();
    }
    void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        instructionText.gameObject.SetActive(false);
        rikuRenderer.enabled =false;
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

    void UpdateInstruction()
    {
        if (instructionText == null) return;

        switch (currentStep)
        {
            case Step.Intro:

                instructionText.text =
                    "¡Hola! Soy Riku y voy a enseñarte cómo prepararte correctamente.";

                break;

            case Step.DragVest:

                instructionText.text =
                    "Primero coloca el chaleco sobre el maniquí.";
                HideDialogue();
                break;

            case Step.VestPlaced:

                instructionText.text =
                    "¡Muy bien! Ahora continuemos.";

                break;

            case Step.DragMask:

                instructionText.text =
                    "Ahora coloca el barbijo sobre el maniquí.";
                HideDialogue();
                break;

            case Step.MaskPlaced:

                instructionText.text =
                    "¡Excelente! Ahora vamos a lavarnos las manos.";

                break;

            case Step.Phase2Intro:

                instructionText.text =
                    "Debemos higienizar nuestras manos antes del procedimiento.";

                break;

            case Step.DragSoap:

                instructionText.text =
                    "Primero coloca jabón sobre las manos.";
                HideDialogue();
                break;

            case Step.SoapPlaced:

                instructionText.text =
                    "¡Perfecto! Ahora abre la canilla.";

                break;

            case Step.TurnOnWater:

                instructionText.text =
                    "Haz click sobre la canilla.";

                break;

            case Step.WaterRunning:

                instructionText.text =
                    "¡Muy bien! Ahora seca las manos con la toalla.";
                
                break;
            case Step.TurnOffWater:

                instructionText.text =
                    "¡Muy bien! Ahora seca las manos con la toalla.";
                HideDialogue();
                break;
            case Step.DragTowel:

                instructionText.text =
                    "Arrastra la toalla hacia las manos.";
                HideDialogue();
                break;

            case Step.TowelPlaced:

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
        congratsPanel?.SetActive(true);

        yield return new WaitForSeconds(2f);
        /*bridge.SendResultToReact();*/

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}