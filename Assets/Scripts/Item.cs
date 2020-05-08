using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public bool IsContacted{get; private set; } = false;

    ParticleSystem _particle;

    [SerializeField] Material _contactedMaterial;

    void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }


    void OnCollisionEnter( Collision other )
    {
        if( IsContacted || ! other.gameObject.CompareTag("Player"))
            return;

        IsContacted = true;
        _particle.Emit(100);
        GetComponent<Renderer>().material = _contactedMaterial;
    }
}
