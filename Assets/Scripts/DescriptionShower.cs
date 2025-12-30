using UnityEngine;

public class DescriptionShower : MonoBehaviour
{
    public static DescriptionShower instance;

    [SerializeField] private TMPro.TextMeshProUGUI descriptionText;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowDescription(string description)
    {
        descriptionText.text = description;
    }

    public void HideDescription()
    {
        descriptionText.text = "";
    }
}
