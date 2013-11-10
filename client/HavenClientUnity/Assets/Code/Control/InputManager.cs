using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ButtonId {
    Confirm, Cancel, Action, Special,
    TriggerLeft, TriggerRight
    //Previous, Next
}

public class InputButton {
    public delegate void ButtonInputDelegate();
    public event ButtonInputDelegate OnPress = delegate { };

    public ButtonId Id { get; private set; }

    private bool _pressed;

    public InputButton(ButtonId id) {
        Id = id;

        _pressed = false;
    }

    public void CheckPress() {
        bool pressed = Input.GetButton(Id.ToString());
        if(!pressed) _pressed = false;

        if(pressed && !_pressed) {
            _pressed = true;
            OnPress();
        }
    }
}

public class InputManager : MonoBehaviour {
    public delegate void AxialInputLeftDelegate(float h, float v);
    public event AxialInputLeftDelegate OnAxialLeftInput = delegate { };

    public delegate void AxialInputRightDelegate(float h, float v);
    public event AxialInputRightDelegate OnAxialRightInput = delegate { };

    public delegate void TriggerRightDelegate();
    public event TriggerRightDelegate OnTriggerRightInput = delegate { };

    /*
    public delegate void TriggerDelegate(float h, float v);
    public event TriggerDelegate OnTriggerInput = delegate { };
    */

    private bool _trigRightPressed;

    private List<InputButton> _buttons;

    public void Awake() {
        _buttons = new List<InputButton>();

        foreach(ButtonId id in Enum.GetValues(typeof(ButtonId)))
            _buttons.Add(new InputButton(id));

        _trigRightPressed = false;
    }

    public void Update() {
        OnAxialLeftInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        OnAxialRightInput(Input.GetAxis("RightHorizontal"), Input.GetAxis("RightVertical"));

        // right trigger
        bool trigRightPressed = Input.GetAxis("TriggerRight") > 0;
        if(!trigRightPressed) _trigRightPressed = false;

        if(trigRightPressed && !_trigRightPressed) {
            _trigRightPressed = true;
            OnTriggerRightInput();
        }

        foreach(InputButton button in _buttons)
            button.CheckPress();
	}

    public InputButton GetButton(ButtonId id) {
        foreach(InputButton button in _buttons) {
            if(button.Id == id) return button;
        }
        return null;
    }
}
