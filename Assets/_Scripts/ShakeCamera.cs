using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    public Animator CamAnim;

    public static ShakeCamera instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de ShakeCamera dans la scène");
            return;
        }
    }

    public void CamShake()
    {
        CamAnim.SetTrigger("Shake");
    }
}
