using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionArrow : MonoBehaviour
{
    [SerializeField] private RectTransform[] options;
    private RectTransform rect;
    private int currentPos;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }

        if(Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.J))
        {
            Interact();
        }
    }

    private void ChangeSelection(int _change)
    {
        currentPos += _change;

        if(currentPos < 0)
        {
            currentPos = options.Length - 1;
        }
        else if(currentPos > options.Length - 1)
        {
            currentPos = 0;
        }

        rect.position = new Vector3(rect.position.x, options[currentPos].position.y, 0);
    }

    private void Interact()
    {
        options[currentPos].GetComponent<Button>().onClick.Invoke();
    }
}
