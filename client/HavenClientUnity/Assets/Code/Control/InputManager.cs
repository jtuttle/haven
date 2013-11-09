using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum ButtonId {
    Confirm
    //Confirm, Cancel, Attack, Special,
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
    public delegate void AxialInputDelegate(float h, float v);
    public event AxialInputDelegate OnAxialInput = delegate { };

    public delegate void CameraInputDelegate(float h);
    public event CameraInputDelegate OnCameraInput = delegate { };

    private List<InputButton> _buttons;

    public void Awake() {
        _buttons = new List<InputButton>();

        foreach(ButtonId id in Enum.GetValues(typeof(ButtonId)))
            _buttons.Add(new InputButton(id));
    }

    public void Update() {
        OnAxialInput(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        OnCameraInput(Input.GetAxis("Camera"));
        
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
