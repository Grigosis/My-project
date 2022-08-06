using ROR.Core;
using ROR.Lib;
using UnityEngine;


[System.Serializable]
public class EffectBar
{
    public readonly LList<EffectEntity> Effects = new LList<EffectEntity>();
    private LivingEntity Owner;

    public int EffectsCount = 0;

    private int Id;
    private static int IdCounter;

    public EffectBar()
    {
        Id = IdCounter++;
    }


    public void Init(LivingEntity owner)
    {
        this.Owner = owner;
    }
    
    public void Update(float delta)
    {
        Effects.Apply();
        foreach (var e in Effects.Data)
        {
            e.Update(delta);
        }
        Effects.Apply();
        Debug.LogWarning($"{this} Update: effect");
    }

    public void AddEffect(EffectEntity e)
    {
        
        if (e.Join(this))
        {
            Debug.LogWarning($"{this} AddEffect: Join {e}");
            return;
        }
        e.OnAdded();
        EffectsCount++;
        Effects.AddItem(e);
        Effects.Apply();
        Debug.LogWarning($"{this} AddEffect: Added {e}");
    } 
    
    public void RemoveEffect(EffectEntity e)
    {
        EffectsCount--;
        e.OnRemoved();
        Effects.RemoveItem(e);
        Debug.LogWarning($"{this} RemoveEffect: "+e);
    }

    public override string ToString()
    {
        return $"EffectBar: {Id} {Owner.EntityId}";
    }
}