using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPool : MonoBehaviour
{
    [SerializeField]
    private Target basetarget;

    private int poolSize = 10;

    private List<Target> targetCollection = new List<Target>();

    public Target GetTarget()
    {
        Target target = null;

        if(targetCollection.Count > 0)
        {
            target = targetCollection[0];
            targetCollection.RemoveAt(0);
            target.gameObject.SetActive(true);
        }
        return target;
    }

    public void StoreTarget(Target storedTarget)
    {
        targetCollection.Add(storedTarget);
        storedTarget.rb.velocity = Vector3.zero;
        storedTarget.gameObject.SetActive(false);
    }

    private void Awake()
    {
        if(basetarget != null)
        {
            for(int i = 0; i< poolSize; i++)
            {
                targetCollection.Add(Instantiate<Target>(basetarget));
            }
        }
    }

}
