using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticlesOnFinish : MonoBehaviour
{
    private float _timer = 0f;
    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= 1)
        {
            Destroy(gameObject);
        }
    }
}
