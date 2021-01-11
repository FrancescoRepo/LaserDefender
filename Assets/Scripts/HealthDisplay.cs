using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthDisplay : MonoBehaviour
{
    private TextMeshProUGUI txtHealth;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        txtHealth = GetComponent<TextMeshProUGUI>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        txtHealth.text = player.GetHealth().ToString();
    }
}
