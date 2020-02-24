using System;
using System.Collections;
using System.Collections.Generic;
using GameCoreEngine;
using UnityEngine;

public class ParticleAttractor : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private ParticleSystem system;

    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[5];

    int count;

    void Update()
    {
        count = system.GetParticles(particles);

        for (int i = 0; i < count; i++)
        {
            ParticleSystem.Particle particle = particles[i];

            Vector3 v1 = system.transform.TransformPoint(particle.position);
            Vector3 v2 = target.transform.position;

            Vector3 tarPosi = (v2 - v1) * (particle.remainingLifetime / particle.startLifetime);
            particle.position = system.transform.InverseTransformPoint(v2 - tarPosi);
            particles[i] = particle;
        }

        system.SetParticles(particles, count);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
