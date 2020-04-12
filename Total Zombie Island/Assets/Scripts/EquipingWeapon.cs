
using UnityEngine;

public class EquipingWeapon : MonoBehaviour
{
    public GameObject Shotgun;
    public GameObject SMG;
    public GameObject Hand;

    public void HandWeapon ()
    {
        Shotgun.gameObject.SetActive(false);
        SMG.gameObject.SetActive(false);
        Hand.gameObject.SetActive(true);
    }
    public void ShotgunWeapon ()
    {
        SMG.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(true);
    }
    public void SMGWeapon ()
    {
        Hand.gameObject.SetActive(false);
        Shotgun.gameObject.SetActive(false);
        SMG.gameObject.SetActive(true);
    }
}
