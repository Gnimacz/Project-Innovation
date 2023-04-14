using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FighterFollowerArrow : MonoBehaviour
{
    public GameObject target;
    Camera camera;
    RectTransform rect;
    Vector3 center;
    Image image;
    public float distFromEdge = 50f;
    public FighterController fighter;
    
    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rect = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        center = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetScreenPos = camera.WorldToScreenPoint(target.transform.position);
        targetScreenPos = targetScreenPos - center;
        
        rect.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(targetScreenPos.normalized, Vector3.right, Vector3.back));

        Vector3 limited = new Vector3(
            Mathf.Clamp(targetScreenPos.x, -center.x + distFromEdge, center.x - distFromEdge),
            Mathf.Clamp(targetScreenPos.y, -center.y + distFromEdge, center.y - distFromEdge),
            0);

        image.color = PlayerColors.colors[fighter.playerId];
        rect.localPosition = limited;
        
        if (Mathf.Abs(targetScreenPos.x) > center.x || Mathf.Abs(targetScreenPos.y) > center.y)
        {
            image.enabled = true;
            return;
        }
        image.enabled = false;

    }
}
