using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField] private PathFindingVisual m_pathFindingVisual;
    [SerializeField] private Bot m_bot;

    [SerializeField] private GameObject m_mainPanel;
    [SerializeField] private GameObject m_showMainPanelButton;

    [SerializeField] private GameObject m_restartButton;

    [SerializeField] private TextMeshProUGUI m_botSpeedText;
    [SerializeField] private Slider m_botSpeedSlider;

    void Update()
    {
        m_botSpeedText.text = m_botSpeedSlider.value.ToString();
        m_bot.SetSpeed(m_botSpeedSlider.value);
    }

    public void PressStart()
    {
        m_mainPanel.SetActive(false);
        m_restartButton.SetActive(true);
    }

    public void RestartButton()
    {
        // Restart the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void StartPathFinding()
    {
        m_bot.StartPathFinding();
        // Hide the main panel and show the restart button
        m_mainPanel.SetActive(false);
        m_restartButton.SetActive(true);
    }

    public void ShowFGHValues()
    {
        m_pathFindingVisual.ShowHideDebugText();
    }

    public void BotCanTravelDiagonally()
    {
        m_bot.SetCanTravelDiagonally();
    }

    public void ShowHideMainPanel()
    {
        m_mainPanel.SetActive(!m_mainPanel.activeSelf);
        m_showMainPanelButton.SetActive(!m_mainPanel.activeSelf);
    }
}
