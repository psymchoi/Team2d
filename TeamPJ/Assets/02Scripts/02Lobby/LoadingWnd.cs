using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : MonoBehaviour
{
    public Slider m_sliderState;

    public void SliderState(float curSlState)
    {
        m_sliderState.value = curSlState;
    }
}
