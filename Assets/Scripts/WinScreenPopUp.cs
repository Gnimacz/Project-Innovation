using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScreenPopUp : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public RectTransform uiElement;

    private bool isAnimating = false;

    // Start is called before the first frame update
    private void Start()
    {
        uiElement.localScale = Vector3.zero;
        Text = FindAnyObjectByType<TextMeshProUGUI>();

    }
    
    private void Update()
    {
        Text.text = "Player _ won!";

        if (Input.GetKeyDown(KeyCode.Space) && !isAnimating)
        {
            isAnimating = true;

            LeanTween.scale(uiElement, Vector3.one, 0.8f)
                .setEaseOutExpo()
                .setOnComplete(() => {
                    isAnimating = false;
                });
        }
    }
}
