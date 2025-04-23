using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitable
{
    public void TakeDamage(float damagePoints);
    public void KnockbackEntity(Transform executionSource);

    public IEnumerator getKnockedBack(Vector3 force);
}
