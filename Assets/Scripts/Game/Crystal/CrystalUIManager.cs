using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CrystalUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI crystalsText;
    [SerializeField] Crystals crystals;
    [SerializeField] Prices prices;
    [SerializeField] List<PriceTextsSerializable> texts;

    void OnEnable()
    {
        Crystals.OnCountCrystalsChange += UpdateUI;
    }

    void OnDisable()
    {
        Crystals.OnCountCrystalsChange -= UpdateUI;
    }

    void Start()
    {
        UpdateUI(crystals.crystals);
        InitPrices();
    }

    void InitPrices()
    {
        foreach (PriceTextsSerializable _text in texts)
        {
            float _price = 0f;
            switch (_text.gunType)
            {
                case GunType.Cannon:
                    _price = prices.createCannon;
                    break;
                case GunType.Crossbow:
                    _price = prices.createCrossbow;
                    break;
                case GunType.MagicCrystal:
                    _price = prices.createMagicCrystal;
                    break;
            }

            _text.text.text = _price.ToString();
        }
    }

    void UpdateUI(int _crystals)
    {
        crystalsText.text = _crystals.ToString();
    }
}

[Serializable]
public class PriceTextsSerializable
{
    public GunType gunType;
    public TextMeshProUGUI text;
}
