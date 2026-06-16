using UnityEngine;
using System.Collections;

public class QuitGame : MonoBehaviour
{
    public void Exit()
    {
        StartCoroutine(DelayedQuit());
    }

    IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(1f); // หน่วงเวลา 1 วินาที

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}