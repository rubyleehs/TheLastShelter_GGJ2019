using UnityEngine;
using UnityEngine.UI;

public class HPbar : MonoBehaviour
{
    public Transform target;
    LiveEntity targetState;
    public float offsetY; //indicate how bottom is the HPbar located at the target

    public Image healthBarUI;

    private void Start()
    {
        targetState = target.gameObject.GetComponent<LiveEntity>();
    }

    void Update()
    {
        //update HPbar position to target
        var wantedPos = Camera.main.WorldToScreenPoint(target.position);
        wantedPos += new Vector3(0, -offsetY, 0);
        transform.position = wantedPos;

        //update fill amount based on player current's hp value
        healthBarUI.fillAmount = (targetState.currentHP / 100f);
    }
}
