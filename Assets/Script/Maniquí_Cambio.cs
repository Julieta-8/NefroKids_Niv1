using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class Maniqui_Cambio : MonoBehaviour
{
    public enum Step
    {
        Intro,
        DragVest,
        VestPlaced,
        DragMask,
        MaskPlaced,
        Finished
    }

    [Header("Maniquí")]
    public SpriteRenderer mannequinImage;

    [Header("Sprites del maniquí")]
    public Sprite mannequinNormal;
    public Sprite mannequinWithVest;
    public Sprite mannequinComplete;

    [Header("Objetos arrastrables")]
    public GameObject vestObject;
    public GameObject maskObject;

    [Header("Zona de colocación")]
    public Collider2D dropZone;

    [Header("UI")]
    public TMP_Text instructionText;
    public Button nextButton;
    public GameObject congratsPanel;

    [Header("Escena siguiente")]
    public string nextSceneName;

    [Header("Posiciones iniciales")]
    public Transform vestStartPosition;
    public Transform maskStartPosition;

    // Estado actual
    public Step currentStep = Step.Intro;

    // Flags
    private bool vestPlaced = false;
    private bool maskPlaced = false;

    // Drag
    private GameObject draggingObject;
    private Vector3 draggingOffset;

    void Start()
    {
        mannequinImage.sprite = mannequinNormal;

        congratsPanel?.SetActive(false);

        currentStep = Step.Intro;

        UpdateInstruction();

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
        switch (currentStep)
        {
            case Step.Intro:

                currentStep = Step.DragVest;
                UpdateInstruction();

                break;

            case Step.VestPlaced:

                currentStep = Step.DragMask;
                UpdateInstruction();

                break;

            case Step.MaskPlaced:

                currentStep = Step.Finished;
                UpdateInstruction();

                StartCoroutine(FinishRoutine());

                break;
        }
    }

    void UpdateInstruction()
    {
        if (instructionText == null) return;

        switch (currentStep)
        {
            case Step.Intro:

                instructionText.text =
                    "¡Hola! Vamos a prepararnos correctamente antes del procedimiento. Pulsa NEXT para comenzar.";

                break;

            case Step.DragVest:

                instructionText.text =
                    "Primero coloca el chaleco de protección sobre el maniquí.";

                break;

            case Step.VestPlaced:

                instructionText.text =
                    "¡Muy bien! Ahora pulsa NEXT para continuar.";

                break;

            case Step.DragMask:

                instructionText.text =
                    "Ahora coloca el barbijo sobre el maniquí.";

                break;

            case Step.MaskPlaced:

                instructionText.text =
                    "¡Excelente trabajo! Pulsa NEXT para finalizar.";

                break;

            case Step.Finished:

                instructionText.text =
                    "¡Completado!";

                break;
        }
    }

    void HandleMouseInput()
    {
        // CLICK
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 worldPoint =
                Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit =
                Physics2D.Raycast(worldPoint, Vector2.zero);

            if (hit.collider != null)
            {
                GameObject clickedObject = hit.collider.gameObject;

                // SOLO permitir mover chaleco en su turno
                if (currentStep == Step.DragVest &&
                    clickedObject == vestObject)
                {
                    BeginDrag(clickedObject);
                    return;
                }

                // SOLO permitir mover barbijo en su turno
                if (currentStep == Step.DragMask &&
                    clickedObject == maskObject)
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
        if (dropZone != null)
        {
            insideDropZone =
                dropZone.OverlapPoint(go.transform.position);
        }
        // Si NO cayó en la zona correcta
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
    }

    void ReturnObject(GameObject go)
    {
        if (go == vestObject &&
            vestStartPosition != null)
        {
            go.transform.position =
                vestStartPosition.position;
        }

        if (go == maskObject &&
            maskStartPosition != null)
        {
            go.transform.position =
                maskStartPosition.position;
        }
    }

    void PlaceVest()
    {
        if (vestPlaced) return;

        vestPlaced = true;

        vestObject.SetActive(false);

        mannequinImage.sprite =
            mannequinWithVest;

        currentStep = Step.VestPlaced;

        UpdateInstruction();
    }

    void PlaceMask()
    {
        if (maskPlaced) return;

        maskPlaced = true;

        maskObject.SetActive(false);

        mannequinImage.sprite =
            mannequinComplete;

        currentStep = Step.MaskPlaced;

        UpdateInstruction();
    }

    IEnumerator FinishRoutine()
    {
        congratsPanel?.SetActive(true);

        yield return new WaitForSeconds(2f);

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}