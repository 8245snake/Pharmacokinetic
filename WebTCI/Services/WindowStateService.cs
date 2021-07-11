using System;
using System.Timers;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using WebTCI.Models;


namespace WebTCI.Services
{
    public class WindowStateService
    {
        public static ComponentInfo ChartAreaInfo;

        public event EventHandler ChartAreaResized;

        private ComponentInfo ChartAreaInfo_save;


        private IJSRuntime JS;
        private Timer _WindowStateObserveTimer = new Timer();

        public WindowStateService()
        {
            _WindowStateObserveTimer.AutoReset = true;
            _WindowStateObserveTimer.Interval = 200;
            _WindowStateObserveTimer.Elapsed += OnTimedEvent;
            _WindowStateObserveTimer.Enabled = true;
        }

        public void SetJsRuntime(IJSRuntime js)
        {
            this.JS = js;
        }

        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            lock (ChartAreaInfo)
            {
                if (ChartAreaInfo == null)
                {
                    return;
                }

                if (!ChartAreaInfo.IsSame(ChartAreaInfo_save))
                {
                    ChartAreaInfo_save = ChartAreaInfo.DeepCopy();
                    RaiseWindowSizeChanged();
                    return;
                }

                ChartAreaInfo_save = ChartAreaInfo.DeepCopy();
            }
        }

        protected virtual void RaiseWindowSizeChanged()
        {
            ChartAreaResized?.Invoke(this, EventArgs.Empty);
        }

        public async Task<ComponentInfo> GetComponentInfo(string id)
        {
            return await JS.InvokeAsync<ComponentInfo>("getComponentInfo", id);
        }




        #region static

        [JSInvokable("WindowResized")]
        public static Task WindowResized(ComponentInfo info)
        {
            return Task.Factory.StartNew(() =>
            {
                if (info != null)
                {
                    lock (ChartAreaInfo)
                    {
                        ChartAreaInfo = info;
                    }
                }
            });
        }

        #endregion


    }
}
