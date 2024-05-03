using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public PlayerMovement movement;
    private float energy = 30f;
    private float energyModifierPerSecond = -1f;

    public Slider energySlider;
    public Text energyModfierText;

    Rigidbody rigidb;

    private bool isDead = false;

    public GameObject deadScreenUI;

    public Text highscoreText;
    public Text newHighscore;

    private float score = 0.0f;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 999999;
    private int scoreToNextLevel = 0;

    void Update()
    {

        if (isDead)
            return;

        if (score >= scoreToNextLevel)
            LevelUp();

        score += Time.deltaTime * 3;

        newHighscore.text = "New Highscore: " + ((int)score).ToString();
    }

    void LevelUp()
    {
        if (difficultyLevel == maxDifficultyLevel)
            return;

        scoreToNextLevel *= 2;
        difficultyLevel++;

        GetComponent<PlayerMovement>().SetForwardForce(difficultyLevel);
    }

    void Start()
    {
        energy = 30f;
        energyModifierPerSecond = -1f;
        rigidb = this.gameObject.GetComponent<Rigidbody>();
        highscoreText.text = "Highscore: " + ((int)PlayerPrefs.GetFloat("Highscore")).ToString();
        InvokeRepeating("UpdateEnergy", 1.0f, 1.0f);
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "Obstacle")
        {
            Die();
        }
    }

    private void UpdateEnergy()
    {
        if (isDead)
        {
            return;
        }
        if (energy <= 0)
        {
            Die();
        }
        if (energy >= 30)
        {
            energy = 30;
        }
        if (energyModifierPerSecond <= -3f)
        {
            energyModifierPerSecond = -3f;
        }
        if (energyModifierPerSecond >= 0)
        {
            energyModifierPerSecond = 0;
        }
        energy += energyModifierPerSecond;
        UpdateEnergyUI();
    }

    // Effect type is = Item type from the itemTypes integer array from ItemManager.
    // It is also there the value comes from.
    // This is done so it's to evaluate aka I'm lazy and think strings look a bit weird sometimes.
    // Anyways:
    //  1 = Vegetable
    //  2 = Meat
    //  3 = Fat groups
    //  4 = Sugar groups
    public void SetEnergyEffect(int effectType)
    {
        energy += 10;   // Add energy, as all the foods above give energy.
        Debug.Log($"Effect of type \"{effectType}\" has been set");

        // Add energy, as food gives energy.
        if (effectType == 1)
        {
            energy += 10f;
        }
        else if (effectType == 2)
        {
            energy += 15f;
        }
        else if (effectType == 3)
        {
            energy += 15f;
        }
        else if (effectType == 4)
        {
            energy += 20f;
        }

        if (effectType == 1)
        {
            energyModifierPerSecond -= -0.5f;   // So if it's a vegetable, you should loose energy slower.
        }
        else if (effectType == 2)
        {
            energyModifierPerSecond += -0.8f;
        }
        else if (effectType == 3)
        {
            energyModifierPerSecond += -0.4f;
        }
        else if (effectType == 4)
        {
            energyModifierPerSecond += -1f;
        }
        UpdateEnergy();
    }

    private void UpdateEnergyUI()
    {
        energyModfierText.text = energyModifierPerSecond.ToString();
        energySlider.value = energy;
    }

    public void Die()
    {
        deadScreenUI.SetActive(true);
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Score>().OnDeath();
        Time.timeScale = 0.3f;
    }
}
