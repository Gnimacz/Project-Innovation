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
    public List<GameObject> fighterImages = new List<GameObject>();
    public Image ImageguiFighterImage;


    public void SetValues(FighterController fighter)
    {
        playerText.text = "Player " + (fighter.playerId + 1).ToString();
        playerText.color = PlayerColors.colors[fighter.playerId];
        percentageText.color = PlayerColors.colors[fighter.playerId];
        //guiImage.color = PlayerColors.colors[fighter.playerId];
        //guiImage.GetComponent<Image>().color = PlayerColors.colors[fighter.playerId];
        attachedFighterId = fighter.playerId;
        //Debug.Log(SimpleServerDemo.instance.clientInfoList.Find(x => x.Item2 == fighter).Item3);
        fighterImages[fighter.selectedCharacterId].SetActive(true);
    }

    void Awake()
    {
        FighterManager.OnFighterHurt += OnFighterHurt;
    }
    private void OnDestroy()
    {
        FighterManager.OnFighterHurt -= OnFighterHurt;
    }
    //GameObject attacker, GameObject victim, int damage
    void OnFighterHurt(GameObject attacker, GameObject fighter, int damage)
    {
        if (fighter.GetComponent<FighterController>().playerId != attachedFighterId) return;
        if (fighter == attacker && damage > 0) return;

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
