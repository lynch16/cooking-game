using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

    [SerializeField] BaseCounter selectedCounter;
    [SerializeField] GameObject[] visualGameObjects;

    private void Start() {
        Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
    }

    private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
        if (e.selectedCounter == selectedCounter) {
            Show();
        } else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject gameObject in visualGameObjects) {
            gameObject.SetActive(true);
        }
    }
    private void Hide() {
        foreach (GameObject gameObject in visualGameObjects) {
            gameObject.SetActive(false);
        }
    }
}
