using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIWord : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public string description;
    public TextMeshProUGUI wordText;

    public void OnSelect(BaseEventData eventData)
    {
        DescriptionShower.instance.ShowDescription(description);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        DescriptionShower.instance.HideDescription();
    }
}
