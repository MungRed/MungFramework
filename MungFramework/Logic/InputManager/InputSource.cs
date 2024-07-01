//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.8.2
//     from Assets/Plugins/MungRed/MungFramework/MungFramework/Logic/InputManager/InputSource.inputactions
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
using UnityEngine;

public partial class @InputSource: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSource()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSource"",
    ""maps"": [
        {
            ""name"": ""Controll"",
            ""id"": ""9f80f55d-a6d0-4150-b7bc-747496a233be"",
            ""actions"": [
                {
                    ""name"": ""MoveAxis"",
                    ""type"": ""Value"",
                    ""id"": ""9f8c2fb8-841f-43ec-b9dd-4c4771b1014c"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ViewAxis"",
                    ""type"": ""Value"",
                    ""id"": ""39689dd0-2e51-4c18-812f-cd95c2899b80"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""W"",
                    ""type"": ""Button"",
                    ""id"": ""00e75949-9cb2-46a7-974f-c664fe01acce"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""A"",
                    ""type"": ""Button"",
                    ""id"": ""314bfa1a-63ad-4ad0-b3b4-55a16a35d935"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""S"",
                    ""type"": ""Button"",
                    ""id"": ""594e760e-b863-4f3f-9235-7c49223d1959"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""D"",
                    ""type"": ""Button"",
                    ""id"": ""ab3397fb-8541-417a-84c1-9f6d394c5366"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""J"",
                    ""type"": ""Button"",
                    ""id"": ""e8982a92-3cdc-4129-a34a-9731c8e3bf96"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""K"",
                    ""type"": ""Button"",
                    ""id"": ""bce040cb-0225-4d76-a107-267615a3bf70"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Mouse"",
                    ""id"": ""fe4a0d7a-6a4b-4082-8f4b-52af7f7c1d5a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""4aafd0f8-ab04-4f94-b106-8afdb268a4c9"",
                    ""path"": ""<Mouse>/delta/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""1e599ec8-1655-44e3-8d43-4b7c7803ae3d"",
                    ""path"": ""<Mouse>/delta/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""606a40dc-0d30-4a9b-81f8-e03d45583753"",
                    ""path"": ""<Mouse>/delta/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""6802baef-eecd-494d-b2d1-9e0a2497954d"",
                    ""path"": ""<Mouse>/delta/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""RightStick"",
                    ""id"": ""ee17a9cc-5fd0-44f0-a0a1-9256e357cd30"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""9bbe9219-f183-47ec-acbe-42ed529b3b11"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""528a11d6-a5b0-43aa-9fff-248f2bb52606"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""925d10f9-3db4-4495-ae2d-eba4827a8aeb"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""c8515c1e-997c-4e68-b394-39c29760ec45"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""ViewAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""LeftStick"",
                    ""id"": ""523565f1-717d-4cb9-bc18-4ba6e2e9d409"",
                    ""path"": ""2DVector(mode=2)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Up"",
                    ""id"": ""09a804cb-d8e3-4130-b574-29a741cad1dc"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Down"",
                    ""id"": ""1c488273-4243-4fbd-8ee2-cd39c36ac8d6"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Left"",
                    ""id"": ""c6b591b1-e807-41d3-8962-6641737155a3"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""Right"",
                    ""id"": ""88cb59d3-d790-4b36-8411-5980077d1787"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""MoveAxis"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3e984ee1-9ede-4bac-b12c-8c5bacedd502"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""W"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""ee8d62c9-50ce-4264-8700-849b70d543f5"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""A"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7694838f-85a9-40a7-ba61-90bfe699f373"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""S"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""95fa60c3-3323-4691-9a6a-5841f2c6d3f8"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""D"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a842facf-d684-4bcf-bbdf-e32dbd79d6e1"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""J"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f04da90b-eead-4958-9a3b-92d37973c751"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""InputSource"",
                    ""action"": ""K"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""InputSource"",
            ""bindingGroup"": ""InputSource"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<XInputController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Controll
        m_Controll = asset.FindActionMap("Controll", throwIfNotFound: true);
        m_Controll_MoveAxis = m_Controll.FindAction("MoveAxis", throwIfNotFound: true);
        m_Controll_ViewAxis = m_Controll.FindAction("ViewAxis", throwIfNotFound: true);
        m_Controll_W = m_Controll.FindAction("W", throwIfNotFound: true);
        m_Controll_A = m_Controll.FindAction("A", throwIfNotFound: true);
        m_Controll_S = m_Controll.FindAction("S", throwIfNotFound: true);
        m_Controll_D = m_Controll.FindAction("D", throwIfNotFound: true);
        m_Controll_J = m_Controll.FindAction("J", throwIfNotFound: true);
        m_Controll_K = m_Controll.FindAction("K", throwIfNotFound: true);
    }

    ~@InputSource()
    {
        Debug.Assert(!m_Controll.enabled, "This will cause a leak and performance issues, InputSource.Controll.Disable() has not been called.");
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

    // Controll
    private readonly InputActionMap m_Controll;
    private List<IControllActions> m_ControllActionsCallbackInterfaces = new List<IControllActions>();
    private readonly InputAction m_Controll_MoveAxis;
    private readonly InputAction m_Controll_ViewAxis;
    private readonly InputAction m_Controll_W;
    private readonly InputAction m_Controll_A;
    private readonly InputAction m_Controll_S;
    private readonly InputAction m_Controll_D;
    private readonly InputAction m_Controll_J;
    private readonly InputAction m_Controll_K;
    public struct ControllActions
    {
        private @InputSource m_Wrapper;
        public ControllActions(@InputSource wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveAxis => m_Wrapper.m_Controll_MoveAxis;
        public InputAction @ViewAxis => m_Wrapper.m_Controll_ViewAxis;
        public InputAction @W => m_Wrapper.m_Controll_W;
        public InputAction @A => m_Wrapper.m_Controll_A;
        public InputAction @S => m_Wrapper.m_Controll_S;
        public InputAction @D => m_Wrapper.m_Controll_D;
        public InputAction @J => m_Wrapper.m_Controll_J;
        public InputAction @K => m_Wrapper.m_Controll_K;
        public InputActionMap Get() { return m_Wrapper.m_Controll; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ControllActions set) { return set.Get(); }
        public void AddCallbacks(IControllActions instance)
        {
            if (instance == null || m_Wrapper.m_ControllActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ControllActionsCallbackInterfaces.Add(instance);
            @MoveAxis.started += instance.OnMoveAxis;
            @MoveAxis.performed += instance.OnMoveAxis;
            @MoveAxis.canceled += instance.OnMoveAxis;
            @ViewAxis.started += instance.OnViewAxis;
            @ViewAxis.performed += instance.OnViewAxis;
            @ViewAxis.canceled += instance.OnViewAxis;
            @W.started += instance.OnW;
            @W.performed += instance.OnW;
            @W.canceled += instance.OnW;
            @A.started += instance.OnA;
            @A.performed += instance.OnA;
            @A.canceled += instance.OnA;
            @S.started += instance.OnS;
            @S.performed += instance.OnS;
            @S.canceled += instance.OnS;
            @D.started += instance.OnD;
            @D.performed += instance.OnD;
            @D.canceled += instance.OnD;
            @J.started += instance.OnJ;
            @J.performed += instance.OnJ;
            @J.canceled += instance.OnJ;
            @K.started += instance.OnK;
            @K.performed += instance.OnK;
            @K.canceled += instance.OnK;
        }

        private void UnregisterCallbacks(IControllActions instance)
        {
            @MoveAxis.started -= instance.OnMoveAxis;
            @MoveAxis.performed -= instance.OnMoveAxis;
            @MoveAxis.canceled -= instance.OnMoveAxis;
            @ViewAxis.started -= instance.OnViewAxis;
            @ViewAxis.performed -= instance.OnViewAxis;
            @ViewAxis.canceled -= instance.OnViewAxis;
            @W.started -= instance.OnW;
            @W.performed -= instance.OnW;
            @W.canceled -= instance.OnW;
            @A.started -= instance.OnA;
            @A.performed -= instance.OnA;
            @A.canceled -= instance.OnA;
            @S.started -= instance.OnS;
            @S.performed -= instance.OnS;
            @S.canceled -= instance.OnS;
            @D.started -= instance.OnD;
            @D.performed -= instance.OnD;
            @D.canceled -= instance.OnD;
            @J.started -= instance.OnJ;
            @J.performed -= instance.OnJ;
            @J.canceled -= instance.OnJ;
            @K.started -= instance.OnK;
            @K.performed -= instance.OnK;
            @K.canceled -= instance.OnK;
        }

        public void RemoveCallbacks(IControllActions instance)
        {
            if (m_Wrapper.m_ControllActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IControllActions instance)
        {
            foreach (var item in m_Wrapper.m_ControllActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ControllActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ControllActions @Controll => new ControllActions(this);
    private int m_InputSourceSchemeIndex = -1;
    public InputControlScheme InputSourceScheme
    {
        get
        {
            if (m_InputSourceSchemeIndex == -1) m_InputSourceSchemeIndex = asset.FindControlSchemeIndex("InputSource");
            return asset.controlSchemes[m_InputSourceSchemeIndex];
        }
    }
    public interface IControllActions
    {
        void OnMoveAxis(InputAction.CallbackContext context);
        void OnViewAxis(InputAction.CallbackContext context);
        void OnW(InputAction.CallbackContext context);
        void OnA(InputAction.CallbackContext context);
        void OnS(InputAction.CallbackContext context);
        void OnD(InputAction.CallbackContext context);
        void OnJ(InputAction.CallbackContext context);
        void OnK(InputAction.CallbackContext context);
    }
}
