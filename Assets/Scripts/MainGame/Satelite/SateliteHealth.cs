using UnityEngine;
using UnityEngine.UI;

public class SateliteHealth : MonoBehaviour
{
    public int health;
    public int maxHealth = 100;
    public Slider hpSlider;
    public void Repair(int amount)
    {
        health += amount;
        hpSlider.value = health;
        if (health >= 60)
        {
            hpSlider.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = Color.green;
        }
        else if (health < 60 && health >= 40)
        {
            hpSlider.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = Color.yellowNice;
        }
        else if (health < 40)
        {
            hpSlider.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = Color.darkRed;
        }
        if (health>maxHealth)
        {
            health = maxHealth;
        }
     
    }
    public void StartUp()
    {
        hpSlider.maxValue = maxHealth;
        hpSlider.value = health;
        if(health >= 60)
        {
            hpSlider.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = Color.green;
        }
        else if (health < 60&& health >=40)
        {
            hpSlider.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = Color.yellowNice;
        }
        else if ( health < 40)
        {
            hpSlider.transform.Find("Fill Area").transform.Find("Fill").GetComponent<Image>().color = Color.darkRed;
        }
    }

    // Update is called once per frame
    void Update()
    {
    
    }
}
