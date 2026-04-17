using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShipSO", menuName = "Scriptable Objects/Ship")]
public class ShipSO : ScriptableObject
{
    public string shipName;

    // O número que vai ser anotado na sua Folha Quadriculada (Grid.cs)
    // Ex: 0 é água, 1 é o Submarino, 2 é o Porta-Aviões
    public int shipID;
    public Sprite shipSprite; // A imagem do seu navio (Lembre-se de arrastar o Sprite aqui no Inspector)

    // As direções para podermos girar a peça (Reaproveitado do PlacedObjectTypeSO)
    public enum Dir { Cima, Direita, Baixo, Esquerda }

    [Header("Formato do Navio")]
    [Tooltip("Coloque os pedaços aqui. O Líder sempre é X:0, Y:0")]
    public List<Vector2Int> shapeOffsets;

    // --- MÉTODOS DE DIREÇÃO (Reaproveitados do PlacedObjectTypeSO) ---

    public static Dir GetNextDir(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Cima: return Dir.Direita;
            case Dir.Direita: return Dir.Baixo;
            case Dir.Baixo: return Dir.Esquerda;
            case Dir.Esquerda: return Dir.Cima;
        }
    }

    public static Vector2Int GetDirForwardVector(Dir dir)
    {
        switch (dir)
        {
            default:
            case Dir.Cima: return new Vector2Int(0, 1);
            case Dir.Direita: return new Vector2Int(1, 0);
            case Dir.Baixo: return new Vector2Int(0, -1);
            case Dir.Esquerda: return new Vector2Int(-1, 0);
        }
    }

    public static Dir GetDir(Vector2Int from, Vector2Int to)
    {
        if (from.x < to.x) return Dir.Direita;
        if (from.x > to.x) return Dir.Esquerda;
        if (from.y < to.y) return Dir.Cima;
        return Dir.Baixo;
    }

    // --- MATEMÁTICA DE POSICIONAMENTO ---

    // Essa é a matemática extraída do arquivo do Code Monkey!
    // Ela pega a posição do seu mouse (offsetBase) e calcula onde
    // cada pedaço do navio vai cair, dependendo de qual lado ele tá virado.
    public List<Vector2Int> GetGridPositionList(Vector2Int offsetBase, Dir dir)
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>();

        foreach (Vector2Int offset in shapeOffsets)
        {
            Vector2Int offsetRotacionado = offset;

            // Muda a direção dos "passos" igual o CodeMonkey faz
            switch (dir)
            {
                default:
                case Dir.Cima: offsetRotacionado = new Vector2Int(offset.x, offset.y); break;
                case Dir.Direita: offsetRotacionado = new Vector2Int(offset.y, -offset.x); break;
                case Dir.Baixo: offsetRotacionado = new Vector2Int(-offset.x, -offset.y); break;
                case Dir.Esquerda: offsetRotacionado = new Vector2Int(-offset.y, offset.x); break;
            }

            // Junta a posição do seu mouse com o pedaço rotacionado
            gridPositionList.Add(offsetBase + offsetRotacionado);
        }

        return gridPositionList;
    }
}
