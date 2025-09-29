using System.Collections;
using UnityEngine;

public class LoaderView : MonoBehaviour
{
    private Coroutine _loadingCoroutine;
    public float rotationSpeed = 350f; // градусов в секунду
    private bool _canRotate;
    private bool _isLoading = false;

    private void OnEnable()
    {
        _isLoading = true;
        _loadingCoroutine = StartCoroutine(DelayLoading());
    }

    private void OnDisable()
    {
        _isLoading = false;
        if (_loadingCoroutine != null)
            StopCoroutine(_loadingCoroutine);
    }

    private IEnumerator DelayLoading()
    {
        while (_isLoading)
        {
            float elapsed = 0f;
            while (elapsed < 1f && _isLoading)
            {
                float delta = Time.deltaTime;
                gameObject.transform.Rotate(0, 0, rotationSpeed * delta);
                elapsed += delta;
                yield return null;
            }
        }
    }
}
