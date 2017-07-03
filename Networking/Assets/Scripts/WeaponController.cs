using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    Weapon equippedWeapon;
    public Weapon defaultWeapon;
    public Transform holster;

    void Start() {
        if (defaultWeapon != null) {
            Equip(defaultWeapon);
        }
    }

    public void Equip(Weapon _toEquip)
    {
        if (equippedWeapon != null) {
            Destroy(equippedWeapon.gameObject);
        }

        equippedWeapon = Instantiate(_toEquip, holster.position, holster.rotation) as Weapon;
        equippedWeapon.transform.parent = holster;
    }

    public void Shoot(Rigidbody _rigidbody, Vector3 _direction,float _force)
    {
        if (equippedWeapon != null) {
            bool applyForce;
            applyForce = equippedWeapon.Shoot();

            if (applyForce)
                _rigidbody.AddForce(_direction * _force);
        }
    }
}
