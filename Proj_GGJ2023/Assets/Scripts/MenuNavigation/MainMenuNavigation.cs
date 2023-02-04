using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuNavigation : MonoBehaviour
{
    private UIDocument m_UIDocument;
    private VisualElement popUp;
    private VisualElement containerButtons;
    private Button buttonStart;
    private Button buttonGoCredits;
    private Button buttonCloseCredits;
    public const string GAME_SCENE = "GameScene";

    void Start()
    {
        m_UIDocument = GetComponent<UIDocument>();
        var root_element = m_UIDocument.rootVisualElement;
        popUp = root_element.Q<VisualElement>("pop-up");
        containerButtons = root_element.Q<VisualElement>("container-buttons");
        buttonStart = root_element.Q<Button>("btn-start");
        buttonStart.clickable.clicked += OnButtonClicked;
        buttonGoCredits = root_element.Q<Button>("btn-credits");
        buttonGoCredits.clickable.clicked += OnButtonClickedCredits;
        buttonCloseCredits = root_element.Q<Button>("btn-back");
        buttonCloseCredits.clickable.clicked += OnButtonCreditsClose;

    }

    private void OnButtonCreditsClose()
    {
        popUp.style.display = DisplayStyle.None;
        containerButtons.style.display = DisplayStyle.Flex;
    }

    private void OnButtonClickedCredits()
    {
        popUp.style.display = DisplayStyle.Flex;
        containerButtons.style.display = DisplayStyle.None;
    }

    private void OnButtonClicked()
    {
        SceneManager.LoadScene(GAME_SCENE);
    }
}
