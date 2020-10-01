using UnityEngine;
using UnityEngine.UI;

public class HealthController : MonoBehaviour
{

    public float maxHealth;
    public Slider healthBarSlider;
    
    private float currentHealth;

    public void Init(float health)
    {
        maxHealth = health;
        currentHealth = health;
        GameObject canvas = transform.Find("Canvas").gameObject;
        healthBarSlider = canvas.transform.Find("HealthBar").gameObject.GetComponent<Slider>();
        healthBarSlider.value = 100;
    }

    public void Init(float health, GameObject parent, Transform camera)
    {
        maxHealth = health;
        currentHealth = health;

        Billboard bill = transform.GetChild(0).GetComponent<Billboard>();
        bill.cam = camera;

        GameObject canvas = transform.Find("Canvas").gameObject;
        healthBarSlider = canvas.transform.Find("HealthBar").gameObject.GetComponent<Slider>();
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public void ReduceHealth(float damage)
    {
        currentHealth -= damage;
        healthBarSlider.value = 100 * currentHealth / maxHealth;
    }


}
