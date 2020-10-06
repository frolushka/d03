using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryWindow : MonoBehaviour
{
    [System.Serializable]
    public class RankItem
    {
        public string rank;
        public Vector2Int range;
    }
    
    [SerializeField] private Text scoreLabel;
    [SerializeField] private Text rankLabel;
    [SerializeField] private RankItem[] ranks;

    private void OnEnable()
    {
        scoreLabel.text = $"score: {GameManager.instance.Score}";
        rankLabel.text = $"rank: {GetRank(GameManager.instance.PlayerHp * GameManager.instance.PlayerEnergy)}";
    }

    private string GetRank(int rankScore)
    {
        for (var i = 0; i < ranks.Length; i++)
        {
            if (ranks[i].range.x <= rankScore && rankScore < ranks[i].range.y)
            {
                return ranks[i].rank;
            }
        }

        return null;
    }
}
