using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PercentageUI : MonoBehaviour
{
    public TextMeshProUGUI percentageText;
    public TextMeshProUGUI playerText;
    public Image guiImage;
    private int attachedFighterId = 0;


    public void SetValues(int fighter)
    {
        playerText.text = "Player " + (fighter + 1).ToString();
        playerText.color = PlayerColors.colors[fighter];
        percentageText.color = PlayerColors.colors[fighter];
        guiImage.color = PlayerColors.colors[fighter];
        guiImage.GetComponent<Image>().color = PlayerColors.colors[fighter];
        attachedFighterId = fighter;
    }

    void Awake()
    {
        FighterManager.OnFighterHurt += OnFighterHurt;
    }

    void OnFighterHurt(Vector3 attackerPosition, GameObject fighter, int damage)
    {
        if (fighter.GetComponent<FighterController>().playerId != attachedFighterId) return;

        LeanTween.rotateZ(percentageText.gameObject, Random.value * damage, 0.05f)
            .setOnComplete(() =>
            {
                LeanTween.rotateZ(percentageText.gameObject, 0, 0.05f);
            });
        
        LeanTween.scale(percentageText.gameObject, percentageText.transform.localScale * 1.25f, 0.1f)
            .setEaseInOutBounce()
            .setOnComplete(() =>
            {
                LeanTween.scale(percentageText.gameObject, Vector3.one, 0.1f).setEaseInOutBounce();
            });
        percentageText.text = fighter.GetComponent<FighterController>().health + " \u0027 /.";
    }
}
