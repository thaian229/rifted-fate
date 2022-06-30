using UnityEngine;
using UnityEngine.UI;

public class WorldspaceHealthBar : MonoBehaviour
{
    public Damageable Damageable;
    public Image HealthBarImage;
    public Transform HealthBarPivot;
    public bool HideFullHealthBar = true;

    void Update()
    {
        // update health bar value
        HealthBarImage.fillAmount = Damageable.health / Damageable.maxHP;

        // rotate health bar to face the camera/player
        HealthBarPivot.LookAt(Camera.main.transform.position);

        // hide health bar if needed
        if (HideFullHealthBar)
            HealthBarPivot.gameObject.SetActive(HealthBarImage.fillAmount != 1);
    }
}
