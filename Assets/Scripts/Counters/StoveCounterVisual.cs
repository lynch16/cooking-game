using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;
    [SerializeField] private GameObject stoveOnGameObject;
    [SerializeField] private GameObject particlesGameObject;

    private void Start() {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
        bool setActive = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
        SetVisualActive(setActive);
    }

    private void SetVisualActive(bool active) {
        stoveOnGameObject.SetActive(active);
        particlesGameObject.SetActive(active);
    }
}
