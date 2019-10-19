﻿using Blazor.GoogleMap.Map.InfoWindows;
using Microsoft.JSInterop;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blazor.GoogleMap.Map.Markers
{
    public class MarkerCollection : IMarkerCollection
    {
        private readonly Dictionary<Guid, Marker> markers;
        private readonly IJSRuntime jSRuntime;
        private readonly InfoWindow infoWindow;

        public int Count => markers.Count;

        public bool IsReadOnly => false;

        public Marker this[Guid markerId] => markers[markerId];

        public MarkerCollection(IJSRuntime jSRuntime, InfoWindow infoWindow)
        {
            this.jSRuntime = jSRuntime ?? throw new ArgumentNullException(nameof(jSRuntime));
            this.infoWindow = infoWindow ?? throw new ArgumentNullException(nameof(infoWindow));
            markers = new Dictionary<Guid, Marker>();
        }

        public async Task<Marker> Add(MarkerOptions markerOptions)
        {
            var marker = new Marker(markerOptions, jSRuntime, infoWindow);

            await jSRuntime.InvokeAsync<bool>(
                "blazorGoogleMap.markersModule.addMarker",
                DotNetObjectReference.Create<Marker>(marker), markerOptions, marker.Id);

            markers.Add(marker.Id, marker);

            return marker;
        }

        public async Task<bool> Remove(Marker marker)
        {
            var jsRemoveResult = await jSRuntime
                .InvokeAsync<bool>("blazorGoogleMap.markersModule.removeMarker", marker.Id);

            if (jsRemoveResult)
            {
                markers.Remove(marker.Id);

                return true;
            }

            return jsRemoveResult;
        }

        public bool Contains(Marker marker)
            => markers.ContainsKey(marker.Id);

        public async Task Clear()
        {

            var jsRemoveResult = await jSRuntime.InvokeAsync<bool>("blazorGoogleMap.markersModule.clear");

            if (jsRemoveResult)
            {
                markers.Clear();
            }


        }


        public IEnumerator<Marker> GetEnumerator()
            => markers.Values.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => markers.Values.GetEnumerator();
    }
}
