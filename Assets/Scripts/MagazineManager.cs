using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagazineManager : MonoBehaviour
{
    private Dictionary<Magazine, Vector3> _magazineDic = new Dictionary<Magazine, Vector3>();
    [SerializeField] private GameObject _magazinePrefab;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddMagazine(Magazine magazine, Vector3 pos)
    {
        _magazineDic.Add(magazine, pos);
    }

    public void MagazineEmptied(Magazine magazine)
    {
        if(_magazineDic.ContainsKey(magazine))
        {
            Vector3 pos = _magazineDic[magazine];
            _magazineDic.Remove(magazine);

            Instantiate(_magazinePrefab, pos, Quaternion.identity);
        }
        
    }
}
