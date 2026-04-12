using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;


public class GridManager: MonoBehaviour
{
[SerializeField] private int _largura, _altura;
[SerializeField] private Tile _tilePrefab;
[SerializeField] private Transform _camera;
void Start()
    {
        CriarGrid();
    }

void CriarGrid()
{
    for (int x = 0; x < _largura; x++)
    {
        for (int y = 0; y < _altura; y++)
        {
            var CriarTile = Instantiate(_tilePrefab, new Vector3(x,y), Quaternion.identity);
            CriarTile.name = $"tile {x} {y}";

            var impar_par = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
            CriarTile.Init(impar_par);
        }
    }

    _camera.transform.position = new Vector3((float)_largura/2 - 0.5f, (float)_altura/2 - 0.5f, -10);

}
}
