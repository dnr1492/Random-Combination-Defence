using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hp : MonoBehaviour
{
    [SerializeField] Slider sliderHp;

    public void SetHp(float curHp, float maxHp) => sliderHp.value = curHp / maxHp;
}
