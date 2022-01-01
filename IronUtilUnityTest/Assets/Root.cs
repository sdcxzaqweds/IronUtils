
public class Root
{
    public void Init()
    {
        this.Log("InitRootLog.");
        Mgr mgr = new Mgr();
        mgr.Init();
    }
}

public class Mgr
{
    public void Init()
    {
        this.Warn("Init Mgr warn.");
        Item item = new Item();
        item.Init();
    }
}

public class Item
{
    public void Init()
    {
        this.Error("Init Item Error");
        this.Trace("Trace this Func");
    }
}