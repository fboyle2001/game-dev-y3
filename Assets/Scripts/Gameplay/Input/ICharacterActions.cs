using UnityEngine;

/**
* Represents a set of actions that the user can make. Each character
* may handle these actions differently. These are called by the PlayerController
* class to align the actions available to the user.
**/
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

    void Jump();

    void SetFrozen(bool frozen);

    bool IsFrozen();

}
