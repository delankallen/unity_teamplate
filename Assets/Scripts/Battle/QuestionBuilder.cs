using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class QuestionBuilder : MonoBehaviour
{
    [System.Serializable]
    public class Choice {
        public string Value;
        public bool Answer;
    }

    [System.Serializable]
    public class Question {
        
        public string Text;
        public List<Choice> Choices;
        public string Explanation;
    }
    public Question Q;
    public TextAsset QJson;
    public UIDocument UiDoc;
    
    public BattleHud battleHud;
    
    private VisualElement _rootEle;

    VisualElement RootElement {
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

    VisualElement ControlContainer {
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
    // Start is called before the first frame update
    void Start()
    {
        Q = JsonUtility.FromJson<Question>(QJson.text);
        Debug.Log($"Explanation: {Q.Explanation}");

        battleHud.SetDialogueText($"{Q.Text}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
