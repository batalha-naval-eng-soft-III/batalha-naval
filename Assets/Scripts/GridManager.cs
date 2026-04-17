using UnityEngine;
using UnityEngine.InputSystem;

public class GridManager : MonoBehaviour
{
    public Grid grid;
    void Start()
    {
        grid = new Grid(10, 10, 10f);
    }
}
