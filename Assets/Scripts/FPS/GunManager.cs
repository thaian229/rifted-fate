using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManager : MonoBehaviour
{
    public List<Gun> startingGuns = new List<Gun>();
    public Gun[] GunSlots = new Gun[2];  // Only 2 weapon at a time
    public Camera GunCamera;
    public Transform GunParentSocket;
    public Transform DefaultGunPosition;
    public int ActiveGunIndex { get; private set; }
    public LayerMask FpsGun;
    int m_newGunIndex;

    // Start is called before the first frame update
    void Start()
    {
        ActiveGunIndex = -1;
        foreach (var gun in startingGuns)
        {
            AddGun(gun);
        }

        SwitchGun();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool AddGun(Gun gunPrefab)
    {
        for (int i = 0; i < GunSlots.Length; i++)
        {
            if (GunSlots[i] == null)
            {
                // Spawn Gun as child of gun socket
                Gun gunInstance = Instantiate(gunPrefab, GunParentSocket);
                gunInstance.transform.localPosition = Vector3.zero;
                gunInstance.transform.localRotation = Quaternion.identity;

                gunInstance.Owner = this.gameObject;
                gunInstance.SourcePrefab = gunPrefab.gameObject;
                gunInstance.ShowWeapon(false);

                // Assign the first person layer to the weapon
                int layerIndex =
                    Mathf.RoundToInt(Mathf.Log(FpsGun.value,
                        2)); // This function converts a layermask to a layer index
                foreach (Transform t in gunInstance.gameObject.GetComponentsInChildren<Transform>(true))
                {
                    t.gameObject.layer = layerIndex;
                }

                GunSlots[i] = gunInstance;
                return true;
            }
        }
        return false;
    }

    public void SwitchGun()
    {
        int newGunIndex = 0;
        if (ActiveGunIndex == -1 || ActiveGunIndex == 1) newGunIndex = 0;
        else newGunIndex = 1;

        // Deactive old gun
        Gun oldGun = null;
        if (ActiveGunIndex >= 0) oldGun = GunSlots[ActiveGunIndex];
        if (oldGun != null)
        {
            oldGun.ShowWeapon(false);
        }

        // Activate new gun
        Gun newGun = GunSlots[newGunIndex];
        ActiveGunIndex = newGunIndex;
        newGun.ShowWeapon(true);
    }
}
