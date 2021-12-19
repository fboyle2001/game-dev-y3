using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICharacterActions {

    void StartMovement(Vector2 direction);

    void StopMovement();

    void StartSprinting();

    void StopSprinting();

    void OnMouseMoved(Vector2 direction);

}
