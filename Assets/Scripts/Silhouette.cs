using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Silhouette : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Color _originalColor;
    [SerializeField] private Color _fadedColor;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        while (true)
        {
            _spriteRenderer.color = _fadedColor;
            yield return new WaitForSeconds(0.25f);
            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(0.25f);
        }
    }
}
