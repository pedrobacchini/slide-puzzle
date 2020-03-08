using UniRx;
using UnityEngine;

public class WinDisplay : MonoBehaviour
{
    public GameManager GameManager;

    private void Start()
    {
        GameManager.GameState.Subscribe(state => gameObject.SetActive(state.Equals(GameState.Win)));
    }
}
