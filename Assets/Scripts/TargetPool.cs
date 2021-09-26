using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetPool : MonoBehaviour
{
    [SerializeField]
    private Target basetarget;

    private int poolSize = 3;

    private List<Target> targetCollection = new List<Target>();

    public Target GetTarget()
    {
        Target target = null;

        if(targetCollection.Count > 0)
        {
            target = targetCollection[0];
            targetCollection.RemoveAt(0);
            target.gameObject.SetActive(true);
            target.enabled = true;
        }
        else
        {
            target = Instantiate<Target>(basetarget);
        }

        target.onTargetStored += StoreTarget;

        return target;
    }

    public void StoreTarget(Target storedTarget)
    {
        storedTarget.onTargetStored -= StoreTarget;
        targetCollection.Add(storedTarget);
        storedTarget.rb.velocity = Vector3.zero;
        storedTarget.gameObject.SetActive(false);
        storedTarget.enabled = false;
        storedTarget.transform.position = transform.position;
    }

    private void Awake()
    {
        if(basetarget != null)
        {
            for(int i = 0; i< poolSize; i++)
            {
                Target targetInstance = Instantiate<Target>(basetarget);
                StoreTarget(targetInstance);
            }
        }
    }

}
