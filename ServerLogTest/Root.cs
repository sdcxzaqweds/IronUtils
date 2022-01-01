namespace ServerLogTest
{
    class Root
    {
        public void Init()
        {
            this.Log("InitRootLog.");
            Mgr mgr = new Mgr();
            mgr.Init();
        }
    }

    class Mgr
    {
        public void Init()
        {
            this.Warn("Init Mgr warn.");
            Item item = new Item();
            item.Init();
        }
    }

    class Item
    {
        public void Init()
        {
            this.Error("Init Item Error");
            this.Trace("Trace this Func");
        }
    }
}