using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class HoverDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea]
    [SerializeField] private string description; // Descripción que aparecerá al pasar el mouse
    public TMP_Text targetText; // Referencia al TextMeshPro que se actualizará

    private string originalText = "";

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetText != null)
        {
            originalText = targetText.text;
            targetText.text = description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetText != null)
        {
            targetText.text = originalText;
        }
    }
}
