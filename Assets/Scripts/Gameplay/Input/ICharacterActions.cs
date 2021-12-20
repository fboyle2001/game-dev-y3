using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterActions {

    void StartMovement(Vector2 direction);

    void StopMovement();

    void StartSprinting();

    void StopSprinting();

    void StartLookAround(Vector2 direction);

    void StopLookAround();

    void StartAttack();

    void StopAttack();

    void Interact();

    void SetFrozen(bool frozen);

    bool IsFrozen();

}
