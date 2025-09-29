using UnityEngine;

[System.Serializable]
public class Task
{
    public string id;
    public string title;
    public string description;
    public int coinReward;
    public bool isCompleted;
    
    public Task(string title, string description, int coinReward)
    {
        this.id = System.Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.coinReward = coinReward;
        this.isCompleted = false;
    }
    
    public void Complete()
    {
        if (!isCompleted)
        {
            isCompleted = true;
            Wallet.Instance?.AddCoins(coinReward);
        }
    }
}
