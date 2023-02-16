using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

public class ButtonDefinition : MonoBehaviour
{
    public bool _animated = false;
    public Color _unselected = Color.gray;
    public Color _selected = Color.white;
    public bool _select = false;
    private Button _button;
    private Image _image;
    private Animator _animator;

    private bool _disableControls = false;

    public AudioClip _swaptoSFX;
    public AudioClip _selectSFX;
    public float selectTimer;

    // Start is called before the first frame update
    void Start()
    {
        _button = GetComponent<Button>();
        _image = GetComponent<Image>();

        _animated = TryGetComponent<Animator>(out _animator);

        if (!_animated)
        {
            if (_select)
                _image.color = _selected;
            else
                _image.color = _unselected;
        }
    }

    public void SwappedTo()
    {
        _select = true;

        if (_swaptoSFX != null)
        {
            AudioSource.PlayClipAtPoint(_swaptoSFX, Vector3.zero);
        }

        if (_animated)
            _animator.SetBool("Selected", _select);
        else
            _image.color = _selected; // color tint
    }

    public void SwappedOff()
    {
        _select = false;

        if (_animated)
            _animator.SetBool("Selected", _select);
        else
            _image.color = _unselected; // color tint
    }

    public IEnumerator ClickButton()
    {
        if (!_disableControls) // checks coroutine
        {
            _disableControls = true;

            if (_selectSFX != null)
            {
                AudioSource.PlayClipAtPoint(_selectSFX, Vector3.zero);
                Debug.Log("Selected");
            }

            yield return new WaitForSeconds(selectTimer);

            _button.onClick.Invoke();

            _disableControls = false;

        }
    }

    public bool GetDisableControls()
    {
        return _disableControls;
    }
}
