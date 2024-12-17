using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillDataPopup : MonoBehaviour
{
    [SerializeField] Text txtSkillName, txtSkillDescription;
    [SerializeField] GameObject arrowGo;
    private RectTransform popupRt, btnRt;

    private void Awake()
    {
        popupRt = transform.Find("Popup").GetComponent<RectTransform>();

        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(btnRt, Input.mousePosition)) return;
            if (RectTransformUtility.RectangleContainsScreenPoint(popupRt, Input.mousePosition)) return;
            else gameObject.SetActive(false);
        }
    }

    public void Init(string skillName, string skillDescription, Vector2 btnPos, RectTransform btnRt)
    {
        txtSkillName.text = skillName;
        txtSkillDescription.text = string.Format(skillDescription);

        Vector2 arrowVec = new Vector2(btnPos.x, arrowGo.transform.position.y);
        arrowGo.transform.position = arrowVec;

        this.btnRt = btnRt;
    }
}
