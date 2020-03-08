using UniRx;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class AmountMovementDisplay : MonoBehaviour
{
    public GameManager GameManager;

    private void Start()
    {
        var text = GetComponent<Text>();
        GameManager.amountMovement.Subscribe(value => text.text = value.ToString());
    }
}
