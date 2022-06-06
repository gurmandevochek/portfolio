using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FailEndAnimationHandler : MonoBehaviour
{
    private void OnEndEvent()
    {
        this.gameObject.GetComponentInParent<TxStatus>().ToDoAfterFailAnimation();
    }
}
