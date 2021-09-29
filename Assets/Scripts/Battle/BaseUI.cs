using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BaseUI : MonoBehaviour
{
    private VisualElement _rootEle;
    public VisualElement RootElement {
        get {
            if (_rootEle == null)
            {
                _rootEle = GetComponent<UIDocument>().rootVisualElement;
            }
            return _rootEle;
        }
        set {
            _rootEle = value;
        }
    }
    private VisualElement _controlsContainer;

    public VisualElement ControlContainer {
        get {
            if (_controlsContainer == null) {
                _controlsContainer = RootElement.Q<VisualElement>("ControlsContainer");
            }
            return _controlsContainer;
        }
        set {
            _controlsContainer = value;
        }
    }
}
