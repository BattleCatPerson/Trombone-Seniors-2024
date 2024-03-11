using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallWardrobeTransition : MonoBehaviour
{
    [SerializeField] Wardrobe wardrobe;
    [SerializeField] WardrobeState state;
    public void Switch() => wardrobe.SwitchWardrobeState(state);
}
