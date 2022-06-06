using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCountEndAnimationHandler : MonoBehaviour
{
    private void OnEndEvent()
    {
        this.gameObject.SetActive(false);
        this.transform.parent.gameObject.transform.parent.transform.Find("TxStatus").GetComponentInParent<TxStatus>().ToDoAfterAddCountAnimation();
    }
}
