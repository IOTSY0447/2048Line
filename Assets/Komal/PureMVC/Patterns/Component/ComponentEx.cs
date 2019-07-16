/* Brief: puremvc 扩展
 * Author: Komal
 * Date: "2019-07-05"
 */

namespace komal.puremvc {
    public class ComponentEx : UnityEngine.MonoBehaviour, INotificationHandler
    {
        /// <summary>Return the Singleton facade instance</summary>
        protected IFacade facade
        {
            get { return Facade.getInstance(); }
        }

        private static int g_Count = 0;
        private KMediator _mediator_ = null;

        protected virtual void Awake(){
            ++g_Count;
            this._mediator_ = new KMediator(g_Count.ToString(), this);
        }

        protected virtual void OnDestroy(){
            if(this._mediator_ != null){
                this._mediator_.OnDestroy();
            }
        }

        // can override by sub class
        public virtual string[] ListNotificationInterests()
        {
            return new string[0];
        }

        public virtual void HandleNotification(INotification notification)
        {
            // can override by sub class
        }
    }

    class KMediator : Mediator
    {
        public KMediator(string mediatorName, object _viewComponent = null) : base(mediatorName, _viewComponent)
        {
            this.facade.RegisterMediator(this);
        }

        public void OnDestroy(){
            this.facade.RemoveMediator(this.mediatorName);
        }

        public override string[] ListNotificationInterests()
        {
            var _viewComponent = this.viewComponent as ComponentEx;
            return _viewComponent.ListNotificationInterests();
        }

        public override void HandleNotification(INotification notification)
        {
            var _viewComponent = this.viewComponent as ComponentEx;
            _viewComponent.HandleNotification(notification);
        }
    }
}
