using StatsManager;
using UnityEngine;

public class Stamina : StatsStamina
{
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void TakeStamina(int staminaTake)
    {
        base.TakeStamina(staminaTake);
    }
}
