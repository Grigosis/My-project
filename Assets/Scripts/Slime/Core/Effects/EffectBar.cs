using ROR.Core;
using ROR.Lib;

public class EffectBar
{
    public readonly LList<EffectEntity> Effects = new LList<EffectEntity>();
    private LivingEntity Owner;

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
    }

    public void AddEffect(EffectEntity e)
    {
        if (e.Join(this))
        {
            return;
        }
        e.OnAdded();
        Effects.AddItem(e);
    } 
    
    public void RemoveEffect(EffectEntity e)
    {
        e.OnRemoved();
        Effects.RemoveItem(e);
    }
}