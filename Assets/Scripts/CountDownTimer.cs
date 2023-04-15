using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    public float countdownTime = 3.0f; // Time in seconds for countdown, including "GO!"
    public float fadeDuration = 0.2f; // Time in seconds for fade animation
    private TextMeshProUGUI countdownText; // TextMeshPro object to display countdown

    void Start()
    {
        countdownText = GetComponent<TextMeshProUGUI>(); 
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(1.0f); // Add a delay of 1 second before starting the countdown
        FighterManager.disableAllFighterInputs?.Invoke();

        for (int i = (int)countdownTime; i >= 1; i--)
        {
            countdownText.text = i.ToString();
            countdownText.alpha = 1.0f;

            yield return new WaitForSeconds(0.2f);

            // Animate the countdown text fading out
            yield return StartCoroutine(FadeOut(countdownText, fadeDuration));

            // Wait for one second before starting the next countdown
            yield return new WaitForSeconds(1.0f);
        }

        // Display "GO!" for one second
        countdownText.text = "GO!";
        countdownText.alpha = 1.0f;

        yield return new WaitForSeconds(0.2f);

        // Animate the countdown text fading out
        yield return StartCoroutine(FadeOut(countdownText, fadeDuration));

        yield return new WaitForSeconds(1.0f);

        // Hide or disable the countdown timer
        countdownText.enabled = false;
        FighterManager.enableAllFighterInputs?.Invoke();
    }

   
    IEnumerator FadeOut(TextMeshProUGUI text, float duration)
    {
        float elapsedTime = 0.0f;
        Color originalColor = text.color;

        while (elapsedTime < duration)
        {
            // Calculate the new alpha value based on the elapsed time and duration
            float alpha = Mathf.Lerp(1.0f, 0.0f, elapsedTime / duration);

            // Update the alpha value of the text
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Set the final alpha value to 0
        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.0f);
    }
}

