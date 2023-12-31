using UnityEngine;
using UnityEngine.UIElements;

public class GameUI : UXBehaviour
{
    //public Label m_TotalScore;
    public AudioClip RailPlaceClick;
    public Label m_TotalCollectableCount;
    public Label m_TotalScore;
    public Label m_SpeedLabel;
    public Button m_leftButton;
    public Button m_middleButton;
    public Button m_rightButton;
    
    private int m_CurrScore;

    private void Awake()
    {
        InitializeDoc();

    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeDoc();
        //m_TotalScore = m_Root.Q<Label>("TotalScore");
        m_TotalCollectableCount = m_Root.Q<Label>("CollectibleCount");
        m_TotalScore = m_Root.Q<Label>("TotalScore");
        m_SpeedLabel = m_Root.Q<Label>("SpeedLabel");
        m_leftButton = m_Root.Q<Button>("left_btn");
        m_middleButton = m_Root.Q<Button>("straight_btn");
        m_rightButton = m_Root.Q<Button>("right_btn");
        m_leftButton.clicked += () => LeftButton();
        m_middleButton.clicked += () => MiddleButton();
        m_rightButton.clicked += () => RightButton();

        m_CurrScore = 0;

        ResetUI();

    }

    private void ResetUI()
    {
        m_TotalCollectableCount.text = "x0";
        m_TotalScore.text = "0";
        m_SpeedLabel.text = "20 km/h";
    }

    public void CollectableCount(int score)
    {
        m_TotalCollectableCount.text = "x"+score.ToString();
        m_CurrScore = score;

    }

    public void UpdateTotalScore(int score)
    {
        m_TotalScore.text = score.ToString();
    }
    

    public void UpdateSpeed(float speed)
    {
        float calculatedSpeed = speed / 1000.0f * 3600.0f;
        calculatedSpeed = Mathf.Round(calculatedSpeed * 100.0f) * 0.01f;

        m_SpeedLabel.text = calculatedSpeed + " km/h";
    }


    public void RestartScore()
    {
        m_CurrScore = 0;
    }

    public void LeftButton()
    {
        Train.Instance.TrackPlacement.SpawnTrainTrack(TrackType.TRACK_LEFT);
        UXManager.Instance.SoundManager.PlayOneShot("RailPlaceSound");
    }

    public void MiddleButton()
    {
        Train.Instance.TrackPlacement.SpawnTrainTrack(TrackType.TRACK_STRAIGHT);
        UXManager.Instance.SoundManager.PlayOneShot("RailPlaceSound");
    }

    public void RightButton()
    {
        Train.Instance.TrackPlacement.SpawnTrainTrack(TrackType.TRACK_RIGHT);
        UXManager.Instance.SoundManager.PlayOneShot("RailPlaceSound");
    }





}
