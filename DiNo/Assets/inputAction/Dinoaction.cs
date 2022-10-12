//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/inputAction/Dinoaction.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @Dinoaction : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @Dinoaction()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Dinoaction"",
    ""maps"": [
        {
            ""name"": ""Dino"",
            ""id"": ""29ee2f8c-4f20-44a7-9a32-e707011ca463"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Button"",
                    ""id"": ""ec9cbe7c-43a1-4404-9c83-356e5cda2c71"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""513a8f11-ddd8-44bc-979d-a817591752b3"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""KeyBoard"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""KeyBoard"",
            ""bindingGroup"": ""KeyBoard"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Dino
        m_Dino = asset.FindActionMap("Dino", throwIfNotFound: true);
        m_Dino_Move = m_Dino.FindAction("Move", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Dino
    private readonly InputActionMap m_Dino;
    private IDinoActions m_DinoActionsCallbackInterface;
    private readonly InputAction m_Dino_Move;
    public struct DinoActions
    {
        private @Dinoaction m_Wrapper;
        public DinoActions(@Dinoaction wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Dino_Move;
        public InputActionMap Get() { return m_Wrapper.m_Dino; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DinoActions set) { return set.Get(); }
        public void SetCallbacks(IDinoActions instance)
        {
            if (m_Wrapper.m_DinoActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_DinoActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_DinoActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_DinoActionsCallbackInterface.OnMove;
            }
            m_Wrapper.m_DinoActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
            }
        }
    }
    public DinoActions @Dino => new DinoActions(this);
    private int m_KeyBoardSchemeIndex = -1;
    public InputControlScheme KeyBoardScheme
    {
        get
        {
            if (m_KeyBoardSchemeIndex == -1) m_KeyBoardSchemeIndex = asset.FindControlSchemeIndex("KeyBoard");
            return asset.controlSchemes[m_KeyBoardSchemeIndex];
        }
    }
    public interface IDinoActions
    {
        void OnMove(InputAction.CallbackContext context);
    }
}