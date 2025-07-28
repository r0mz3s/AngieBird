using UnityEngine;
using UnityEngine.UI;

public class IconHandler : MonoBehaviour
{
    [SerializeField] private Image[] icon;
    [SerializeField] private Color usedColor;

    public void UseShot(int shotNumber)
    { 
        for (int i = 0; i < icon.Length; i++)
        {
            if (i + 1 == shotNumber)
            {
                icon[i].color = usedColor;
                return;
            }
        }
    }
}
