namespace komal.sdk {
    public class SDKBase : ILifeCycle
    {
        public virtual void OnInit() { }

        public virtual void OnPause() { }

        public virtual void OnResume() { }

        public virtual void OnDestroy() { }

        public virtual void OnUpdate() { }
    }
}
