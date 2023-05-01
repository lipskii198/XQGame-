using UnityEngine;

namespace _game.Scripts.Managers
{
    public class ShopManager : MonoBehaviour
    {
        [SerializeField] private string currencyName = "Jungle Coin";
        [SerializeField] private string currencyShortName = "J$";
        [SerializeField] private bool allowSell;
        [SerializeField] private bool allowBuyback;
        [SerializeField] private int shopBaseCash = 1000;
    }
}