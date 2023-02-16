using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject _activeMenu;
    public AudioSource _bgAudio;

    public List<KeyCode> _increaseVert;
    public List<KeyCode> _decreaseVert;
    public List<KeyCode> _increaseHori;
    public List<KeyCode> _decreaseHori;
    public List<KeyCode> _confirmButton;
    public List<KeyCode> _cancelButton;

    [SerializeField] private MenuDefinition _activeMenuDefinition;
    [SerializeField] private int _activeButton = 0;

    public void Start()
    {
        // update active menu
        UpdateActiveMenuDefinition();
    }

    public void Update()
    {
        switch (_activeMenuDefinition.GetMenuType())
        {
            case MenuType.HORIZONTAL:
                MenuInput(_increaseHori, _decreaseHori);
                break;
            case MenuType.VERTICAL:
                MenuInput(_increaseVert, _decreaseVert);
                break;
        }
    }

    private void MenuInput(List<KeyCode> increase, List<KeyCode> decrease)
    {
        int newActive = _activeButton;

        for (int i = 0; i < increase.Count; i++)
        {
            if (Input.GetKeyDown(increase[i]))
            {
                newActive = SwitchCurrentButton(1);
            }
        }

        for (int i = 0; i < decrease.Count; i++)
        {
            if (Input.GetKeyDown(decrease[i]))
            {
                newActive = SwitchCurrentButton(-1);
            }
        }

        for (int i = 0; i < _confirmButton.Count; i++)
        {
            if (Input.GetKeyDown(_confirmButton[i]))
            {
                ClickCurrentButton();
            }
        }

        for (int i = 0; i < _cancelButton.Count; i++)
        {
            if (Input.GetKeyDown(_cancelButton[i]))
            {
                ClickBackButton();
            }
        }

        _activeButton = newActive;
    }

    private void ClickCurrentButton()
    {
        if (!_activeMenuDefinition.GetButtonDefinitions()[_activeButton].GetDisableControls())
        {
            StartCoroutine(_activeMenuDefinition.GetButtonDefinitions()[_activeButton].ClickButton());
        }
    }

    private void ClickBackButton()
    {
        if (!_activeMenuDefinition.GetBackButton().GetDisableControls())
        {
            StartCoroutine(_activeMenuDefinition.GetBackButton().ClickButton());
        }
    }

    private int SwitchCurrentButton(int increment)
    {
        if (!_activeMenuDefinition.GetButtonDefinitions()[_activeButton].GetDisableControls())
        {
            int newActive = Utility.WrapAround(_activeMenuDefinition.GetButtonCount(), _activeButton, increment);

            _activeMenuDefinition.GetButtonDefinitions()[_activeButton].SwappedOff();
            _activeMenuDefinition.GetButtonDefinitions()[newActive].SwappedTo();

            return newActive;
        }

        return _activeButton;
    }

    public void UpdateActiveMenuDefinition()
    {
        _activeMenuDefinition = _activeMenu.GetComponent<MenuDefinition>();

        if (_activeMenuDefinition._menuMusic != null ) 
        {
            _bgAudio.clip = _activeMenuDefinition._menuMusic;
            _bgAudio.Play();
        }
        else if (!_activeMenuDefinition.GetContinueMusic())
        {
            _bgAudio.Stop();
        }
    }

    public void SetActiveMenu(GameObject activeMenu)
    {
        _activeMenu = activeMenu;
        UpdateActiveMenuDefinition();
    }
}
