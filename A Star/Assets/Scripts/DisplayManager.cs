using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayManager : MonoBehaviour
{
    public Text star;
    public Text stamina;
    StarChaser starChaser;
    private void Start()
    {
        starChaser = (StarChaser)GameObject.FindObjectOfType(typeof(StarChaser));
    }

    // Update is called once per frame
    private void Update()
    {
        List<Entity> entites = GameManager.instance.tileManager.GetAllEntitiesOfType("FallenStar");
        star.text = "Collected Stars:" + starChaser.collectedStars.ToString() + " / " + entites.Count.ToString();
        stamina.text = "Left Stamina:" + starChaser.stamina + " / " + starChaser.maxStamina;
    }
}
