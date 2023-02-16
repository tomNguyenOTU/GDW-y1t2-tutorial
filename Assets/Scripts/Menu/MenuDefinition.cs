using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MenuType
{
    HORIZONTAL, 
    VERTICAL
}
public class MenuDefinition : MonoBehaviour
{
    public MenuType _menuType = MenuType.HORIZONTAL;
    public AudioClip _menuMusic;
    public bool _continuePrevMusic;
    public List<GameObject> _menuButtonObjects = new List<GameObject>();

    private List<ButtonDefinition> _menuButtonDefinitions = new List<ButtonDefinition>();
    private List<Button> _menuButtons = new List<Button>();
    private List<Animator> _menuAnimators = new List<Animator>();

    public GameObject _backButton;
    private ButtonDefinition _backButtonDefinition;

    private void Start()
    {
        // search / grab components
        for (int i = 0; i < _menuButtonObjects.Count; i++)
        {
            _menuButtonDefinitions.Add(_menuButtonObjects[i].GetComponent<ButtonDefinition>());
            _menuButtons.Add(_menuButtonObjects[i].GetComponent<Button>());

            _backButtonDefinition = _backButton.GetComponent<ButtonDefinition>();

            // tries to grab an animator, assigns null otherwise
            Animator temp = null;
            _menuButtonObjects[i].TryGetComponent<Animator>(out temp); 
        }
    }

    public MenuType GetMenuType()
    {
        return _menuType;
    }

    public int GetButtonCount()
    {
        return _menuButtonObjects.Count;
    }

    public List<ButtonDefinition> GetButtonDefinitions()
    {
        return _menuButtonDefinitions;
    }

    public ButtonDefinition GetBackButton()
    {
        return _backButtonDefinition;
    }

    public List<Button> GetButtons()
    {
        return _menuButtons;
    }

    public List<Animator> GetAnimators() 
    {
        return _menuAnimators;
    }

    public bool GetContinueMusic()
    {
        return _continuePrevMusic;
    }
}