using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutBoundsDestroy : MonoBehaviour
{
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

}
