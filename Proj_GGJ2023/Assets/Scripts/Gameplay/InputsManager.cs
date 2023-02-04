using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class InputsManager : MonoBehaviour
{

    public TMP_InputField userInput;


    public void Start()
    {
        userInput.Select();
        userInput.onValueChanged.AddListener(OnPlayerWritting);
    }

    public void Update()
    {

        if (userInput.isFocused == false)
        {
            EventSystem.current.SetSelectedGameObject(userInput.gameObject, null);
            userInput.OnPointerClick(new PointerEventData(EventSystem.current));
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            var defensiveWord = userInput.text;

            defensiveWord = defensiveWord.ToLowerInvariant();
            defensiveWord = defensiveWord.Trim();
            Debug.Log("Sended response request: " + userInput.text);
            GameManager.Instance.PlayerSendedDefenseWord(defensiveWord);
            userInput.text = ("");
            userInput.ForceLabelUpdate();
        }
    }


    public void OnPlayerWritting(string newInput)
    {

    }


}
