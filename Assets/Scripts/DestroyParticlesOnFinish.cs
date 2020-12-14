using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticlesOnFinish : MonoBehaviour
{
    private ParticleSystem ps;
    private void Update()
    {
        if (ps && !ps.IsAlive())
            Destroy(gameObject);
    }
}
