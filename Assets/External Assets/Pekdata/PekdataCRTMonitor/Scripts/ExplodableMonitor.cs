using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExplodableMonitor : MonoBehaviour, IShootable
{
    [SerializeField]
    private GameObject screenExplosionParticleSystem;
    [SerializeField]
    private GameObject screenOff;
    [SerializeField]
    private GameObject screenOn;
    [SerializeField]
    private GameObject screenOnMedium;
    [SerializeField]
    private GameObject screenOnHard;
    [SerializeField]
    private GameObject shards;
    private bool broken;

    [Header("Difficulty")]
    public Image easyTexture;
    public Image mediumTexture;
    public Image hardTexture;

    public void Enable()
    {
        throw new System.NotImplementedException();
    }

    public void GetShot()
    {
        broken = true;
        screenOff.SetActive(false);
        screenOn.SetActive(false);
        screenOnMedium.SetActive(false);
        screenOnHard.SetActive(false);
        shards.SetActive(true);
        Rigidbody[] shardRBs = GetComponentsInChildren<Rigidbody>();
        screenExplosionParticleSystem.SetActive(true);
        foreach (Rigidbody shardRB in shardRBs)
        {
            float randomForce = Random.Range(1,5);
            float randomRotationX = Random.Range(-20,20);
            float randomRotationY = Random.Range(-20,20);
            float randomRotationZ = Random.Range(-20,20);
            shardRB.transform.Rotate(randomRotationX,randomRotationY,randomRotationZ);
            shardRB.AddRelativeForce(Vector3.forward * randomForce,ForceMode.Impulse);     
        }
    }

    public void ChangeDifficulty()
    {

    }
}
