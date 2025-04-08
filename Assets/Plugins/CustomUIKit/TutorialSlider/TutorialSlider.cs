using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialSlider : MonoBehaviour
{
    private VisualElement _root;
    private VisualElement _slider;
    private VisualElement _dragger;
    private VisualElement _bar;
    private VisualElement _newDragger;
    
    // Properties
    private float _newDraggerOffset = 0f;

    private void Start()
    {
        _root = GetComponent<UIDocument>().rootVisualElement;
        _slider = _root.Q<Slider>(className: "tutorial-slider");
        _dragger = _root.Q<VisualElement>("unity-dragger");
        
        AddElements();
        RegisterCallbacks();
    }

    private void AddElements()
    {
        _bar = new VisualElement();
        _bar.name = "Bar";
        _bar.AddToClassList("bar");
        _dragger.Add(_bar);

        _newDragger = new VisualElement();
        _newDragger.name = "NewDragger";
        _newDragger.AddToClassList("new-dragger");
        _newDragger.pickingMode = PickingMode.Ignore;
        _slider.Add(_newDragger);
    }

    private void RegisterCallbacks()
    {
        _slider.RegisterCallback<ChangeEvent<float>>(OnSliderValueChanged);
        _slider.RegisterCallback<GeometryChangedEvent>(OnSliderGeometryChanged);
    }

    private void OnSliderGeometryChanged(GeometryChangedEvent evt)
    {
        var distant = new Vector2((_newDragger.layout.width - _dragger.layout.width) / 2 - _newDraggerOffset,
            (_newDragger.layout.height - _dragger.layout.height) / 2 - _newDraggerOffset);
        var position = _dragger.parent.LocalToWorld(_dragger.transform.position);
        
        _newDragger.transform.position = _newDragger.parent.WorldToLocal(position - distant);
    }

    private void OnSliderValueChanged(ChangeEvent<float> evt)
    {
        var distant = new Vector2((_newDragger.layout.width - _dragger.layout.width) / 2 - _newDraggerOffset,
            (_newDragger.layout.height - _dragger.layout.height) / 2 - _newDraggerOffset);
        var position = _dragger.parent.LocalToWorld(_dragger.transform.position);
        
        _newDragger.transform.position = _newDragger.parent.WorldToLocal(position - distant);
    }
}
