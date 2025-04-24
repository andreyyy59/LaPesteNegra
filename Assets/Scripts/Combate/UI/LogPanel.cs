using UnityEngine;
using TMPro;
using System.Collections;

public class LogPanel : MonoBehaviour
{
    protected static LogPanel current;

    public TMP_Text logLabel;
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    public static bool IsTyping => current.isTyping;

    private void Awake()
    {
        current = this;
    }

    public static void Write(string message)
    {
        if (current.typingCoroutine != null)
            current.StopCoroutine(current.typingCoroutine);

        current.typingCoroutine = current.StartCoroutine(current.TypeText(message));
    }

    private IEnumerator TypeText(string message)
    {
        current.isTyping = true;
        logLabel.text = "";

        foreach (char c in message)
        {
            logLabel.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        current.isTyping = false;
    }

    public static IEnumerator WaitForMessage()
    {
        while (IsTyping)
            yield return null;
    }
}