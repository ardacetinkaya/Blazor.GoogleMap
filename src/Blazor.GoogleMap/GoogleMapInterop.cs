using Blazor.GoogleMap.Map;
using Blazor.GoogleMap.Map.Coordinates;
using Blazor.GoogleMap.Map.Events;
using Microsoft.JSInterop;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Blazor.GoogleMap
{
    public class GoogleMapInterop
    {
        private readonly IJSRuntime jSRuntime;
        private readonly IMouseEventsInovkable mouseEventsInovkable;

        public GoogleMapInterop(IJSRuntime jSRuntime, IMouseEventsInovkable mouseEventsInovkable)
        {
            this.jSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
            this.mouseEventsInovkable = mouseEventsInovkable ?? throw new ArgumentNullException(nameof(mouseEventsInovkable));
        }

        public async Task InitMap(InitialMapOptions initialMapOptions)
        {
            await jSRuntime.InvokeVoidAsync("blazorGoogleMap.initMap", initialMapOptions);
        }

        public async Task MoveToPosition(double latitude, double longitude)
        {
            Debug.Assert(latitude != 0);
            Debug.Assert(longitude != 0);

            await jSRuntime.InvokeVoidAsync("blazorGoogleMap.moveToPosition", latitude, longitude);
        }

        public async Task ExecuteInitMapCallback()
        {
            await jSRuntime.InvokeAsync<bool>("blazorGoogleMap.initMapCallback");
        }

        public async Task RegisterCallbacks(InitMapCallback initMapCallback)
        {
            var mouseEvents = DotNetObjectReference.Create<IMouseEventsInovkable>(mouseEventsInovkable);
            var mapCallBack = DotNetObjectReference.Create<InitMapCallback>(initMapCallback);

            await jSRuntime.InvokeAsync<object>("blazorGoogleMap.registerEventInvokers", mouseEvents, mapCallBack);

        }

        public async Task<object> CreateInfoWindow(string id, LatLng position)
        {
            return await jSRuntime.InvokeAsync<object>("blazorGoogleMap.createInfoWindow", id, position);
        }
    }
}
