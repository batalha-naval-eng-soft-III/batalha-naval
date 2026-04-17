using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShipDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Arraste o seu ScriptableObject (ShipSO) aqui!")]
    public ShipSO shipSO;

    private GridManager gridManager;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 startPosition;
    private ShipSO.Dir direcaoAtual = ShipSO.Dir.Cima;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();

        gridManager = FindObjectOfType<GridManager>();

        // Força o container a ter o tamanho de 1 célula
        if (gridManager != null && gridManager.grid != null)
        {
            rectTransform.sizeDelta = new Vector2(gridManager.grid.cellSize, gridManager.grid.cellSize);
        }

        // Desliga a imagem base para não ficar uma imagem esticada atrás
        Image baseImage = GetComponent<Image>();
        if (baseImage != null) baseImage.enabled = false;

        // --- A MÁGICA: CRIA AS BOLINHAS/QUADRADOS SEPARADOS ---
        if (shipSO != null && shipSO.shipSprite != null && gridManager != null)
        {
            foreach (Vector2Int offset in shipSO.shapeOffsets)
            {
                // Cria a bolinha
                GameObject blockObj = new GameObject($"Bloco_{offset.x}_{offset.y}");
                blockObj.transform.SetParent(this.transform, false);

                Image blockImage = blockObj.AddComponent<Image>();
                blockImage.sprite = shipSO.shipSprite;

                // Ajusta pro tamanho do quadradinho da sua grid
                RectTransform blockRect = blockObj.GetComponent<RectTransform>();
                blockRect.sizeDelta = new Vector2(gridManager.grid.cellSize, gridManager.grid.cellSize);

                // Posiciona ela certinho na distância
                blockRect.anchoredPosition = new Vector2(offset.x * gridManager.grid.cellSize, offset.y * gridManager.grid.cellSize);
            }
        }
    }

    private void Update()
    {
        // Gira tudo junto se apertar R
        if (canvasGroup.blocksRaycasts == false && Keyboard.current.rKey.wasPressedThisFrame)
        {
            rectTransform.Rotate(0, 0, -90f);
            direcaoAtual = ShipSO.GetNextDir(direcaoAtual);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        startPosition = rectTransform.anchoredPosition;
        canvasGroup.alpha = 0.7f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / GetComponentInParent<Canvas>().scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        (int gridX, int gridY) = gridManager.grid.GetXY(mouseWorldPos);

        Vector2Int offsetBase = new Vector2Int(gridX, gridY);
        List<Vector2Int> posicoesOcupadas = shipSO.GetGridPositionList(offsetBase, direcaoAtual);

        if (gridManager.grid.CanPlaceShip(shipSO, new Vector3(gridX, gridY, 0), direcaoAtual))
        {
            // Tem espaço? Cola na matriz!
            gridManager.grid.PlaceShip(posicoesOcupadas, shipSO.shipID);

            Vector3 snapWorldPosition = gridManager.grid.GetWorldPosition(gridX, gridY);
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, snapWorldPosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform.parent as RectTransform, screenPoint, null, out Vector2 localPoint);

            rectTransform.anchoredPosition = localPoint + new Vector2(gridManager.grid.cellSize / 2f, gridManager.grid.cellSize / 2f);
        }
        else
        {
            // Não tem espaço? Volta pra mão
            rectTransform.anchoredPosition = startPosition;
            rectTransform.rotation = Quaternion.identity;
            direcaoAtual = ShipSO.Dir.Cima;
        }
    }
}