using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashManager : Singleton<SlashManager>
{
    [SerializeField]
    private Transform activeSlashes;
    [SerializeField]
    private Transform inactiveSlashes;

    [SerializeField] private GameObject slashPrefab;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeNewSlash(Vector3[] positions)
    {
        var newSlash = GetASlash();
        newSlash.DoSlash(positions);
    }

    public SlashUnit GetASlash()
    {
        if (inactiveSlashes.childCount == 0)
        {
            var newSlash = Instantiate(slashPrefab, inactiveSlashes);
        }
        
        
        return (inactiveSlashes.GetChild(0).GetComponent<SlashUnit>());
        
    }

    public void MoveSlashToActive(SlashUnit slash)
    {
        slash.transform.parent = activeSlashes;
    }
    
    public void MoveSlashToInactive(SlashUnit slash)
    {
        
        slash.transform.parent = inactiveSlashes;
    }
}
