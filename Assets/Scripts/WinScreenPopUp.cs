using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinScreenPopUp : MonoBehaviour
{
    public TextMeshProUGUI Text;
    public RectTransform uiElement;


    // Start is called before the first frame update
    private void Start()
    {
        //uiElement.localScale = Vector3.zero;
        uiElement.transform.localScale = Vector3.zero;
        FighterManager.OnPlayerWon += OnPlayerWon;
        TimerScript.OnTimerEndEvent += OnTimerEnd;

    }

    //private void Awake()
    //{
    //    gameObject.SetActive(false);
    //}

    private void OnTimerEnd()
    {
        if (uiElement == null) return;
        FighterManager.OnPlayerWon -= OnPlayerWon;
        Text.text = "Time's Up!\nDraw!";
        LeanTween.scale(uiElement, Vector3.one, 0.8f)
                 .setEaseOutExpo()
                 .setOnComplete(() =>
                 {
                     LeanTween.delayedCall(5, () =>
                     {
                         LeanTween.scale(uiElement, Vector3.zero, 0.8f)
                         .setEaseOutExpo()
                         .setOnComplete(() =>
                         {
                             SimpleServerDemo.UpdateServerState?.Invoke(SimpleServerDemo.ServerState.MainMenu);
                             //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                         });
                     });
                 });
    }

    private void OnPlayerWon(int id)
    {
        if (uiElement == null) return;
        FighterManager.OnPlayerWon -= OnPlayerWon;
        Text.text = "Player " + (id + 1).ToString() + " Wins!";
        LeanTween.scale(uiElement, Vector3.one, 0.8f)
                 .setEaseOutExpo()
                 .setOnComplete(() =>
                 {
                     LeanTween.delayedCall(5, () =>
                     {
                         LeanTween.scale(uiElement, Vector3.zero, 0.8f)
                         .setEaseOutExpo()
                         .setOnComplete(() =>
                         {
                             SimpleServerDemo.UpdateServerState?.Invoke(SimpleServerDemo.ServerState.MainMenu);
                             //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
                         });
                     });
                 });
    }

    //private void Update()
    //{
    //    Text.text = "Player _ won!";

    //    if (Input.GetKeyDown(KeyCode.Space) && !isAnimating)
    //    {
    //        isAnimating = true;

    //        LeanTween.scale(uiElement, Vector3.one, 0.8f)
    //            .setEaseOutExpo()
    //            .setOnComplete(() => {
    //                isAnimating = false;
    //            });
    //    }
    //}

    //private void OnEnable()
    //{
    //    LeanTween.scale(uiElement, Vector3.one, 0.8f)
    //             .setEaseOutExpo();
    //}
    //private void OnDisable()
    //{
    //    LeanTween.scale(uiElement, Vector3.zero, 0.8f)
    //             .setEaseOutExpo()
    //             .setOnComplete(() =>
    //             {
    //                 gameObject.SetActive(false);
    //             });
    //}
}
