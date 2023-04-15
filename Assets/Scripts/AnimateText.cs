using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimateText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private string[] animationFrames = { "Waiting for other players.", "Waiting for other players..", "Waiting for other players..." };
    private int currentFrame = 0;
    private float animationSpeed = 0.5f;
    private float timer = 0f;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= animationSpeed)
        {
            currentFrame++;
            if (currentFrame >= animationFrames.Length)
            {
                currentFrame = 0;
            }
            textMesh.text = animationFrames[currentFrame];
            timer = 0f;
        }
    }
}