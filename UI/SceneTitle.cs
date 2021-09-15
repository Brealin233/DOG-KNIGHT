using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class SceneTitle : MonoBehaviour
{
    TextMeshProUGUI title;
    public Transform endPos;

    private void Start()
    {
        title = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        title.gameObject.SetActive(true);
        title.transform.DOMove(endPos.position, 4f);
        title.DOFade(0, 4f);
        Destroy(this.gameObject);
    }
}
