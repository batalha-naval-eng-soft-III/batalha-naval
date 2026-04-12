using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

public class Tile : MonoBehaviour
{

    [SerializeField] private Color _corPar, _corImpar;
    [SerializeField] private SpriteRenderer _renderizacao;


    public void Init(bool impar_par)
    {
        _renderizacao.color = impar_par ? _corImpar : _corPar;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created

}
